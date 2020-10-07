using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ImageTypers;

namespace example
{
    class TestTiktok
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
            d.Add("page_url", "https://tiktok.com");
            // make sure `s_v_web_id` cookie is present
            d.Add("cookie_input", "s_v_web_id:verify_kd6243o_fd449FX_FDGG_1x8E_8NiQ_fgrg9FEIJ3f;tt_webid:612465623570154;tt_webid_v2:7679206562717014313;SLARDAR_WEB_ID:d0314f-ce16-5e16-a066-71f19df1545f;");
            //d.Add("proxy", "126.45.34.53:123"); // or with auth 126.45.34.53:123:user:pass - optional
            //d.Add("user_agent", "Your user agent"); // optional

            string captcha_id = i.submit_tiktok(d);
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