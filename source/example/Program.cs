using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ImageTypers;

namespace example
{
    class Program
    {
        /// <summary>
        /// Test API
        /// </summary>
        static void test_api()
        {
            // change to your own username & password
            // -----------------------------------------
            string access_key = "access_token_here";

            // init imagetypersAPI obj with username and password
            ImageTypersAPI i = new ImageTypersAPI(access_key);

            // old school / legacy way
            // i.set_user_and_password("your_username", "your_password");

            // balance
            // ------------
            string balance = i.account_balance();
            Console.WriteLine(string.Format("Balance: {0}", balance));

            // captcha image
            // ==========================================================================================
            Console.WriteLine("Solving image captcha ...");
            string captcha_image_text = i.solve_captcha("captcha.jpg");
            Console.WriteLine(string.Format("Captcha text: {0}", captcha_image_text));

            // ==========================================================================================
            // recaptcha
            // ----------
            // submit
            // -------
            // check https://www.github.com/imagetyperz-api/imagetyperz-api-csharp for more details 
            // about how to get the page_url and sitekey

            // create params dict
            Dictionary<string, string> d = new Dictionary<string, string>();
            d.Add("page_url", "page_url_here");
            d.Add("sitekey", "sitekey_here");
            //d.Add("type", "3");                 // optional
            //d.Add("v3_min_score", "0.1");       // optional
            //d.Add("v3_action", "homepage");     // optional
            //d.Add("proxy", "126.45.34.53:123"); // or with auth 126.45.34.53:123:user:pass - optional
            //d.Add("user_agent", "Your user agent"); // optional

            string captcha_id = i.submit_recaptcha(d);
            Console.WriteLine("Waiting for recaptcha to be solved ...");

            // retrieve
            // ---------
            while (i.in_progress(captcha_id)) System.Threading.Thread.Sleep(10000);      // sleep for 10 seconds and retry
            string gresponse = i.retrieve_captcha(captcha_id);
            Console.WriteLine(string.Format("Recaptcha response: {0}", gresponse));

            // Geetest
            // ----------
            // create params dict
            //Dictionary<string, string> dg = new Dictionary<string, string>();
            //dg.Add("domain", "geetest captcha domain");
            //dg.Add("challenge", "geetest captcha challenge");
            //dg.Add("gt", "geetest captcha gt");
            ////dg.Add("proxy", "126.45.34.53:123"); // or with auth 126.45.34.53:123:user:pass - optional
            ////dg.Add("user_agent", "Your user agent"); // optional

            //string geetest_id = i.submit_geetest(dg);
            //Console.WriteLine(string.Format("Geetest captcha id: {0}", geetest_id));
            //Console.WriteLine("Waiting for geetest captcha to be solved ...");

            //// retrieve
            //// ---------
            //while (i.in_progress(geetest_id)) System.Threading.Thread.Sleep(10000);      // sleep for 10 seconds and retry

            //// we got a response at this point
            //// ---------------------------------
            //Dictionary<string, string> geetest_response = i.retrieve_geetest(geetest_id);     // get the response
            //Console.WriteLine(string.Format("Geetest response: {0} - {1} - {2}", geetest_response["challenge"], 
            //    geetest_response["validate"], geetest_response["seccode"]));

            // Other examples
            // ----------------
            // ImagetypersAPI i = new ImagetypersAPI(username, password, 123);     // init with refid
            // i.set_timeout(10);                                                  // set timeout to 10 seconds
            // Console.WriteLine(i.set_captcha_bad(captcha_id));                   // set captcha bad
            // i.submit_recaptcha(page_url, sitekey, "127.0.0.1:1234");    // solve recaptcha with proxy
            // i.submit_recaptcha(page_url, sitekey, "127.0.0.1:1234:user:pass");    // solve recaptcha with proxy - auth

            // Console.WriteLine(i.was_proxy_used(captcha_id));         // get status of proxy (if submitted with recaptcha)

            // Console.WriteLine(i.captcha_id());                       // last captcha solved id
            // Console.WriteLine(i.captcha_text());                     // last captcha solved text
            // Console.WriteLine(i.recaptcha_id());                     // last recaptcha solved id
            // Console.WriteLine(i.recaptcha_response());               // last recaptcha solved response
            // Console.WriteLine(i.error());                            // last error
        }


        /// <summary>
        /// Main method
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            try
            {
                Program.test_api();          // test API
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("Error occured: {0}", ex.Message));
            }
            finally
            {
                // disabled for command-line mode
                Console.WriteLine("FINISHED ! Press ENTER to close window ...");
                Console.ReadLine();
            }
        }
    }
}
