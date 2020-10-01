using System;
using System.Collections.Generic;
using System.Text;
using Libraries;
using ProgrammingParadigms;
using Twilio;
using Twilio.Clients;
using Twilio.Rest.Api.V2010.Account;

namespace DomainAbstractions
{
    public class SendSMS : IEvent
    {
        // Public properties
        public string InstanceName { get; set; } = "Default";
        public string Message { get; set; } = "";

        // Find your Account Sid and Auth Token at twilio.com/user/account
        public string AccountSid { get; set; } = "";
        public string AuthToken { get; set; } = "";
        public string FromNumber { get; set; } = "";
        public string ToNumber { get; set; } = "";

        // IEvent implementation
        void IEvent.Execute()
        {
            SendMessage(Message);
        }

        // Methods
        private void SendMessage(string message)
        {
            // Find your Account Sid and Auth Token at twilio.com/user/account
            string AccountSid = "{{ AC0f7585feaf972f1fc2c04ddd7f8596e0 }}";
            string AuthToken = "{{ e8416e31ae6fc614d2e119aec701903a }}";

            TwilioClient.Init(AccountSid, AuthToken);

            //var message = MessageResource.Create(
            //body: "Patient skipped to take medication.",
            //from: new Twilio.Types.PhoneNumber("+2562425369"),
            //to: new Twilio.Types.PhoneNumber("+640225620291"));
            //Console.WriteLine(message.Sid);
            Console.WriteLine("Patient Skipped to take medication");
        }

        public SendSMS()
        {

        }
    }
}