using CorreoFei.Services.ErrorLog;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;

namespace CorreoFei.Services.Email
{
    public class Email : IEmail
    {
        private readonly IErrorLog _errorLog;
        private readonly IConfiguration _configuration;

        public Email(IErrorLog errorLog, IConfiguration configuration)
        {
            _errorLog = errorLog;
            _configuration = configuration;
        }

        [HttpPost]
        public Task<bool> EnviaCorreoAsync(string tema, string para, string cc, string bcc, string cuerpo, Attachment adjunto = null)
        {
            bool res = false;
            try
            {
                MailMessage eMail = new();
                if (para != null)
                    eMail.To.Add(para);
                if (cc != null)
                    eMail.CC.Add(cc);
                if (bcc != null)
                    eMail.Bcc.Add(bcc);

                eMail.From = new MailAddress(_configuration["Smtp:SmtpUser"]);
                if (string.IsNullOrEmpty(tema))
                    tema = "[sin asunto]";
                eMail.Subject = tema;
                eMail.Body = cuerpo;
                if (adjunto != null)
                    eMail.Attachments.Add(adjunto);
                eMail.BodyEncoding = System.Text.Encoding.UTF8;
                eMail.SubjectEncoding = System.Text.Encoding.UTF8;
                eMail.HeadersEncoding = System.Text.Encoding.UTF8;
                eMail.IsBodyHtml = true;

                SmtpClient clienteSMTP = new(_configuration["Smtp:SmtpServer"]);
                clienteSMTP.Port = Convert.ToInt16(_configuration["Smtp:SmtpPort"]);
                clienteSMTP.EnableSsl = true;
                clienteSMTP.DeliveryMethod = SmtpDeliveryMethod.Network;
                clienteSMTP.UseDefaultCredentials = false;
                clienteSMTP.Credentials = new System.Net.NetworkCredential(_configuration["Smtp:SmtpUser"], _configuration["Smtp:SmtpPwd"]);

                _errorLog.ErrorLogAsync($"Enviando correo a: {para}");
                clienteSMTP.SendAsync(eMail, null);
                res = true;

            }
            catch (Exception ex)
            {
                _errorLog.ErrorLogAsync(ex.Message);
            }
            return Task.FromResult(res);
        }
    }
}
