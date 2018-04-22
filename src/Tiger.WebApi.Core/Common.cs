using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using Tiger.WebApi.Core.Constants;
using Tiger.WebApi.Core.Service;
using Tiger.WebApi.Core.Service.Attributes;

namespace Tiger.WebApi.Core
{
    public sealed class Common
    {
        /// <summary>
        /// 签名算法
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="secret"></param>
        /// <param name="signMethod"></param>
        /// <returns></returns>
        public static string BuildSign(IDictionary<string, string> parameters, string secret, string signMethod)
        {
            // 第一步：把字典按Key的字母顺序排序
            IDictionary<string, string> sortedParams = new SortedDictionary<string, string>(parameters, StringComparer.Ordinal);
            IEnumerator<KeyValuePair<string, string>> dem = sortedParams.GetEnumerator();

            // 第二步：把所有参数名和参数值串在一起
            StringBuilder query = new StringBuilder();
            if (CommonConstant.SIGN_METHOD_MD5.Equals(signMethod))
            {
                query.Append(secret);
            }
            while (dem.MoveNext())
            {
                string key = dem.Current.Key;
                string value = dem.Current.Value;
                if (!string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(value))
                {
                    query.Append(key).Append(value);
                }
            }

            // 第三步：使用MD5/HMAC加密
            byte[] bytes;
            if (CommonConstant.SIGN_METHOD_HMAC.Equals(signMethod))
            {
                HMACMD5 hmac = new HMACMD5(Encoding.UTF8.GetBytes(secret));
                bytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(query.ToString()));
            }
            else
            {
                query.Append(secret);
                MD5 md5 = MD5.Create();
                bytes = md5.ComputeHash(Encoding.UTF8.GetBytes(query.ToString()));
            }

            // 第四步：把二进制转化为大写的十六进制
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                result.Append(bytes[i].ToString("X2"));
            }

            return result.ToString();
        }

        /// <summary>
        /// 添加Service
        /// </summary>
        /// <param name="type"></param>
        public static void AddService(Type type)
        {
            MethodAttribute attr = (MethodAttribute)type.GetCustomAttribute(typeof(MethodAttribute));
            if (attr != null && typeof(ITigerMethod).IsAssignableFrom(type))
            {
                Global.Service.Add(attr.Name, type);
            }
        }

        /// <summary>
        /// 添加Service
        /// </summary>
        /// <param name="packageItems"></param>
        public static void AddService(params PackageItem[] packageItems)
        {
            foreach (PackageItem packageItem in packageItems)
            {
                // 设置PackageItem(重复出现则会覆盖)
                Global.PackageItem[packageItem.AssemblyName] = packageItem;

                foreach (Type typeItem in packageItem.Assembly.GetExportedTypes())
                {
                    AddService(typeItem);
                }
            }
        }

        public static void RemoveService(string itemFullPath)
        {
            FileInfo fileInfo = new FileInfo(itemFullPath);
            if (Global.PackageItem.ContainsKey(fileInfo.DirectoryName))
            {
                PackageItem packageItem = Global.PackageItem[itemFullPath];
                foreach (Type type in packageItem.Assembly.GetExportedTypes())
                {
                    MethodAttribute attr = (MethodAttribute)type.GetCustomAttribute(typeof(MethodAttribute));
                    if (attr != null && typeof(ITigerMethod).IsAssignableFrom(type))
                    {
                        if (Global.Service.ContainsKey(attr.Name))
                        {
                            Global.Service.Remove(attr.Name);
                        }
                    }
                }
                Global.PackageItem.Remove(fileInfo.DirectoryName);
            }
        }
    }
}