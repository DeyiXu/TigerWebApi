using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using Tiger.WebApi.Core.Collections.Generic;

namespace Tiger.WebApi.Core.IO
{
    public class ThreadFileWatcher : IFileWatcher
    {
        readonly string _path;
        Thread _thread = null;
        readonly SyncDictionary<string, string> _fileDic = new SyncDictionary<string, string>();
        public ThreadFileWatcher(string path)
        {
            if (!Directory.Exists(path))
            {
                throw new Exception($"路径{path}不存在");
            }
            _path = path;


        }
        public bool IsWatch { get; private set; } = false;

        public event Action<FileChangeType, string> Changed;

        public void Run()
        {
            IsWatch = true;

            FileInfo[] fileInfos = GetFileInfos(_path);
            for (int i = 0; i < fileInfos.Length; i++)
            {
                _fileDic.Add(fileInfos[i].FullName, GetFileHash(fileInfos[i].FullName));
            }

            _thread = new Thread(Watcher)
            {
                IsBackground = true
            };

            _thread.Start();
        }

        public void Stop()
        {
            IsWatch = false;
        }

        private string GetFileHash(string filePath)
        {
            MD5 md5 = MD5.Create();
            byte[] bytes = md5.ComputeHash(File.ReadAllBytes(filePath));
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                result.Append(bytes[i].ToString("X2"));
            }
            return result.ToString();
        }

        private FileInfo[] GetFileInfos(string path)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(path);
            return directoryInfo.GetFiles("*.dll");
        }

        private void Watcher()
        {
            while (IsWatch)
            {
                Thread.CurrentThread.Join(10000);

                string[] keys1 = _fileDic.Keys.ToArray();
                string[] keys2 = GetFileInfos(_path).Select(s => s.FullName).ToArray();

                string[] containsKey = keys1.Intersect(keys2).ToArray();
                List<string> notContainsKey = new List<string>();
                notContainsKey.AddRange(keys1.Except(keys2).ToArray());
                notContainsKey.AddRange(keys2.Except(keys1).ToArray());

                for (int i = 0; i < containsKey.Length; i++)
                {
                    string fullName = containsKey[i];
                    string hash = GetFileHash(fullName);
                    if (!_fileDic[fullName].Equals(hash))
                    {
                        _fileDic[fullName] = hash;
                        Changed?.Invoke(FileChangeType.Update, fullName);
                    }
                }

                for (int i = 0; i < notContainsKey.Count; i++)
                {
                    string fullName = notContainsKey[i];
                    if (File.Exists(fullName))
                    {
                        _fileDic.Add(fullName, GetFileHash(fullName));
                        Changed?.Invoke(FileChangeType.Create, fullName);
                    }
                    else
                    {
                        _fileDic.Remove(fullName);
                        Changed?.Invoke(FileChangeType.Delete, fullName);
                    }
                }
            }
        }

    }
}
