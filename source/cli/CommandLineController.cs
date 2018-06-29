using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ImageTypers
{
    class CommandLineController
    {
        private Arguments _arguments;

        public CommandLineController(string[] args)
        {
            this.init_parse(args);      // init & parse args
        }

        /// <summary>
        /// Init command-line arguments
        /// </summary>
        /// <param name="args"></param>
        private void init_parse(string[] args)
        {
            // init dict with key, value pair
            var d = new Dictionary<string, string>();
            for (int i = 0; i < args.Length; i += 2)
            {
                if (i + 1 == args.Length)
                {
                    break;
                }
                d.Add(args[i].Replace("\"", "").Trim(), args[i + 1].Replace("\"", "").Trim());
            }

            // check our dicts length first
            if (d.Count == 0)
            {
                throw new Exception("no arguments given. Check README.md file for examples on how to use it");
            }

            // not that we have the dict, check for what we're looking for
            // init the Arguments obj
            // ------------------------------------------------------------
            this._arguments = new Arguments();      // init new obj

            // check what we're looking for
            // ----------------------------
            if (d.ContainsKey("-t"))
            {
                this._arguments.set_token(d["-t"]);  // we have token
            }
            else
            {
                if (!d.ContainsKey("-u"))
                {
                    throw new Exception("-u arguments is required or use -t (token)");
                }
                if (!d.ContainsKey("-p"))
                {
                    throw new Exception("-p arguments is required or use -t (token)");
                }
                this._arguments.set_username(d["-u"]);
                this._arguments.set_password(d["-p"]);
            }
            if (!d.ContainsKey("-m"))
            {
                throw new Exception("-m arguments is required");
            }


            this._arguments.set_mode(d["-m"]);

            // init the optional ones
            // ----------------------
            if (d.ContainsKey("-i"))
            {
                this._arguments.set_captcha_file((d["-i"]));
            }
            if (d.ContainsKey("-o"))
            {
                this._arguments.set_output_file((d["-o"]));
            }
            if (d.ContainsKey("-pageurl"))
            {
                this._arguments.set_page_url((d["-pageurl"]));
            }
            if (d.ContainsKey("-sitekey"))
            {
                this._arguments.set_site_key((d["-sitekey"]));
            }
            if (d.ContainsKey("-captchaid"))
            {
                this._arguments.set_captcha_id((d["-captchaid"]));
            }
            if (d.ContainsKey("-case"))
            {
                bool b = false;
                string cc = d["-case"];
                if (cc.Equals("1"))
                    b = true;       // make it true
                this._arguments.set_case_sensitive(b);
            }
            if (d.ContainsKey("-proxy"))
            {
                this._arguments.set_proxy(d["-proxy"]);
            }
        }

        /// <summary>
        /// Run method
        /// </summary>
        public void run()
        {
            try
            {
                this._run();
            }
            catch (Exception ex)
            {
                this.save_error(ex.Message);        // save error to error text file
                throw;      // re-throw
            }
        }

        /// <summary>
        /// Private _run method
        /// </summary>
        private void _run()
        {
            var a = this._arguments;        // for easier use local

            ImageTypersAPI i;
            var token = a.get_token();
            if (!string.IsNullOrWhiteSpace(token))
            {
                i = new ImageTypersAPI(token);
            }
            else
            {
                i = new ImageTypersAPI("");
                i.set_user_and_password(a.get_username(), a.get_password());
            }
            switch (a.get_mode())
            {
                case "1":
                    // solve normal captcha
                    // -------------------------
                    /*string captcha_file = a.get_captcha_file();
                    if (string.IsNullOrWhiteSpace(captcha_file))
                        throw new Exception("Invalid captcha file");
                    bool cs = a.is_case_sensitive();
                    string resp = i.solve_captcha(captcha_file, cs);
                    this.show_output(resp);*/
                    break;
                case "2":
                    // submit recapthca
                    // ----------------------------------------------------
                    string page_url = a.get_page_url();
                    if (string.IsNullOrWhiteSpace(page_url))
                        throw new Exception("Invalid recaptcha pageurl");
                    string site_key = a.get_site_key();
                    if (string.IsNullOrWhiteSpace(site_key))
                        throw new Exception("Invalid recaptcha sitekey");
                    string proxy = a.get_proxy();

                    string captcha_id = "";

                    Dictionary<string, string> d = new Dictionary<string, string>();
                    d.Add("page_url", page_url);
                    d.Add("sitekey", site_key);

                    // v3
                    if (!string.IsNullOrWhiteSpace(a.get_type())) d.Add("type", a.get_type());
                    if (!string.IsNullOrWhiteSpace(a.get_v3_action())) d.Add("v3_action", a.get_v3_action());
                    if (!string.IsNullOrWhiteSpace(a.get_v3_score())) d.Add("v3_min_score", a.get_v3_score());

                    // user agent
                    if (!string.IsNullOrWhiteSpace(a.get_user_agent())) d.Add("user_agent", a.get_user_agent());

                    // check proxy
                    if (!string.IsNullOrWhiteSpace(proxy)) d.Add("proxy", proxy);
                    captcha_id = i.submit_recaptcha(d);
                    this.show_output(captcha_id);
                    break;
                case "3":
                    // retrieve captcha
                    string recaptcha_id = a.get_captcha_id();
                    if (string.IsNullOrWhiteSpace(recaptcha_id))
                        throw new Exception("recaptcha captchaid is invalid");
                    string recaptcha_response = i.retrieve_captcha(recaptcha_id);     // get recaptcha response
                    this.show_output(recaptcha_response);       // show response
                    break;
                case "4":
                    // get balance
                    // -------------------------
                    string balance = i.account_balance();
                    this.show_output(balance);      // show balance
                    break;
                case "5":
                    // set bad captcha
                    // -------------------------
                    string bad_id = a.get_captcha_id();
                    if (string.IsNullOrWhiteSpace(bad_id))
                        throw new Exception("captchaid is invalid");
                    string response = i.set_captcha_bad(bad_id);        // set it bad
                    this.show_output(response);     // show response
                    break;
                case "6":
                    // get proxy status
                    string was_used_id = a.get_captcha_id();
                    if (string.IsNullOrWhiteSpace(was_used_id))
                        throw new Exception("captchaid is invalid");
                    string rr = i.was_proxy_used(was_used_id);        // set it bad
                    this.show_output(rr);     // show response
                    break;
            }
        }

        #region misc
        /// <summary>
        /// Save error to file
        /// </summary>
        /// <param name="error"></param>
        private void save_error(string error)
        {
            this.save_text("error.txt", error);
        }

        /// <summary>
        /// Show output and save to file if given
        /// </summary>
        /// <param name="text"></param>
        private void show_output(string text)
        {
            Console.WriteLine(text);        // print to screen
            if (!string.IsNullOrWhiteSpace(this._arguments.get_output_file()))  // if outputfile arg given
            {
                this.save_text(this._arguments.get_output_file(), text);        // save to file
            }
        }

        /// <summary>
        /// Save text to file
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="text"></param>
        private void save_text(string filename, string text)
        {
            System.IO.File.WriteAllText(filename, text);
        }
        #endregion
    }
}
