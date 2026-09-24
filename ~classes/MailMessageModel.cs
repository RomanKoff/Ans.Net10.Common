// rev 2026-09-19

using MimeKit;

namespace Ans.Net10.Common
{

	/// <summary>
	/// Модель данных, представляющая структуру электронного письма
	/// для последующей отправки через MimeKit/MailKit.
	/// </summary>
	public class MailMessageModel
	{

		/// <summary>
		/// Возвращает или задает адрес и имя отправителя письма.
		/// </summary>
		public MailboxAddress? From { get; set; }


		/// <summary>
		/// Возвращает или задает адрес и имя главного получателя письма.
		/// </summary>
		public MailboxAddress? To { get; set; }


		/// <summary>
		/// Возвращает или задает массив адресов получателей копии письма (Cc).
		/// Гарантированно не равен <see langword="null"/>.
		/// </summary>
		public MailboxAddress[] Cc { get; set; } = [];


		/// <summary>
		/// Возвращает или задает массив адресов получателей скрытой копии письма (Bcc).
		/// Гарантированно не равен <see langword="null"/>.
		/// </summary>
		public MailboxAddress[] Bcc { get; set; } = [];


		/// <summary>
		/// Возвращает или задает тему электронного письма.
		/// </summary>
		public string Subject { get; set; } = string.Empty;


		/// <summary>
		/// Возвращает или задает тело письма в формате HTML.
		/// </summary>
		public string ContentHtml { get; set; } = string.Empty;

	}

}
