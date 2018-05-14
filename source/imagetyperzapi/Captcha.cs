using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace ImageTypers
{
    class Captcha
    {
        private string _captcha_id = "";
        private string _text = "";

        /// <summary>
        /// Takes response from server API
        /// </summary>
        /// <param name="response">id|text</param>
        public Captcha(string response)
        {
            this.parse_response(response);
        }
        /// <summary>
        /// Parse response
        /// </summary>
        /// <param name="response"></param>
        private void parse_response(string response)
        {
            var s = response.Split('|');        // split by |
            if (s.Length < 2)                   // check if at least 2 items
            {
                throw new Exception(string.Format("cannot parse response: {0}", response));
            }

            this._captcha_id = s[0];
            var a = s.ToList().GetRange(1, s.Length - 1).ToArray();
            this._text = String.Join("|", a);
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
        /// Getter for text
        /// </summary>
        /// <returns></returns>
        public string text()
        {
            return this._text;
        }
    }
}
