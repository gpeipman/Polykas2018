using System;
using System.Collections.Generic;

namespace MediaGallery.Commands
{
    public abstract class CompositeCommand<T> : ICommand<T>
    {
        private readonly IList<ICommand<T>> _commands;
        private readonly IList<ICommand<T>> _executedCommands;

        public CompositeCommand()
        {
            _commands = new List<ICommand<T>>();
            _executedCommands = new List<ICommand<T>>();
        }

        protected void Add(ICommand<T> command)
        {
            _commands.Add(command);
        }

        public bool Execute(T parameter)
        {
            foreach (var command in _commands)
            {
                var result = command.Execute(parameter);

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
                if (!command.Rollback())
                {
                    result = false;
                }
            }

            return result;
        }

        public List<string> Validate(T parameter)
        {
            var messages = new List<string>();

            foreach(var command in _commands)
            {
                messages.AddRange(command.Validate(parameter));
            }

            return messages;
        }
    }
}
