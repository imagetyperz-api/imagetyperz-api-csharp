using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ImageTypers;

namespace example
{
    class TestTask
    {
        public static void run()
        {
            // get access token from: http://www.imagetyperz.com
            string token = Config.TOKEN;
            ImageTypersAPI i = new ImageTypersAPI(token);

            // balance
            string balance = i.account_balance();
            Console.WriteLine(string.Format("Balance: {0}", balance));

            Dictionary<string, string> d = new Dictionary<string, string>();
            d.Add("template_name", "Login test page");
            d.Add("page_url", "https://imagetyperz.net/automation/login");
            d.Add("variables", "{\"username\": \"abc\", \"password\": \"paZZW0rd\"}");
            //d.Add("proxy", "126.45.34.53:123"); // or with auth 126.45.34.53:123:user:pass - optional
            //d.Add("user_agent", "Your user agent"); // optional

            string captcha_id = i.submit_task(d);
            Console.WriteLine("Waiting for captcha to be solved...");

            // send pushVariable - update of variable while task is running (e.g 2FA code)
            // string code = "24323";
            // i.task_push_variables(captcha_id, "{\"twofactor_code\": \"" + code + "\"}");

            Dictionary<string, string> response = null;
            while (response == null)
            {
                System.Threading.Thread.Sleep(10000);       // sleep for 10 secons before checking for response
                response = i.retrieve_response(captcha_id);
            }
            ImageTypers.Utils.print_response(response);
        }
    }
}