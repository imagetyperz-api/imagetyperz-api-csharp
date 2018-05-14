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
            string access_key = "your_access_key";

            // init imagetypersAPI obj with username and password
            ImageTypersAPI i = new ImageTypersAPI(access_key);

            // old school / legacy way
            i.set_user_and_password("your_username", "your_password");

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
            // check http://www.imagetyperz.com/Forms/recaptchaapi.aspx for more details 
            // about how to get the page_url and sitekey
            string page_url = "your_page_url_here";
            string sitekey = "your_site_key_here";

            string captcha_id = i.submit_recaptcha(page_url, sitekey);
            Console.WriteLine("Waiting for recaptcha to be solved ...");

            // retrieve
            // ---------
            while (i.in_progress(captcha_id))       // check if it's still being decoded
            {
                System.Threading.Thread.Sleep(10000);      // sleep for 10 seconds
            }

            // we got a response at this point
            // ---------------------------------
            string recaptcha_response = i.retrieve_captcha(captcha_id);     // get the response
            Console.WriteLine(string.Format("Recaptcha response: {0}", recaptcha_response));

            // Other examples
            // ----------------
            // ImagetypersAPI i = new ImagetypersAPI(username, password, 123);     // init with refid
            // i.set_timeout(10);                                                  // set timeout to 10 seconds
            // Console.WriteLine(i.set_captcha_bad(captcha_id));                   // set captcha bad
            // i.submit_recaptcha(page_url, sitekey, "127.0.0.1:1234");    // solve recaptcha with proxy
            // i.submit_recaptcha(page_url, sitekey, "127.0.0.1:1234:user:pass");    // solve recaptcha with proxy - auth

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
