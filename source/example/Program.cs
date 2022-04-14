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
        /// Main method
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            try
            {
                TestRecaptcha.run();
                // TestImage.run();
                // TestGeetest.run();
                // TestGeetestV4.run();
                // TestCapy.run();
                // TestHcaptcha.run();
                // TestTiktok.run();
                // TestFuncaptcha.run();
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
