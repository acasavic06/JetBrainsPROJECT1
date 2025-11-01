using System;

namespace DemoApp
{
    class Program
    {
        static void Main(string[] args)
        {
            PrintMessage("Hello");
            int result = Square(5);
            Console.WriteLine(result);
        }

        static void PrintMessage(string message)
        {
            Console.WriteLine(message);
        }

        static int Square(int number)
        {
            return number * number;
        }

        static void ShowDate(DateTime date)
        {
            Console.WriteLine($"Today is {date.Day}/{date.Month}/{date.Year}");
        }

        static int Add(int a, int b)
        {
            return a + b;
        }

        void Outer(int a)
        {
            void Inner(int x) { }
        }
    }
}