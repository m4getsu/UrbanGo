using System;
using System.IO;
using System.Text;

namespace BussinessLogic.Logging
{
	public sealed class FileLogger : ILogger
	{
		private readonly object _sync = new object();
		private readonly string _logFilePath;

		public FileLogger(string? logDirectory = null, string fileName = "actions.log")
		{
			var directory = logDirectory ?? Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
			_logFilePath = Path.Combine(directory, fileName);
		}

		public void Log(string message)
		{
			try
			{
				var line = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {message}" + Environment.NewLine;
				lock (_sync)
				{
					var dir = Path.GetDirectoryName(_logFilePath);
					if (!string.IsNullOrEmpty(dir))
					{
						Directory.CreateDirectory(dir);
					}
					File.AppendAllText(_logFilePath, line, Encoding.UTF8);
				}
			}
			catch
			{
				// намеренно подавляем ошибки логирования
			}
		}
	}
}

