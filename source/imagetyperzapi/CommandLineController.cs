using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ImageTypers
{
    public class Arguments
    {
        private string _username;
        private string _password;
        private string _mode;

        private string _captcha_file = "";
        private string _output_file = "";
        private string _ref_id = "";
        private string _page_url = "";
        private string _site_key = "";

        private string _captcha_id = "";

        private string _proxy = "";
        private string _token = "";

        // v3
        private string _type = "0";
        private string _v3_action = "";
        private string _v3_score = "";

        private string _user_agent = "";

        private bool _case_sensitive = false;

        public void set_user_agent(string user_agent)
        {
            this._user_agent = user_agent;
        }
        public string get_user_agent()
        {
            return this._user_agent;
        }
        public void set_v3_score(string score)
        {
            this._v3_score = score;
        }
        public string get_v3_score()
        {
            return this._v3_score;
        }
        public void set_v3_action(string action)
        {
            this._v3_action = action;
        }
        public string get_v3_action()
        {
            return this._v3_action;
        }
        public void set_type(string type)
        {
            this._type = type;
        }
        public string get_type()
        {
            return this._type;
        }
        public string get_token()
        {
            return _token;
        }

        public void set_token(string token)
        {
            this._token = token;
        }

        public string get_username()
        {
            return _username;
        }

        public void set_username(string _username)
        {
            this._username = _username;
        }

        public string get_password()
        {
            return _password;
        }

        public void set_password(string _password)
        {
            this._password = _password;
        }

        public string get_mode()
        {
            return _mode;
        }

        public void set_mode(string _mode)
        {
            this._mode = _mode;
        }

        public string get_captcha_file()
        {
            return _captcha_file;
        }

        public void set_captcha_file(string _captcha_file)
        {
            this._captcha_file = _captcha_file;
        }

        public string get_output_file()
        {
            return _output_file;
        }

        public void set_output_file(string _output_file)
        {
            this._output_file = _output_file;
        }

        public string get_ref_id()
        {
            return _ref_id;
        }

        public void set_ref_id(string _ref_id)
        {
            this._ref_id = _ref_id;
        }

        public string get_page_url()
        {
            return _page_url;
        }

        public void set_page_url(string _page_url)
        {
            this._page_url = _page_url;
        }

        public string get_site_key()
        {
            return _site_key;
        }

        public void set_site_key(string _site_key)
        {
            this._site_key = _site_key;
        }

        public string get_captcha_id()
        {
            return _captcha_id;
        }

        public void set_captcha_id(string _captcha_id)
        {
            this._captcha_id = _captcha_id;
        }

        public bool is_case_sensitive()
        {
            return _case_sensitive;
        }

        public void set_case_sensitive(bool _case_sensitive)
        {
            this._case_sensitive = _case_sensitive;
        }

        public string get_proxy()
        {
            return _proxy;
        }

        public void set_proxy(string _proxy)
        {
            this._proxy = _proxy;
        }
    }

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

            // v3
            if (d.ContainsKey("-type")) this._arguments.set_type(d["-type"]);
            if (d.ContainsKey("-v3_min_score")) this._arguments.set_v3_score(d["-v3_min_score"]);
            if (d.ContainsKey("-v3_action")) this._arguments.set_v3_action(d["-v3_action"]);
            // user agent
            if (d.ContainsKey("-user_agent")) this._arguments.set_user_agent(d["-user_agent"]);
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
