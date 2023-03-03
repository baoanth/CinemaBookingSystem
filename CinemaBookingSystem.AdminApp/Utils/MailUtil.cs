using System.Net.Mail;

namespace CinemaBookingSystem.AdminApp.Utils
{
    public class MailUtil
    {
        private static readonly string _from = "cinemabookingsystem19@gmail.com"; // Email của Sender (của bạn)
        private static readonly string _pass = "kpygpflfqoygbxcl"; // Mật khẩu Email của Sender (của bạn)

        public static string Send(string sendto, string content)
        {
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com", 587);

                mail.From = new MailAddress(_from);
                mail.To.Add(sendto);
                mail.Subject = "Phản hồi từ Cinemax";
                mail.IsBodyHtml = true;
                mail.Body = content;

                mail.Priority = MailPriority.High;

                SmtpServer.Port = 587;
                SmtpServer.UseDefaultCredentials = false;
                SmtpServer.Credentials = new System.Net.NetworkCredential(_from, _pass);
                SmtpServer.EnableSsl = true;

                SmtpServer.Send(mail);
                return "OK";
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }

        }
    }
}
