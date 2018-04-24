using System;
using System.Collections.Generic;
using System.IO;
using Tiger.WebApi.Core.Constants;

namespace Tiger.WebApi.Core.IO
{
    /// <summary>
    /// 文件监听
    /// </summary>
    public class FileWatcher
    {
        string _path;
        bool _isWatch = false;
        FileSystemWatcher _watcher = null;
        // 记录最后修改事件，防止重复触发Changed事件
        Dictionary<string, DateTime> _fileLastUpdateTime = null;
        public FileWatcher(string path)
        {
            if (!Directory.Exists(path))
            {
                throw new Exception($"路径{path}不存在");
            }
            _path = path;
        }
        /// <summary>
        /// 验证文件夹是不是根目录
        /// </summary>
        /// <param name="dirInfo"></param>
        /// <returns></returns>
        private bool VerifyNotPackagePath(string fullPath)
        {
            FileInfo fileInfo = new FileInfo(fullPath);
            DirectoryInfo packageDirInfo = new DirectoryInfo(_path);
            return !packageDirInfo.FullName.Equals(fileInfo.Directory.FullName);
        }
        /// <summary>
        /// 运行
        /// </summary>
        public void Run()
        {
            _watcher = new FileSystemWatcher(_path)
            {
                // 包含子目录
                IncludeSubdirectories = true,
                // 启用
                EnableRaisingEvents = true,
            };

            _watcher.Created += new FileSystemEventHandler(CreatedEvent);
            _watcher.Deleted += new FileSystemEventHandler(DeletedEvent);
            _watcher.Changed += new FileSystemEventHandler(ChangedEvent);
            _watcher.Renamed += new RenamedEventHandler(RenamedEvent);

            _fileLastUpdateTime = new Dictionary<string, DateTime>();

            _isWatch = true;
        }
        /// <summary>
        /// 停止
        /// </summary>
        public void Stop()
        {
            _watcher.Dispose();
            _isWatch = false;
            _fileLastUpdateTime = null;
        }

        /// <summary>
        /// 监听中
        /// </summary>
        public bool IsWatch { get => _isWatch; }

        public event Action<FileChangeType, string> Changed;

        #region Event
        /// <summary>
        /// 文件创建事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CreatedEvent(object sender, FileSystemEventArgs e)
        {
            string path = "";
            // 文件夹
            if (!VerifyNotPackagePath(e.FullPath) && FileHelper.IsDirectory(e.FullPath))
            {
                path = Path.Combine(e.FullPath, CommonConstant.PACKAGE_ITEM);
                if (!File.Exists(path))
                    return;
            }
            // 文件
            else if (VerifyNotPackagePath(e.FullPath))
            {
                FileInfo fileInfo = new FileInfo(e.FullPath);
                path = Path.Combine(fileInfo.DirectoryName, CommonConstant.PACKAGE_ITEM);

                if (!File.Exists(path))
                    return;
            }
            else
            {
                return;
            }

            DateTime currFileLastUpdateTime = File.GetLastWriteTime(path);
            // 解决重复事件问题
            if (_fileLastUpdateTime.ContainsKey(path))
            {
                if (currFileLastUpdateTime == _fileLastUpdateTime[path])
                {
                    return;
                }
                else
                {
                    _fileLastUpdateTime[path] = currFileLastUpdateTime;
                }
            }
            else
            {
                _fileLastUpdateTime.Add(path, currFileLastUpdateTime);
            }

            Changed?.Invoke(FileChangeType.Create, path);
        }
        /// <summary>
        /// 文件删除事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeletedEvent(object sender, FileSystemEventArgs e)
        {
            foreach (var item in Global.PackageItem)
            {
                if (item.Value.Directory.Equals(e.FullPath))
                {
                    Changed?.Invoke(FileChangeType.Delete, Path.Combine(e.FullPath, CommonConstant.PACKAGE_ITEM));
                    return;
                }
            }
            FileInfo fileInfo = new FileInfo(e.FullPath);
            string path = Path.Combine(fileInfo.DirectoryName, CommonConstant.PACKAGE_ITEM);
            if (fileInfo.FullName.Equals(path))
            {
                if (_fileLastUpdateTime.ContainsKey(path))
                {
                    _fileLastUpdateTime.Remove(path);
                }
                Changed?.Invoke(FileChangeType.Delete, path);
            }

        }
        /// <summary>
        /// 文件修改事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChangedEvent(object sender, FileSystemEventArgs e)
        {
            if (!VerifyNotPackagePath(e.FullPath))
                return;

            FileInfo fileInfo = new FileInfo(e.FullPath);
            string path = Path.Combine(fileInfo.DirectoryName, CommonConstant.PACKAGE_ITEM);

            if (!File.Exists(path))
                return;

            DateTime currFileLastUpdateTime = File.GetLastWriteTime(path);
            // 解决重复出发Change事件问题
            if (_fileLastUpdateTime.ContainsKey(path))
            {
                if (currFileLastUpdateTime == _fileLastUpdateTime[path])
                {
                    return;
                }
                else
                {
                    _fileLastUpdateTime[path] = currFileLastUpdateTime;
                }
            }
            else
            {
                _fileLastUpdateTime.Add(path, currFileLastUpdateTime);
            }

            Changed?.Invoke(FileChangeType.Update, e.FullPath);
        }
        /// <summary>
        /// 文件重名事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RenamedEvent(object sender, RenamedEventArgs e)
        {
            if (FileHelper.IsDirectory(e.FullPath))
            {
                string oldPath = Path.Combine(e.OldFullPath, CommonConstant.PACKAGE_ITEM);
                if (_fileLastUpdateTime.ContainsKey(oldPath))
                {
                    _fileLastUpdateTime.Remove(oldPath);
                }
                string path = Path.Combine(e.FullPath, CommonConstant.PACKAGE_ITEM);
                Changed?.Invoke(FileChangeType.Delete, oldPath);

                if (!File.Exists(path))
                    return;

                Changed?.Invoke(FileChangeType.Create, path);
            }
            else
            {
                FileInfo fileInfo = new FileInfo(e.FullPath);
                string path = Path.Combine(fileInfo.DirectoryName, CommonConstant.PACKAGE_ITEM);
                // 如果是 Item 移除
                if (!fileInfo.Name.Equals(CommonConstant.PACKAGE_ITEM))
                {
                    if (_fileLastUpdateTime.ContainsKey(path))
                    {
                        _fileLastUpdateTime.Remove(path);
                    }
                    Changed?.Invoke(FileChangeType.Delete, path);
                }
                else
                {
                    if (_fileLastUpdateTime.ContainsKey(fileInfo.DirectoryName))
                    {
                        _fileLastUpdateTime[fileInfo.DirectoryName] = File.GetLastWriteTime(path);
                    }
                    Changed?.Invoke(FileChangeType.Update, path);
                }
            }
        }
        #endregion
    }
}
