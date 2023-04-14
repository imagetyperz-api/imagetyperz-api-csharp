using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ImageTypers;

namespace example
{
    class TestTurnstile
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
            d.Add("sitekey", "0x4ABBBBAABrfvW5vKbx11FZ");
            //d.Add("domain", "challenges.cloudflare.com"); // domain used in loading turnstile interface, default: challenges.cloudflare.com - optional
            //d.Add("action", "homepage");                  // used in loading turnstile interface, similar to reCAPTCHA - optional
            //d.Add("cdata", "your cdata");                 // used in loading turnstile interface - optional
            //d.Add("proxy", "126.45.34.53:123"); // or with auth 126.45.34.53:123:user:pass - optional
            //d.Add("user_agent", "Your user agent"); // optional

            string captcha_id = i.submit_turnstile(d);
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