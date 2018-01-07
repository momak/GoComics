using Fclp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GoComics
{
    class CommandLineOptions
    {
        public string DefaultCulture { get; set; }

        public int StartDayOffest { get; set; }

        public int DaysBefore { get; set; }
        
        public static FluentCommandLineParser<CommandLineOptions> Setup()
        {
            var p = new FluentCommandLineParser<CommandLineOptions>();

            p.Setup(arg => arg.DefaultCulture)
                .As('d', "DefaultCulture")
                .Required()
                .WithDescription("Enter Default Culture")
                .SetDefault(GlobalVars.DefaultCulture);

            p.Setup(arg => arg.StartDayOffest)
                .As('s', "StartDayOffset")
                .Required()
                .WithDescription("Start day offset")
                .SetDefault(GlobalVars.StartDayOffest);

            p.Setup(arg => arg.DaysBefore)
                .As('e', "DaysBefore")
                .WithDescription("Days before")
                .SetDefault(GlobalVars.DaysBefore);

            p.SetupHelp("h", "help", "?")
                .Callback(text =>
                {
                    Console.WriteLine();
                    Console.Write("Command line options:");

                    Console.WriteLine(text);
                });

            return p;
        }

        public static bool HandleStandardResult(ICommandLineParserResult result)
        {
            if (result.HelpCalled)
                return true;

            if (result.HasErrors)
            {
                Console.Error.WriteLine(result.ErrorText);
                Console.Error.WriteLine($"Type \"{Assembly.GetExecutingAssembly().GetName().Name} --help\" to show the command line syntax.");

                return true;
            }

            foreach (var option in result.AdditionalOptionsFound)
            {
                Console.Error.WriteLine(string.IsNullOrEmpty(option.Value)
                    ? $"Ignoring option {(option.Key.Length > 1 ? "--" : "-")}{option.Key}"
                    : $"Ignoring option {(option.Key.Length > 1 ? "--" : "-")}{option.Key}='{option.Value}'");
            }

            return false;
        }
    }
}
