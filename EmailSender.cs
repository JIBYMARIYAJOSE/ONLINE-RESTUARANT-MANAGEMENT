using Mailjet.Client;
using Mailjet.Client.Resources;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace firstProjectWith_ASP.Utility
{
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _configuration;

        public MailJetSettings _mailJetSettings { get; set; }

        public EmailSender(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            return Execute(email, subject, htmlMessage);
        }

        public async Task Execute(string email, string subject, string body)
        {
            _mailJetSettings = _configuration.GetSection("MailJet").Get<MailJetSettings>();



             MailjetClient client = new MailjetClient("dbd0128813ee9e8b0a674e3217edca92","63760449f573b0df91cee4aaefa9b6c2")//add keys from mailjet-->account setting->rest API key-> copy (api key,secret key)
             {
            // Version = ApiVersion.V3_1,
             };
           // MailjetClient client = new MailjetClient(_mailJetSettings.ApiKey, _mailJetSettings.SecretKey)
           // {
                //Version = ApiVersion.V3_1,
           // };
            MailjetRequest request = new MailjetRequest
            {
                Resource = Send.Resource,
            }
             .Property(Send.Messages, new JArray {
     new JObject {
      {
       "From",
       new JObject {
        {"Email", "jiby.jose1404@gmail.com"},
        {"Name", "jiby"}
       }
      }, {
       "To",
       new JArray {
        new JObject {
         {
          "Email",
          "jiby.jose1404@gmail.com"
         }, {
          "Name",
          "jiby"
         }
        }
       }
      }, {
       "Subject",
       "Greetings from Mailjet."
      }, {
       "TextPart",
       "My first Mailjet email"
      }, {
       "HTMLPart",
       "<h3>Dear passenger 1, welcome to <a href='https://www.mailjet.com/'>Mailjet</a>!</h3><br />May the delivery force be with you!"
      }, {
       "CustomID",
       "AppGettingStartedTest"
      }
     }
             });
            MailjetResponse response = await client.PostAsync(request);
        }
    }
}
