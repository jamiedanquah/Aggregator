using System;
using System.Net.Mail;
using System.Text;
using Aggregator.Objects.Configuration;

namespace Aggregator.Business.Helpers
{
    public class ErrorHelper
    {
        public static bool SendErrorEmail(Exception exception, string errorMessage, Settings settings)
        {
            var sb = new StringBuilder();
            sb.AppendLine("<h2>Error on aggregator</h2>");

            sb.Append("<p>Server: ");
            sb.Append(Environment.MachineName);
            sb.Append("</p>");

            sb.Append("<p>Date: ");
            sb.Append(DateTime.Now);
            sb.Append("</p>");

            sb.Append("<p>Error Message: ");
            sb.Append(errorMessage);
            sb.Append("</p>");

            sb.Append("<p>Exception: ");
            sb.Append(exception.Message);
            sb.Append("</p>");

            sb.Append("<p>Exception: ");
            sb.Append(exception.StackTrace);
            sb.Append("</p>");

            try
            {
                var mail = new MailMessage
                {
                    Subject = "Aggregator Error " + DateTime.Now,
                    From = new MailAddress(settings.MailSettings.ErrorMailFrom),
                    IsBodyHtml = true,
                    Body = sb.ToString()
                };
                mail.To.Add(settings.MailSettings.ErrorMailTo);

                var smtpClient = new SmtpClient(settings.MailSettings.SmtpServer);
                smtpClient.Send(mail);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}