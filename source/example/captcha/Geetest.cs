using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ImageTypers;

namespace example
{
    class TestGeetest
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
            d.Add("domain", "https://your-site.com");
            d.Add("challenge", "eea8d7d1bd1a933d72a9eda8af6d15d3");
            d.Add("gt", "1a761081b1114c388092c8e2fd7f58bc");
            //d.Add("api_server", "api.geetest.com"); // optional
            //d.Add("proxy", "126.45.34.53:123"); // or with auth 126.45.34.53:123:user:pass - optional
            //d.Add("user_agent", "Your user agent"); // optional

            string captcha_id = i.submit_geetest(d);
            Console.WriteLine("Waiting for captcha to be solved...");
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
