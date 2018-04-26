using System;
namespace Tiger.WebApi.Core.IO
{
	public interface IFileWatcher
	{
		/// <summary>
		/// 监听中
		/// </summary>
		bool IsWatch { get; }
		event Action<FileChangeType, string> Changed;
		/// <summary>
		/// 运行
		/// </summary>
		void Run();
		/// <summary>
		/// 停止
		/// </summary>
		void Stop();
	}
}