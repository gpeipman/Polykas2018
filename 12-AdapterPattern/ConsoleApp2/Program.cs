using System;
using System.Collections.Generic;

namespace ConsoleApp2
{
    class Program
    {
        static FallbackLogger _logger = new FallbackLogger();

        static void Main(string[] args)
        {
            _logger.AddLogger(new SerilogLoggerWrapper());
            _logger.AddLogger(new FileLoggerWrapper());

            _logger.Info("Program started");

            Console.WriteLine("Press any key to continue ...");
            Console.ReadKey();
        }

        public interface ILogger
        {
            void Info(string message);
            void Error(string message);
        }

        public class FallbackLogger : ILogger
        {
            private IList<ILogger> _loggers = new List<ILogger>();

            public void AddLogger(ILogger logger)
            {
                _loggers.Add(logger);
            }

            public void Error(string message)
            {
                foreach(var logger in _loggers)
                {
                    try
                    {
                        logger.Error(message);
                        return;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(logger.GetType() + ex.Message);
                    }
                }
            }

            public void Info(string message)
            {
                foreach (var logger in _loggers)
                {
                    try
                    {
                        logger.Info(message);
                        return;
                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine(logger.GetType() + ex.Message);
                    }
                }
            }
        }

        public class FileLoggerWrapper : ILogger
        {
            private FileLogger _fileLogger = new FileLogger();

            public void Error(string message)
            {
                _fileLogger.WriteError(message);
            }

            public void Info(string message)
            {
                _fileLogger.WriteInfo(message);
            }
        }

        public class SerilogLoggerWrapper : ILogger
        {
            private SerilogLogger _serilogLogger = new SerilogLogger();

            public void Error(string message)
            {
                throw new Exception();

                _serilogLogger.LogError(message);
            }

            public void Info(string message)
            {
                throw new Exception();

                _serilogLogger.LogInfo(message);
            }
        }

        public class FileLogger
        {
            public void WriteInfo(string message)
            {
                Console.WriteLine("FileLogger: Info: " + message);
            }

            public void WriteError(string message)
            {
                Console.WriteLine("FileLogger: Error: " + message);
            }
        }

        public class SerilogLogger
        {
            public void LogInfo(string message)
            {
                Console.WriteLine("SerilogLogger: Info: " + message);
            }

            public void LogError(string message)
            {
                Console.WriteLine("SerilogLogger: Error: " + message);
            }
        }
    }
}
