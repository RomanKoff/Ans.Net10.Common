// rev 2026-09-19

using System.Text;

namespace Ans.Net10.Common
{

	/// <summary>
	/// Хелпер буферизированной записи логов в файл порциями для минимизации дисковых операций ввода-вывода.
	/// </summary>
	public class TinyLogWriterHelper
	{

		private StringBuilder _sb = null!;
		private int _count;


		/* ctor */


		/// <summary>
		/// Инициализирует новый экземпляр класса <see cref="TinyLogWriterHelper"/>.
		/// </summary>
		/// <param name="filename">Полный путь к файлу лога.</param>
		/// <param name="rewrite">Если <see langword="true"/>, файл лога будет предварительно очищен.</param>
		/// <param name="length">
		/// Максимальное количество записей (порций) в буфере до автоматического сброса на диск.
		/// </param>
		public TinyLogWriterHelper(
			string filename,
			bool rewrite,
			int length = 300)
		{
			_init();
			Filename = filename;
			Length = length;
			if (rewrite)
				SuppIO.FileWrite(Filename, string.Empty);
		}


		/* properties */


		/// <summary>
		/// Возвращает или задает лимит количества порций записей в буфере до автосохранения.
		/// </summary>
		public int Length { get; set; }


		/// <summary>
		/// Возвращает полный путь к целевому файлу лога.
		/// </summary>
		public string Filename { get; private set; }


		/* methods */


		/// <summary>
		/// Добавляет текст в буфер лога.
		/// </summary>
		public void Append(
			string? text)
		{
			_sb.Append(text);
			_test();
		}


		/// <summary>
		/// Форматирует и добавляет текст в буфер лога без промежуточных строковых аллокаций.
		/// </summary>
		public void Append(
			string template,
			params object[] templateArgs)
		{
			_sb.AppendFormat(template, templateArgs);
			_test();
		}


		/// <summary>
		/// Добавляет строку с текстом и символом переноса строки в буфер лога.
		/// </summary>
		public void AppendLine(
			string? text)
		{
			_sb.AppendLine(text);
			_test();
		}


		/// <summary>
		/// Форматирует и добавляет строку с переносом строки в буфер лога.
		/// </summary>
		public void AppendLine(
			string template,
			params object[] templateArgs)
		{
			_sb
				.AppendFormat(template, templateArgs)
				.AppendLine();
			_test();
		}


		/// <summary>
		/// Форматирует текст, добавляет его в буфер лога и дублирует вывод в стандартную консоль.
		/// </summary>
		public void AppendLog(
			string template,
			params object[] templateArgs)
		{
			var s1 = string.Format(template, templateArgs);
			Append(s1);
			Console.Write(s1);
		}


		/// <summary>
		/// Форматирует текст, добавляет строку в буфер лога с переносом и дублирует вывод в консоль.
		/// </summary>
		public void AppendLineLog(
			string template,
			params object[] args)
		{
			var s1 = string.Format(template, args);
			AppendLine(s1);
			Console.WriteLine(s1);
		}


		/// <summary>
		/// Принудительно сбрасывает все накопленные в буфере логи на диск (в конец файла) и очищает буфер.
		/// </summary>
		public void Save()
		{
			if (_sb.Length == 0)
				return;
			SuppIO.FileWrite(Filename, _sb.ToString(), mode: FileMode.Append);
			_init();
		}


		/* privates */


		private void _init()
		{
			_sb = new StringBuilder();
			_count = 0;
		}


		private void _test()
		{
			_count++;
			if (_count > Length)
				Save();
		}

	}

}
