using System;
using System.IO;

namespace ConsoleApplication3
{
    interface ILogger
    {
        void WriteLog(string message);
    }

    class Program
    {
        static void Main(string[] args)
        {
            Pathfinder logToFile = new Pathfinder(new FileLogWritter(), "Написать в файл");
            Pathfinder logToConsole = new Pathfinder(new ConsoleLogWritter(), "Написать в консоль");
            Pathfinder logToFileOnFriday =
                new Pathfinder(new SecureLogWritter(new FileLogWritter()), "Написать в файл по пятницам");
            Pathfinder logToConsoleOnFriday = new Pathfinder(new SecureLogWritter(new ConsoleLogWritter()),
                "лог в консоль по пятницам");
            Pathfinder logToConsoleAndOnFridayToFail = new Pathfinder(
                new ConsoleLogWritter(new SecureLogWritter(new FileLogWritter())),
                "Пишет лог в консоль а по пятницам ещё и в файл.");

            logToFile.Find();
            logToConsole.Find();
            logToFileOnFriday.Find();
            logToConsoleOnFriday.Find();
            logToConsoleAndOnFridayToFail.Find();
        }
    }

    class Pathfinder
    {
        private ILogger _logger;
        private string _message;

        public Pathfinder(ILogger logger, string message = null)
        {
            _logger = logger;
            _message = message;
        }

        public void Find()
        {
            _logger.WriteLog(_message);
        }
    }

    class ConsoleLogWritter : ILogger
    {
        private ILogger _logger;

        public ConsoleLogWritter(ILogger logger = null)
        {
            _logger = logger;
        }

        public void WriteLog(string message)
        {
            Console.WriteLine(message);
        }
    }

    class FileLogWritter : ILogger
    {
        private ILogger _logger;

        public FileLogWritter(ILogger logger = null)
        {
            _logger = logger;
        }

        public void WriteLog(string message)
        {
            File.WriteAllText("log.txt", message);
        }
    }

    class SecureLogWritter : ILogger
    {
        private ILogger _logger;

        public SecureLogWritter(ILogger logger)
        {
            _logger = logger;
        }

        public void WriteLog(string message)
        {
            if (DateTime.Now.DayOfWeek == DayOfWeek.Friday)
            {
                _logger.WriteLog(message);
            }
        }
    }
}