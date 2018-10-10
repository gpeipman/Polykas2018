using System;
using System.Collections.Generic;

namespace ConsoleApp2
{
    public class CompositeCommand : ICommand
    {
        private readonly IList<ICommand> _commands;
        private readonly IList<ICommand> _executedCommands;

        public CompositeCommand()
        {
            _commands = new List<ICommand>();
            _executedCommands = new List<ICommand>();
        }

        public void Add(ICommand command)
        {
            _commands.Add(command);
        }

        public bool Execute()
        {
            foreach (var command in _commands)
            {
                var result = command.Execute();

                if (!result)
                {
                    Console.WriteLine("Error");

                    return false;
                }

                _executedCommands.Insert(0, command);

                Console.WriteLine("OK");
            }

            return true;
        }

        public bool Rollback()
        {
            var result = true;

            foreach (var command in _executedCommands)
            {
                if(!command.Rollback())
                {
                    result = false;
                }
            }

            return result;
        }

        public IList<string> Errors
        {
            get
            {
                var errors = new List<string>();

                foreach(var command in _commands)
                {
                    if(command.Errors.Count == 0)
                    {
                        continue;
                    }

                    errors.AddRange(command.Errors);
                }

                return errors;
            }
        }
    }
}
