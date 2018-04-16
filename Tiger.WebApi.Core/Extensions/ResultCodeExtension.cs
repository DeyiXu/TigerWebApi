using System;
using System.Reflection;
using Tiger.WebApi.Core.Attributes;
using Tiger.WebApi.Core.Result;

namespace Tiger.WebApi.Core.Extensions
{
    public static class ResultCodeExtension
    {
        public static string ToMessage(this ResultCode code)
        {
            FieldInfo field = code.GetType().GetField(code.ToString());
            ResultCodeMessageAttribute msg = (ResultCodeMessageAttribute)field.GetCustomAttribute(typeof(ResultCodeMessageAttribute));
            if (msg == null)
                return "";

            return msg.Message ?? "";
        }
    }
}
