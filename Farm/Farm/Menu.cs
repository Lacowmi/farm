using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Farm
{
    class Menu
    {
        public static Info info = new Info();
        private static int choice;
        public static Actions actions = new Actions();

        public void MainMenu()
        {
            Console.Clear();

            PrintMoney();
            Console.WriteLine("1.Show farm");
            Console.WriteLine("2.Inventory");
            Console.WriteLine("3.Shop");
            Console.WriteLine("4.Quit");
            Console.WriteLine("\n");

            while (!int.TryParse(Console.ReadLine(), out choice) || choice > 4)
            {
                Console.WriteLine("Enter correct value!");
                Thread.Sleep(500);
                Console.Clear();
                MainMenu();
            }

            switch (choice)
            {
                case 1:
                    Farm();
                    break;
                case 2:
                    Inventory();
                    break;
                case 3:
                    ShopMenu();
                    break;
                case 4:
                    Environment.Exit(0);
                    break;
            }
        }

        public void ShopMenu()
        {
            Console.Clear();

            PrintMoney();

            for (int i = 0;i < info.vegetables.Length; i++)
            {
                Console.WriteLine($"{i+1}) {info.vegetables[i].name} \nPrice - {info.vegetables[i].money} \nSelling price - {info.vegetables[i].money * info.profit} \nTime - {info.vegetables[i].time} seconds \n");
            }
            Console.WriteLine($"{info.vegetables.Length + 1}. Quit");

            while (!int.TryParse(Console.ReadLine(), out choice) || choice > info.vegetables.Length + 1 || choice < 0)
            {
                Console.WriteLine("Enter correct value!");
                Thread.Sleep(500);
                Console.Clear();
                ShopMenu();
            }
            if(choice <= info.vegetables.Length)
            {
               Console.Clear();
                if(info.userMoney == 0)
                {
                    Console.WriteLine("You have no money!");
                    Thread.Sleep(500);
                    ShopMenu();
                }
                else
                {
                    info.inventory[choice - 1].CountSeeds += actions.Buy(ref info.userMoney, info.vegetables[choice - 1].money);
                    Console.Clear();
                    ShopMenu();
                }
            }
            else
            {
                MainMenu();
            }
        }

        public void Farm()
        {
            int choice;

            Console.Clear();

            PrintMoney();
            info.ShowFarm();

            Console.WriteLine();
            Console.WriteLine("1.Wring out");
            Console.WriteLine("2.Collect");
            Console.WriteLine("3.Update");
            Console.WriteLine("4.Back to menu");
            Console.WriteLine("\n");

            while (!int.TryParse(Console.ReadLine(), out choice) || choice > 4)
            {
                Console.WriteLine("Enter correct value!");
                Thread.Sleep(500);
                Console.Clear();
                Farm();
            }

            switch (choice)
            {
                case 1:
                    actions.WringOut(ref info);
                    Farm();
                    break;
                case 2:
                    actions.Collect(ref info);
                    Farm();
                    break;
                case 3:
                    Farm();
                    break;
                case 4:
                    MainMenu();
                    break;
            }
        }

        public void Inventory()
        {
            Console.Clear();

            PrintMoney();

            int i = 1;
            foreach(CurrentVegetable vegetable in info.inventory)
            {
                Console.WriteLine(i + ". " + vegetable.Name + " \nSeeds: " + vegetable.CountSeeds + "\nHarvest: " + vegetable.CountHarvest);
                Console.WriteLine();
                i++;
            }

            Console.WriteLine("1.Plant");
            Console.WriteLine("2.Sell");
            Console.WriteLine("3.Back to menu");

            while (!int.TryParse(Console.ReadLine(), out choice) || choice > 3)
            {
                Console.WriteLine("Enter correct value!");
                Thread.Sleep(500);
                Console.Clear();
                Inventory();
            }

            switch (choice)
            {
                case 1:
                    actions.Plant(ref info);
                    Inventory();
                    break;
                case 2:
                    actions.Sell(info, ref info.userMoney);
                    Inventory();
                    break;
                case 3:
                    MainMenu();
                    break;
            }
        }

        public void PrintMoney()
        {
            Console.WriteLine(" Money: " + info.userMoney + "\n");
        }
    }
}
