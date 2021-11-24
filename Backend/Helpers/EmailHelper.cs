using System;
using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;

namespace Backend.Helpers
{
    public class EmailHelper
    {
        private readonly IConfiguration _configuration;

        SmtpClient smtpClient;
        public EmailHelper(IConfiguration configuration)
        {
            _configuration = configuration;
             smtpClient = new SmtpClient(_configuration["Smtp:Host"])
            {
                Port = int.Parse(this._configuration["Smtp:Port"]),
                Credentials = new NetworkCredential(this._configuration["Smtp:Username"], this._configuration["Smtp:Password"]),
                EnableSsl = true,
            };
        }

        public void send() {

            var mailMessage = new MailMessage
            {
                From = new MailAddress(_configuration["Smtp:Username"]),
                Subject = "subject",
                Body = "<h1>Hello Paula</h1>",
                IsBodyHtml = true,
            };
            //text Use number and carrier:
            /*
             * Carrier destinations

                ATT: Compose a new email and use the recipient's 10-digit wireless phone number, followed by @txt.att.net. For example, 5551234567@txt.att.net.
                Verizon: Similarly, ##@vtext.com
                Sprint: ##@messaging.sprintpcs.com
                TMobile: ##@tmomail.net
                Virgin Mobile: ##@vmobl.com
                Nextel: ##@messaging.nextel.com
                Boost: ##@myboostmobile.com
                Alltel: ##@message.alltel.com
                EE: ##@mms.ee.co.uk (might support send without reply-to)
             * 
             */
            mailMessage.To.Add("8168599585@tmomail.net");

            //regular email
            mailMessage.To.Add("paulis_0822@hotmail.com");

            smtpClient.Send(mailMessage);
        }

    }
}
