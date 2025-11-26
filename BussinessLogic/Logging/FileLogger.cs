using System;
using System.IO;
using System.Text;

namespace BussinessLogic.Logging
{
	/// <summary>
	/// Реализация логгера, записывающего сообщения в файл на диске.
	/// Обеспечивает потокобезопасную запись и автоматическое создание директории.
	/// </summary>
	public sealed class FileLogger : ILogger
	{
		private readonly object _sync = new object();
		private readonly string _logFilePath;

		/// <summary>
		/// Инициализирует новый экземпляр логгера с указанной директорией и именем файла.
		/// </summary>
		/// <param name="logDirectory">Директория для файла журнала. Если null, используется рабочий стол пользователя.</param>
		/// <param name="fileName">Имя файла журнала. По умолчанию "actions.log".</param>
		public FileLogger(string? logDirectory = null, string fileName = "actions.log")
		{
			var directory = logDirectory ?? Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
			_logFilePath = Path.Combine(directory, fileName);
		}

		/// <summary>
		/// Записывает сообщение в файл журнала с временной меткой.
		/// </summary>
		/// <param name="message">Текст сообщения для записи.</param>
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

			}
		}
	}
}

