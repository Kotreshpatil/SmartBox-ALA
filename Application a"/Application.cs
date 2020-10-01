using System;
using System.Collections.Generic;
using Libraries;
using ProgrammingParadigms;
using DomainAbstractions;
using System.Device.Gpio;
using System.Data.SqlTypes;
using System.Data.SqlClient;

namespace Application
{
    public class Application
    {
        public EventConnector appStartConnector = new EventConnector() { InstanceName = "appStartConnector" };

        public void InitialiseLDR()
        {
            double value = 0; //used to store LDR value
            int ldr = 17;
            int led = 11;
            GpioController gpioController = new GpioController();
            gpioController.OpenPin(led, PinMode.Output);

            static int rcTime(int ldr)
            {
                int count = 0;
                GpioController gpioController = new GpioController();
                gpioController.SetPinMode(ldr, PinMode.Output);
                gpioController.Write(ldr, false);
                System.Threading.Thread.Sleep(2);
                gpioController.SetPinMode(ldr, PinMode.Input);
                while (gpioController.Read(ldr) == 0)
                {
                    count++;
                }

                return count;
            }
            while (true)
            {
                Console.WriteLine("The LDR Values are");
                value = rcTime(ldr);
                Console.WriteLine(value);
                if (value >= 100000)
                {
                    UpdateDb(' ', 0);
                }
            }
        }

        private void UpdateDb(char userType, int checkFlag)
        {
            SqlConnection conn = new SqlConnection("Server=.\\SQLEXPRESS;Database=flagdata;Integrated Security=True");
            conn.Open();
            string cmd1 = "update [dbo].[flags] set user_type = '" + userType;
            //string cmd2 = "', skip_flag = " + skip_flag;
            string cmd3 = ", chk_flg = " + checkFlag;
            string cmd4 = " where row_id = 1 ";
            string Sqlcmd = cmd1 + cmd3 + cmd4;
            //Console.WriteLine(Sqlcmd);
            SqlCommand cmd = new SqlCommand(Sqlcmd, conn);
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        static void Main(string[] args)
        {
            var app = new Application(); // Do the wiring

            app.InitialiseLDR(); // Set up the light-dependent resistors

            (app.appStartConnector as IEvent).Execute(); // Start the app
        }

        public Application()
        {
            // BEGIN AUTO-GENERATED INSTANTIATIONS
            DataFlowConnector<int> currentCount = new DataFlowConnector<int>() { InstanceName = "currentCount" };
            ConvertToEvent<int> id_e43183ad69b4406f8641bd88f44c5fd6 = new ConvertToEvent<int>() {  };
            FlagQuery id_936f506a36c2446d942932db25dc4f5e = new FlagQuery() { Query = "select chk_flg from dbo.flags where row_id = 1" };
            Data<int> id_3605c8eea127447ba56d6529f2ea591f = new Data<int>() {  };
            Apply<int, string> id_0de39292db4942d590e2d18ef2c67b77 = new Apply<int, string>() { Lambda = count => {	if (count == 0) return "P";	if (count == 1) return "R";	if (count == 2) return "L";	if (count == 3) return "S";		return "";} };
            Apply<string, string> id_55b486b0742b41ccba7b7ba84d177c24 = new Apply<string, string>() { Lambda = s => {	return $"update [dbo].[flags] set user_type = {s}, chk_flag = 0";} };
            UpdateQuery id_f50f338350b84df18f5e9487fe02fb84 = new UpdateQuery() { Config = "Server=.\\SQLEXPRESS;Database=flagdata;Integrated Security=True" };
            Apply<int, bool> id_baa88fa66d4a4119b75dcab78415f6d9 = new Apply<int, bool>() { Lambda = count => {	return count == 3;} };
            IfElse id_fcafe1047a6f406eb396a2b3babd6c04 = new IfElse() {  };
            SendSMS id_a6f63156309a422489934d2be8f92d1e = new SendSMS() { Message = "Patient skipped taking medication", AccountSid = "{{ AC0f7585feaf972f1fc2c04ddd7f8596e0 }}", AuthToken = "{{ e8416e31ae6fc614d2e119aec701903a }}", FromNumber = "+2562425369", ToNumber = "+640225620291" };
            EventLambda id_8540f2746d5d4871a11f09e3057e3000 = new EventLambda() { Lambda = () => {	Console.WriteLine("It's time to take your medication");} };
            EventConnector id_c06622dee4374c79b9049ea136c8d83f = new EventConnector() {  };
            EventLambda id_293a2e18b79a4f40aaf082917057c8e3 = new EventLambda() { Lambda = () => {	System.Threading.Thread.Sleep(TimeSpan.FromMinutes(10).Milliseconds);} };
            Data<List<int>> id_32910ec9bba24fbfad9c94ca4484dc8c = new Data<List<int>>() { storedData = new List<int>() { 0, 1, 2, 3 } };
            ForEach<int> id_0c804088f26a4adcb79555dd352ccdfe = new ForEach<int>() {  };
            // END AUTO-GENERATED INSTANTIATIONS

            // BEGIN AUTO-GENERATED WIRING
            appStartConnector.WireTo(id_32910ec9bba24fbfad9c94ca4484dc8c, "fanoutList");
            currentCount.WireTo(id_e43183ad69b4406f8641bd88f44c5fd6, "fanoutList");
            currentCount.WireTo(id_baa88fa66d4a4119b75dcab78415f6d9, "fanoutList");
            id_e43183ad69b4406f8641bd88f44c5fd6.WireTo(id_c06622dee4374c79b9049ea136c8d83f, "eventOutput");
            id_936f506a36c2446d942932db25dc4f5e.WireTo(id_3605c8eea127447ba56d6529f2ea591f, "high");
            id_936f506a36c2446d942932db25dc4f5e.WireTo(id_8540f2746d5d4871a11f09e3057e3000, "low");
            id_3605c8eea127447ba56d6529f2ea591f.WireTo(currentCount, "inputDataB");
            id_3605c8eea127447ba56d6529f2ea591f.WireTo(id_0de39292db4942d590e2d18ef2c67b77, "dataOutput");
            id_0de39292db4942d590e2d18ef2c67b77.WireTo(id_55b486b0742b41ccba7b7ba84d177c24, "output");
            id_55b486b0742b41ccba7b7ba84d177c24.WireTo(id_f50f338350b84df18f5e9487fe02fb84, "output");
            id_baa88fa66d4a4119b75dcab78415f6d9.WireTo(id_fcafe1047a6f406eb396a2b3babd6c04, "output");
            id_fcafe1047a6f406eb396a2b3babd6c04.WireTo(id_a6f63156309a422489934d2be8f92d1e, "ifOutput");
            id_c06622dee4374c79b9049ea136c8d83f.WireTo(id_936f506a36c2446d942932db25dc4f5e, "fanoutList");
            id_c06622dee4374c79b9049ea136c8d83f.WireTo(id_293a2e18b79a4f40aaf082917057c8e3, "complete");
            id_32910ec9bba24fbfad9c94ca4484dc8c.WireTo(id_0c804088f26a4adcb79555dd352ccdfe, "dataOutput");
            id_0c804088f26a4adcb79555dd352ccdfe.WireTo(currentCount, "elementOutput");
            // END AUTO-GENERATED WIRING
        }
    }
}






