using UnityEngine;
using System.Collections.Generic;
using System.IO;

namespace Terminal.Core
{
    public class Interpreter : MonoBehaviour
    {
        [SerializeField] private TerminalManager Terminal;

        private List<string> response = new List<string>();
        private Dictionary<string, CommandBase> commands = new Dictionary<string, CommandBase>();

        void Start()
        {
            // Initialize commands
            var commandList = new CommandBase[]
            {
            new HelpCommand(),
            new PrintCommand()
                // Add new command instances here
            };

            foreach (var command in commandList)
            {
                commands[command.Name] = command;
            }
        }

        public List<string> Interpret(string userInput)
        {
            response.Clear();

            string[] args = userInput.Split(" ");
            if (args.Length == 0)
            {
                response.Add("Please type something.");
                return response;
            }

            if (commands.TryGetValue(args[0].ToUpper(), out CommandBase command))
            {
                response = command.Execute(args);
            }
            else
            {
                response.Add("Command not recognized. Type HELP for a list of commands.");
            }

            return response;
        }

        public List<string> LoadTitle(string path, int spacing)
        {
            StreamReader file = new StreamReader(Path.Combine(Application.streamingAssetsPath, path));

            for (int i = 0; i < spacing; i++)
            {
                response.Add("");
            }

            while (!file.EndOfStream)
            {
                response.Add(file.ReadLine());
            }

            for (int i = 0; i < spacing; i++)
            {
                response.Add("");
            }

            file.Close();

            return response;
        }
    }
}