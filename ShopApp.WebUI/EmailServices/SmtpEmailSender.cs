using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace ShopApp.WebUI.EmailServices
{
    public class SmtpEmailSender : IEmailSender
    {
        private string _host;
        private int _port;
        private bool _enableSSL;
        private string _userName;
        private string _password;
        public SmtpEmailSender(string host, int port, bool enableSSL, string userName, string password)
        {
            this._host = host;
            this._port = port;
            this._enableSSL = enableSSL;
            this._userName = userName;
            this._password = password;
        }
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var client = new SmtpClient(this._host, this._port)
            {
                Credentials = new NetworkCredential(this._userName, this._password),
                EnableSsl = true
            };

            return client.SendMailAsync(

                new MailMessage(this._userName, email, subject, htmlMessage)
                {
                    IsBodyHtml = true
                }
        );

        }

    }
}
