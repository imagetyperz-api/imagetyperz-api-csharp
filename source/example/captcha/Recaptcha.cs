using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ImageTypers;

namespace example
{
    class TestRecaptcha
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
            d.Add("page_url", "https://your-site.com");
            d.Add("sitekey", "7LrGJmcUABBAALFtIb_FxC0LXm_GwOLyJAfbbUCL");
            // reCAPTCHA type(s) - optional, defaults to 1
            // ---------------------------------------------
            // 1 - v2
            // 2 - invisible
            // 3 - v3
            // 4 - enterprise v2
            // 5 - enterprise v3
            //
            // d.Add("type", "1");                 // optional
            //
            //d.Add("domain", "www.google.com");  // used in loading reCAPTCHA interface, default: www.google.com (alternative: recaptcha.net) - optional
            //d.Add("v3_min_score", "0.1");       // optional
            //d.Add("v3_action", "homepage");     // optional
            //d.Add("proxy", "126.45.34.53:123"); // or with auth 126.45.34.53:123:user:pass - optional
            //d.Add("user_agent", "Your user agent"); // optional
            //d.Add("data-s", "recaptcha data-s value"); // optional
            //d.Add("cookie_input", "a=b;c=d"); // optional

            string captcha_id = i.submit_recaptcha(d);
            Console.WriteLine("Waiting for captcha to be solved...");
            Dictionary<string, string> response = null;
            while (response == null)
            {
                System.Threading.Thread.Sleep(10000);       // sleep for 10 secons before checking for response
                response = i.retrieve_response(captcha_id);
            }
            ImageTypers.Utils.print_response(response);

            // other examples
            // ImagetypersAPI i = new ImagetypersAPI(username, password, 123);     // init with affiliate id
            // i.set_timeout(10);                                                  // set timeout to 10 seconds
            // Console.WriteLine(i.set_captcha_bad(captcha_id));                   // if response is incorrect, set captcha as bad using ID
        }
    }
}