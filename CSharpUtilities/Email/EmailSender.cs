using MailKit.Net.Smtp;
using MimeKit;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;

namespace CSharpUtilities.Email
{
    public class EmailSender : IEmailSender
    {
        private readonly EmailConfiguration _emailConfig;

        public EmailSender(EmailConfiguration emailConfig)
        {
            _emailConfig = emailConfig;
        }

        #region PrivateMethods
        /// <summary>
        /// Use to convert a Meesage object to a native MimeMessage object
        /// </summary>
        /// <param name="message" type="Message">defines the message object that will converted to a MimeMessage</param>
        /// <returns>MimeMessage - the converted MimeMessage to be sent via email</returns>
        private MimeMessage CreateEmailMessage(Message message)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress(_emailConfig.From));
            emailMessage.To.AddRange(message.To);
            emailMessage.Subject = message.Subject;

            var bodyBuilder = new BodyBuilder { HtmlBody = message.Content };

            emailMessage.Body = bodyBuilder.ToMessageBody();

            return emailMessage;

        }

        /// <summary>
        /// Use to send an email via an smtp server
        /// </summary>
        /// <param name="emailMessage" type="MimeMessage">defines the MimeMessage to be sent via email</param>
        private void Send(MimeMessage emailMessage)
        {
            using (var client = new SmtpClient())
            {
                try
                {
                    client.Connect(_emailConfig.SmtpServer, _emailConfig.Port, true);
                    client.AuthenticationMechanisms.Remove("XOAUTH2");
                    client.Authenticate(_emailConfig.Username, _emailConfig.Password);

                    client.Send(emailMessage);
                }
                catch
                {
                    throw;
                }
                finally
                {
                    client.Disconnect(true);
                    client.Dispose();
                }
            }
        }
        #endregion

        /// <summary>
        /// Use to send an email
        /// </summary>
        /// <param name="message" type="Message">defines the message object that will be sent via email</param>
        public void SendEmail(Message message)
        {
            Send(CreateEmailMessage(message));
        }
    }
}
