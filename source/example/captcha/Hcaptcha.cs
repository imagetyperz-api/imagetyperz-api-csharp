using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ImageTypers;

namespace example
{
    class TestHcaptcha
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
            d.Add("sitekey", "8c7062c7-cae6-4e12-96fb-303fbec7fe4f");
            //d.Add("invisible", "1");              // if captcha is invisible - optional

            // domain used in loading of hcaptcha interface, default: hcaptcha.com - optional
            // d.Add("domain", "hcaptcha.com");

            // extra parameters, useful for enterprise
            // submit userAgent from requests too, when this is used
            // d.Add("HcaptchaEnterprise", "{\"rqdata\": \"value taken from web requests\"}");

            //d.Add("proxy", "126.45.34.53:123"); // or with auth 126.45.34.53:123:user:pass - optional
            //d.Add("user_agent", "Your user agent"); // optional

            string captcha_id = i.submit_hcaptcha(d);
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
