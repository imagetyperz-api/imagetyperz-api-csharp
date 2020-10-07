using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ImageTypers;

namespace example
{
    class TestImage
    {
        public static void run()
        {
            // get access token from: http://www.imagetyperz.com
            string token = Config.TOKEN;
            ImageTypersAPI i = new ImageTypersAPI(token);

            // balance
            string balance = i.account_balance();
            Console.WriteLine(string.Format("Balance: {0}", balance));

            // optional parameters dict
            Dictionary<string, string> image_params = new Dictionary<string, string>();
            //image_params.Add("iscase", "true");         // case sensitive captcha
            //image_params.Add("isphrase", "true");       // text contains at least one space (phrase)
            //image_params.Add("ismath", "true");         // instructs worker that a math captcha has to be solved
            //image_params.Add("alphanumeric", "1");      // 1 - digits only, 2 - letters only
            //image_params.Add("minlength", "2");         // captcha text length (minimum)
            //image_params.Add("maxlength", "5");         // captcha text length (maximum)

            Console.WriteLine("Waiting for captcha to be solved...");
            string captcha_id = i.submit_image("captcha.jpg", image_params);
            Dictionary<string, string> response = i.retrieve_response(captcha_id);
            ImageTypers.Utils.print_response(response);
        }
    }
}
