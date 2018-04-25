using System;
using System.Collections.Generic;
using System.IO;

namespace Tiger.WebApi.Core.IO
{
	/// <summary>
	/// 文件监听
	/// </summary>
	public class FileWatcher
	{
		readonly string _path;
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
				IncludeSubdirectories = false,
				// 启用
				EnableRaisingEvents = true,
				Filter = "*.dll"
			};

			_watcher.Created += new FileSystemEventHandler(CreatedEvent);
			_watcher.Deleted += new FileSystemEventHandler(DeletedEvent);
			_watcher.Changed += new FileSystemEventHandler(ChangedEvent);
			_watcher.Renamed += new RenamedEventHandler(RenamedEvent);

			_fileLastUpdateTime = new Dictionary<string, DateTime>();

			IsWatch = true;
		}
		/// <summary>
		/// 停止
		/// </summary>
		public void Stop()
		{
			_watcher.Dispose();
			IsWatch = false;
			_fileLastUpdateTime = null;
		}

		/// <summary>
		/// 监听中
		/// </summary>
		public bool IsWatch { get; private set; } = false;

		public event Action<FileChangeType, string> Changed;

		#region Event
		/// <summary>
		/// 文件创建事件
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void CreatedEvent(object sender, FileSystemEventArgs e)
		{
			DateTime currFileLastUpdateTime = File.GetLastWriteTime(e.FullPath);
			// 解决重复事件问题
			if (_fileLastUpdateTime.ContainsKey(e.FullPath))
			{
				if (currFileLastUpdateTime == _fileLastUpdateTime[e.FullPath])
				{
					return;
				}
				else
				{
					_fileLastUpdateTime[e.FullPath] = currFileLastUpdateTime;
				}
			}
			else
			{
				_fileLastUpdateTime.Add(e.FullPath, currFileLastUpdateTime);
			}

			Changed?.Invoke(FileChangeType.Create, e.FullPath);
		}
		/// <summary>
		/// 文件删除事件
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void DeletedEvent(object sender, FileSystemEventArgs e)
		{
			if (_fileLastUpdateTime.ContainsKey(e.FullPath))
			{
				_fileLastUpdateTime.Remove(e.FullPath);
			}

			Changed?.Invoke(FileChangeType.Delete, e.FullPath);
		}
		/// <summary>
		/// 文件修改事件
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ChangedEvent(object sender, FileSystemEventArgs e)
		{
			DateTime currFileLastUpdateTime = File.GetLastWriteTime(e.FullPath);
			// 解决重复出发Change事件问题
			if (_fileLastUpdateTime.ContainsKey(e.FullPath))
			{
				if (currFileLastUpdateTime == _fileLastUpdateTime[e.FullPath])
				{
					return;
				}
				else
				{
					_fileLastUpdateTime[e.FullPath] = currFileLastUpdateTime;
					Changed?.Invoke(FileChangeType.Update, e.FullPath);
				}
			}
			else
			{
				_fileLastUpdateTime.Add(e.FullPath, currFileLastUpdateTime);
				Changed?.Invoke(FileChangeType.Update, e.FullPath);
			}
		}
		/// <summary>
		/// 文件重名事件
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void RenamedEvent(object sender, RenamedEventArgs e)
		{
			if (_fileLastUpdateTime.ContainsKey(e.OldFullPath))
			{
				_fileLastUpdateTime.Remove(e.OldFullPath);
				Changed?.Invoke(FileChangeType.Delete, e.OldFullPath);
				return;
			}

			DateTime currFileLastUpdateTime = File.GetLastWriteTime(e.FullPath);
			if (!_fileLastUpdateTime.ContainsKey(e.FullPath))
			{
				_fileLastUpdateTime.Add(e.FullPath, currFileLastUpdateTime);
			}
			else
			{
				_fileLastUpdateTime[e.FullPath] = currFileLastUpdateTime;
			}

			Changed?.Invoke(FileChangeType.Update, e.FullPath);
		}
		#endregion
	}
}
