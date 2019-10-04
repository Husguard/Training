

using System;
using System.Collections.Generic;

namespace IMJunior
{
    class Program
    {
        static void Main(string[] args)
        {
            int age = 0, points = 25, operandPoints = 0;
            Dictionary<string, int> subjects = new Dictionary<string, int>(3);
            subjects.Add("Сила", 0);
            subjects.Add("Ловкость", 0);
            subjects.Add("Интеллект", 0);

            Welcome();

            while (points > 0)
            {
                PrintCurrent(points, subjects);

                Console.WriteLine("Какую характеристику вы хотите изменить?");
                string subject = Console.ReadLine();

                Console.WriteLine(@"Что вы хотите сделать? +\-");
                string operation = Console.ReadLine();

                Console.WriteLine(@"Колличество поинтов которые следует {0}", operation == "+" ? "прибавить" : "отнять");

                Read(ref operandPoints);

                if (operation == "-") operandPoints = -operandPoints;

                Change(subject, operandPoints, ref points, ref subjects);
            }
            Console.WriteLine("Вы распределили все очки. Введите возраст персонажа:");
            Read(ref age);
            PrintCurrent(points, subjects);
            Console.WriteLine("Возраст - {0}", age);
        }
        static void Welcome()
        {
            Console.WriteLine("Добро пожаловать в меню выбора создания персонажа!");
            Console.WriteLine("У вас есть 25 очков, которые вы можете распределить по умениям");
            Console.WriteLine("Нажмите любую клавишу чтобы продолжить...");
            Console.ReadKey();
        }
        static void Change(string subject, int operandPoints, ref int points, ref Dictionary<string, int> subjects)
        {
            int overhead;
            foreach (string obj in subjects.Keys)
            {
                if (obj.Equals(subject, StringComparison.CurrentCultureIgnoreCase))
                {
                    if (operandPoints > 0)
                    {
                        overhead = operandPoints - (10 - subjects[obj]);
                        overhead = overhead < 0 ? 0 : overhead;
                    }
                    else
                    {
                        overhead = subjects[obj] + operandPoints;
                        overhead = overhead < 0 ? overhead : 0;
                    }
                    operandPoints -= overhead;
                    subjects[obj] = subjects[obj] + operandPoints;
                    points = points - operandPoints;
                    break;
                }
            }
        }
        static void PrintCurrent(int points, Dictionary<string, int> subjects)
        {
            string Visual = string.Empty;
            Console.Clear();
            Console.WriteLine("Поинтов - {0}", points);
            foreach (KeyValuePair<string, int> obj in subjects)
            {
                Visual = string.Empty.PadLeft(obj.Value, '#').PadRight(10, '_');
                Console.WriteLine("{0} - [{1}]", obj.Key, Visual);
            }
        }
        static void Read(ref int data)
        {
            string ageRaw = string.Empty;
            do
            {
                ageRaw = Console.ReadLine();
            } while (!int.TryParse(ageRaw, out data));
        }
    }

}

