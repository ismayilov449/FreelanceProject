using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace Api.Infrastructure.Helpers
{
    public interface ISmsService
    {
        Task SendMailAsync(string toNumber, string body);
    }

    public class SmsService : ISmsService
    {
        private IConfiguration _configuration;

        public SmsService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendMailAsync(string toNumber, string body)
        {
            string sid = _configuration.GetSection("Sms:sid").Value;
            string token = _configuration.GetSection("Sms:token").Value;
            string from = _configuration.GetSection("Sms:number").Value;

            TwilioClient.Init(sid, token);

            var message = await MessageResource.CreateAsync(
                body: body,
                from: new PhoneNumber(from),
                to: new PhoneNumber(toNumber)
            );

        }
    }
}
