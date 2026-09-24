// rev 2026-09-19

using MailKit.Net.Smtp;
using MimeKit;
using MimeKit.Text;
using System.Text;

namespace Ans.Net10.Common.Services
{

	/// <summary>
	/// Интерфейс службы отправки электронных писем.
	/// </summary>
	public interface IMailerService
	{
		/// <summary>
		/// Асинхронно отправляет электронное письмо на основе переданной модели.
		/// </summary>
		/// <param name="message">Модель данных отправляемого сообщения.</param>
		Task SendAsync(MailMessageModel message);
	}



	/// <summary>
	/// Интерфейс параметров конфигурации SMTP-клиента для службы отправки писем.
	/// </summary>
	public interface IMailerServiceOptions
	{
		string SmtpServer { get; }
		int SmtpPort { get; }
		bool SmtpUseSsl { get; }
		string SmtpUsername { get; }
		string SmtpPassword { get; }
		string DefaultFromAddress { get; }
		string DefaultFromTitle { get; }
		string DebugCc { get; }
	}



	/// <summary>
	/// Заглушка службы отправки писем для использования в средах разработки или тестирования.
	/// Не выполняет реальную отправку.
	/// </summary>
	public class FakeMailerService
		: IMailerService
	{
		/// <inheritdoc />
		public Task SendAsync(
			MailMessageModel message)
		{
			return Task.CompletedTask;
		}
	}



	/// <summary>
	/// Реализация службы отправки писем через SMTP-протокол с использованием библиотеки MailKit.
	/// </summary>
	/// <param name="options">Параметры конфигурации подключения к SMTP-серверу.</param>
	public class AnsMailerService(
		IMailerServiceOptions options)
		: IMailerService
	{

		private readonly IMailerServiceOptions _options = options;


		/* functions */


		/// <summary>
		/// Создает объект адреса <see cref="MailboxAddress"/> с поддержкой UTF-8.
		/// </summary>
		/// <param name="title">Отображаемое имя адресата.</param>
		/// <param name="address">Электронный почтовый адрес.</param>
		public static MailboxAddress GetMailboxAddress(
			string title,
			string address)
		{
			return new MailboxAddress(Encoding.UTF8, title, address);
		}


		/// <summary>
		/// Создает объект адреса <see cref="MailboxAddress"/>, где имя совпадает с электронным адресом.
		/// </summary>
		/// <param name="address">Электронный почтовый адрес.</param>
		public static MailboxAddress GetMailboxAddress(
			string address)
		{
			return GetMailboxAddress(address, address);
		}


		/// <inheritdoc />
		public async Task SendAsync(
			MailMessageModel message)
		{
			var mimeMessage1 = new MimeMessage();
			mimeMessage1.From.Add(message.From ?? GetMailboxAddress(
				_options.DefaultFromTitle, _options.DefaultFromAddress));
			if (message.To != null)
				mimeMessage1.To.Add(message.To);
			if (message.Cc.Length > 0)
				mimeMessage1.Cc.AddRange(message.Cc);
			if (message.Bcc.Length > 0)
				mimeMessage1.Bcc.AddRange(message.Bcc);
			mimeMessage1.Subject = message.Subject;
			mimeMessage1.Body = new TextPart(TextFormat.Html)
			{
				Text = message.ContentHtml
			};
			using var client1 = new SmtpClient();
			client1.AuthenticationMechanisms.Remove("XOAUTH2");
			try
			{
				await client1.ConnectAsync(
					_options.SmtpServer,
					_options.SmtpPort,
					_options.SmtpUseSsl);
				await client1.AuthenticateAsync(
					_options.SmtpUsername,
					_options.SmtpPassword);
				await client1.SendAsync(mimeMessage1);
			}
			finally
			{
				if (client1.IsConnected)
					await client1.DisconnectAsync(true);
			}
		}

	}

}
