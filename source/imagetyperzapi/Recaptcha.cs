using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ImageTypers
{
    class Recaptcha
    {
        private string _captcha_id = "";
        private string _response = "";

        public Recaptcha(string captcha_id)
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
        public string response()
        {
            return this._response;
        }
    }
}
