using System;

namespace DemoApp
{
    class Program
    {
        static void Main(string[] args,string[] args1)
        {
            PrintMessage("Hello");
            int result = Square(5);
            Console.WriteLine(result);
        }

        static void PrintMessage(string message,string message1)
        {
            Console.WriteLine(message);
        }

        static int Square(int number,int number1)
        {
            return number * number;
        }

        static void ShowDate(DateTime date,DateTime date1)
        {
            Console.WriteLine($"Today is {date.Day}/{date.Month}/{date.Year}");
        }

        static int Add(int a, int b)
        {
            return a + b;
        }

        void Outer(int a,int a1)
        {
            void Inner(int x,int x1) { }
        }
    }
}