using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ImageTypers
{
    class Geetest
    {
        private string _captcha_id = "";
        private string _response = "";

        public Geetest(string captcha_id)
        {
            this._captcha_id = captcha_id;
        }
        /// <summary>
        /// Save captcha response to obj
        /// </summary>
        /// <param name="response"></param>
        public void set_response(string response)
        {
            this._response = response;
        }
        /// <summary>
        /// Getter for ID
        /// </summary>
        /// <returns></returns>
        public string captcha_id()
        {
            return this._captcha_id;
        }
        /// <summary>
        /// Getter for response
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, string> response()
        {
            var s = this._response.Split(new string[] { ";;;" }, StringSplitOptions.None);
            if(s.Length == 3)
            {
                var d = new Dictionary<string, string>();
                d.Add("challenge", s[0]);
                d.Add("validate", s[1]);
                d.Add("seccode", s[2]);
                return d;
            }
            throw new Exception("invalid geetest response");
        }
    }
}
