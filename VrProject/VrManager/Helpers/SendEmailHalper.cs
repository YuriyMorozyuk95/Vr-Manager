//using System;
//using System.Collections.Generic;
//using System.Linq;
using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace VrManager.Helpers
{
    public static class SendEmailHalper
    {


        public static void SendEmailTo(string to, string fromAddres = "occulustest@gmail.com", string fromPassword = "12345678q",
            string fromName = "Occulus", string subject = "Subject", string body = "", string host = "smtp.gmail.com", int port = 587)
        {
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient();
                SmtpServer.Host = host;
                SmtpServer.Port = 587;
                SmtpServer.Credentials = new NetworkCredential(fromAddres, fromPassword);

                MailAddress mailFrom = new MailAddress(fromAddres, fromName);
                MailAddress mailTo = new MailAddress(to);

                mail.From = mailFrom;
                mail.To.Add(mailTo);

                mail.Subject = subject;
                mail.Body = body;

                string pathToTempFile = App.Setting.PathToFolderFiles + $"\\ReportStatistic-{DateTime.Now.Date.GetDateTimeFormats()[0]}.xlsx";
                var stitisticItems = StisticFormatHelper.StatisticItemFactory(App.Repository.Observes);
                CreateExcelFileHelper.CreateExcelDocument(stitisticItems, pathToTempFile);
                System.Net.Mail.Attachment attachment = new System.Net.Mail.Attachment(pathToTempFile);
               
                
                mail.Attachments.Add(attachment);
                SmtpServer.EnableSsl = true;
                SmtpServer.Send(mail);

                attachment.Dispose();
                mail.Dispose();
                
                File.Delete(pathToTempFile);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
