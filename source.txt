using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandParser
{
    class Program
    {
        static void Main(string[] args)
        {
            new Program().Run(args);
        }

        public void Run(string[] args)
        {
            if (args.Length == 0) {
                ShowHelp();
            } else {
                try {
                    var commands = ParseCommands(args);

                    if (commands.ContainsKey("help")) {
                        ShowHelp();
                    } else {
                        foreach (var command in commands) {
                            if (command.Key == "map") {
                                ShowMap(command.Value.ToArray());
                            } else if (command.Key == "ping") {
                                Ping();
                            } else if (command.Key == "print") {
                                Print(command.Value.ToArray());
                            }
                        }
                    }
                } catch (Exception ex) {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        IDictionary<string, IList<string>> ParseCommands(string[] args)
        {
            var commands = new Dictionary<string, IList<string>>();

            var currentCommand = "";

            foreach (var arg in args) {
                if (arg == "/?" || arg == "/help" || arg == "-help") {
                    currentCommand = "help";
                } else if (arg == "-k") {
                    currentCommand = "map";
                } else if (arg == "-ping") {
                    currentCommand = "ping";
                } else if (arg == "-print") {
                    currentCommand = "print";
                } else if (arg.StartsWith("-")) {
                    throw new Exception(String.Format("Command {0} is not supported, use CommandParser.exe /? to see a set of allowed commands", arg));
                } else {
                    // This is not a command but a parameter - let's add it to our commands 
                    // dictionary, provided we have previously parsed a respective command

                    if (currentCommand == "") {
                        // The command has not been parsed yet, so we cannot proceed with this parameter
                        throw new Exception(String.Format("No command specified for the parameter {0}, use CommandParser.exe /? to see a set of allowed commands", arg));
                    } else {
                        // The commands[currentCommand] element was already created when the command was parsed,
                        // so just add the parameter to the list
                        commands[currentCommand].Add(arg);
                    }
                }

                if (!commands.ContainsKey(currentCommand)) {
                    commands[currentCommand] = new List<string>();
                }
            }

            // Once parsed, let's check for the correct use of the commands

            if (commands.ContainsKey("map") && commands["map"].Count == 0) {
                throw new Exception("Command -k must have at least one parameter");
            }

            if (commands.ContainsKey("ping") && commands["ping"].Count > 0) {
                throw new Exception("Command -ping does not have any parameters");
            }

            if (commands.ContainsKey("print") && commands["print"].Count == 0) {
                throw new Exception("Command -print must have at least one parameter");
            }

            return commands;
        }

        void ShowHelp()
        {
            Console.WriteLine("CommandParser.exe [/?] [/help] [-help] [-k key value] [-ping] [-print <print a value>]");
        }

        void ShowMap(string[] values)
        {
            for (var i = 0; i < values.Length; i = i + 2) {
                var key = values[i];
                var value = i + 1 < values.Length ? values[i + 1] : "null";
                Console.WriteLine(String.Format("{0} - {1}", key, value));
            }
        }

        void Ping()
        {
            Console.WriteLine("Pinging...");
            Console.Beep();
        }

        void Print(string[] messages)
        {
            Console.WriteLine(String.Join(" ", messages));
        }
    }
}