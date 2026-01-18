using MailKit.Net.Smtp;
using MimeKit;
using MimeKit.Text;
using System.Text;

namespace Ans.Net10.Common.Services
{

	/*
	 *	builder.Services.AddSingleton<IMailerService, AnsMailerService>(
	 *		x => new(options1.MailService));
	 */


	public interface IMailerService
	{
		Task SendAsync(MailMessageModel message);
	}



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



	public class FakeMailerService
		: IMailerService
	{
		Task IMailerService.SendAsync(MailMessageModel message) { return null; }
	}



	public class AnsMailerService(
		IMailerServiceOptions options)
		: IMailerService
	{

		private readonly IMailerServiceOptions _options = options;


		/* functions */


		public static MailboxAddress GetMailboxAddress(
			string title,
			string address)
		{
			return new MailboxAddress(
				Encoding.UTF8, title, address);
		}


		public static MailboxAddress GetMailboxAddress(
			string address)
		{
			return GetMailboxAddress(
				address, address);
		}


		public async Task SendAsync(
			MailMessageModel message)
		{
			var message1 = new MimeMessage();
			message1.From.Add(message.From ?? GetMailboxAddress(
				_options.DefaultFromTitle, _options.DefaultFromAddress));
			message1.To.Add(message.To);
			if (message.Cc?.Length > 0)
				message1.Cc.AddRange(message.Cc);
			if (message.Bcc?.Length > 0)
				message1.Bcc.AddRange(message.Bcc);
			message1.Subject = message.Subject;
			message1.Body = new TextPart(TextFormat.Html)
			{
				Text = message.ContentHtml
			};
			using var client1 = new SmtpClient();
			client1.AuthenticationMechanisms.Remove("XOAUTH2");
			await client1.ConnectAsync(
				_options.SmtpServer,
				_options.SmtpPort,
				_options.SmtpUseSsl);
			await client1.AuthenticateAsync(
				_options.SmtpUsername,
				_options.SmtpPassword);
			await client1.SendAsync(message1);
			await client1.DisconnectAsync(true);
		}

	}

}
