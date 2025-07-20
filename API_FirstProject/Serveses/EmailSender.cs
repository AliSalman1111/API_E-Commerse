
 using System.Net;
using System.Net.Mail;


namespace API_FirstProject.Serveses
{
   
    
        public class EmailSender : IEmailSender
        {
            public Task SendEmailAsync(string email, string subject, string message)
            {
                var client = new SmtpClient("smtp.gmail.com", 587)
                {
                    EnableSsl = true,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential("alisalman444111@gmail.com", "gyss lzjv hrit htxo") // ده لازم يكون App Password
                };

                var mailMessage = new MailMessage("alisalman444111@gmail.com", email, subject, message);
                mailMessage.IsBodyHtml = true; 

                return client.SendMailAsync(mailMessage);
            }
        }
    }



