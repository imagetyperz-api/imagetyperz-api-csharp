using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ImageTypers;

namespace ImageTypers
{
    class Program
    {
        /// <summary>
        /// Command line method
        /// </summary>
        /// <param name="args"></param>
        static void command_line(string[] args)
        {
            new CommandLineController(args).run();      // run command-line controller
        }

        /// <summary>
        /// Main method
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            try
            {
                Program.command_line(args);     // run command-line controller
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
