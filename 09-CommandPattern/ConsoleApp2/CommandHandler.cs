using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp2
{
    public class CommandHandler
    {
        private readonly IList<ICommand> _commands;
        
        public CommandHandler()
        {
            _commands = new List<ICommand>();
        }

        public void Add(ICommand command)
        {
            _commands.Add(command);
        }

        public void Run()
        {
            var executedCommands = new List<ICommand>();

            foreach (var command in _commands)
            {
                var result = command.Execute();

                if (!result)
                {
                    Console.WriteLine("Error");

                    break;
                }

                executedCommands.Insert(0, command);

                Console.WriteLine("OK");
            }

            if (_commands.Count != executedCommands.Count)
            {
                foreach (var command in executedCommands)
                {
                    command.Rollback();
                }
            }

            foreach (var command in _commands)
            {
                if (command.Errors.Count == 0)
                {
                    continue;
                }

                Console.WriteLine(command.ToString());

                foreach (var error in command.Errors)
                {
                    Console.WriteLine("  - " + error);
                }
            }
        }
    }
}
