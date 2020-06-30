using System;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Text;
using System.Threading;
using System.Timers;

namespace Farm
{
    class Actions
    {
        Info info = new Info();

        public int Buy(ref int userMoney, int cost)
        {
            int seeds;

            Console.WriteLine("How many seeds do you want to buy?");
            while (!int.TryParse(Console.ReadLine(), out seeds))
            {
                Console.WriteLine("Enter correct value!");
            }

            if(userMoney - cost * seeds < 0)
            {
                Console.WriteLine("Not enough money!");
                Thread.Sleep(500);
                seeds = 0;
            }
            else
            {
                userMoney = userMoney - cost * seeds;
            }

            return seeds;
        }

        public void SellSeeds(ref CurrentVegetable[] vegetables, ref int money, Info info)
        {
            int choice;
            int vegetableID;

            Console.WriteLine("Which type of seed do you want to sell?");

            while (!int.TryParse(Console.ReadLine(), out vegetableID) || vegetableID > vegetables.Length + 1 || vegetableID < 0 || info.inventory[vegetableID - 1].CountSeeds == 0)
            {
                Console.WriteLine("Enter correct value(maybe you dont have seed of this type)!");
            }

            if(vegetableID - 1 > vegetables.Length)
            {
                return;
            }

            Console.WriteLine("How many seeds do you want to sell?");

            while (!int.TryParse(Console.ReadLine(), out choice) || choice > vegetables[vegetableID - 1].CountSeeds || choice < 0)
            {
                Console.WriteLine("Enter correct value!");
            }

            vegetables[vegetableID - 1].CountSeeds -= choice;
            money += choice * FindPrice(vegetables[vegetableID - 1], info);
        }

        private int FindPrice(CurrentVegetable findingVegetable, Info db)
        {
            foreach(Vegetable vegetable in info.vegetables)
            {
                if(findingVegetable.Name.Equals(vegetable.name)) {
                    return vegetable.money;
                }
            }

            return -1;
        }

        private void PrintInventory(CurrentVegetable[] inventory)
        {
            int i = 1;

            foreach(CurrentVegetable vegetable in inventory)
            {
                Console.WriteLine($"{i}){vegetable.Name} \nSeeds: {vegetable.CountSeeds} \nHarvest: {vegetable.CountHarvest}\n");
                 i++;
            }
        }

        public void Sell(Info info, ref int userMoney)
        {
            int choice, vegetableID, amount;

            Console.Clear();
            PrintInventory(info.inventory);

            Console.WriteLine("What do you want to sell?" + "\n1)Seeds \n2)Harvest \n3)Cancel\n");

            while (!int.TryParse(Console.ReadLine(), out choice) || choice > 3 || choice < 1)
            {
                Console.WriteLine("Choose correct item!\n");
            }

            if (choice == 3)
                return;


            Console.Clear();
            PrintInventory(info.inventory);
            Console.WriteLine($"{info.inventory.Length + 1})Cancel");


            Console.WriteLine("\nChoose vegetable:");
            while (!int.TryParse(Console.ReadLine(), out vegetableID) || vegetableID > info.inventory.Length + 1 || vegetableID < 1)
            {
                Console.WriteLine("Choose correct vegetable!");
            }

            if (vegetableID == info.inventory.Length + 1)
                return;


            if ((info.inventory[vegetableID - 1].CountSeeds == 0 && choice == 1) || (info.inventory[vegetableID - 1].CountHarvest == 0 && choice == 2))
            {
                Console.WriteLine("\nNot enough items!");
                Thread.Sleep(600);
                return;
            }


            Console.WriteLine("Amount:");
            while (!int.TryParse(Console.ReadLine(), out amount) || amount < 1)
            {
                Console.WriteLine("\nEnter correct value!");
            }


            switch (choice)
            {
                case 1:
                    if(info.inventory[vegetableID - 1].CountSeeds == 0 || info.inventory[vegetableID - 1].CountSeeds < amount)
                    {
                        Console.WriteLine("Not enough items!");
                        Thread.Sleep(500);
                        return;
                    }
                    else
                    {
                        info.inventory[vegetableID - 1].CountSeeds -= amount;
                        info.userMoney += amount * FindPrice(info.inventory[vegetableID - 1], info);
                    }
                    break;
                case 2:
                    if (info.inventory[vegetableID - 1].CountHarvest == 0 || info.inventory[vegetableID - 1].CountHarvest < amount)
                    {
                        Console.WriteLine("Not enough items!");
                        Thread.Sleep(500);
                        return;
                    }
                    else
                    {
                        float money = userMoney;
                        info.inventory[vegetableID - 1].CountHarvest -= amount;
                        money += amount * FindPrice(info.inventory[vegetableID - 1], info) * info.profit;
                        info.userMoney = Convert.ToInt32(money);
                    }
                    break;
                case 3:
                    return;
                    break;
            }
        }

        public void Plant(ref Info info)
        {
            int line, column;
            int seedsType;

            Console.Clear();
            info.ShowFarm();


            Console.WriteLine("\nEnter column coordinate:");
            while (!int.TryParse(Console.ReadLine(), out column) || column > 3 || column < 1)
            {
                Console.WriteLine("\nEnter correct coordinate!");
            }

            Console.WriteLine("\nEnter line coordinate:");
            while (!int.TryParse(Console.ReadLine(), out line) || line > 3 || line < 1)
            {
                Console.WriteLine("\nEnter correct coordinate!");
            }



            Console.Clear();
            PrintInventory(info.inventory);
            Console.WriteLine($"{info.inventory.Length + 1})Cancel");


            Console.WriteLine("Choose seeds:");
            while (!int.TryParse(Console.ReadLine(), out seedsType) || seedsType > info.inventory.Length + 1 || seedsType < 1)
            {
                Console.WriteLine("\nEnter correct value!");
            }


            if (seedsType == info.inventory.Length + 1)
                return;


            if (info.farm[line - 1, column - 1] == null && info.inventory[seedsType - 1].CountSeeds != 0) {

                info.farm[line - 1, column - 1] = GetPlantingVegetable(info, info.inventory[seedsType - 1]);
                info.farm[line - 1, column - 1].plantingTime = DateTime.Now;
                info.inventory[seedsType - 1].CountSeeds--;
            }
            else
            {
                Console.WriteLine("Ops!Maybe you dont have enought seeds or this cell isn't empty!(Press any key to continue)");
                Console.ReadKey();
            }


            Console.Clear();
        }

        public void WringOut(ref Info info)
        {
            int column = 0, line = 0;

            Console.Clear();
            info.ShowFarm();

            GetCoordinates(ref line, ref column);

            if(info.farm[line - 1,column - 1] == null)
            {
                Console.WriteLine("Already empty!");
                Thread.Sleep(1000);
                return;
            }
            else
            {
                info.farm[line - 1, column - 1] = null;
            }
        }

        public void Collect(ref Info info)
        {
            int column = 0, line = 0;

            Console.Clear();
            info.ShowFarm();

            GetCoordinates(ref line, ref column);

            if(info.farm[line - 1, column - 1] != null && info.farm[line - 1, column - 1].isReady)
            {
                info.inventory[GetCollectingVegetableId(info.inventory, info.farm[line - 1,column - 1])].CountHarvest++;
                info.farm[line - 1, column - 1] = null;
            }
            else
            {
                Console.WriteLine("Empty!");
                Thread.Sleep(1000);
            }
        }

        public Vegetable GetPlantingVegetable(Info db, CurrentVegetable vegetable) //helps to get vegetable from inventory in db
        {
            foreach(Vegetable vg in db.vegetables)
            {
                if (vg.name.Equals(vegetable.Name))
                {
                    return vg;
                }
            }
            return null;
        }

        public int GetCollectingVegetableId(CurrentVegetable[] inventory, Vegetable findingVegetable)
        {
            int i = 0;
            foreach (CurrentVegetable vg in inventory)
            {
                if (vg.Name.Equals(findingVegetable.name))
                {
                    return i;
                }
                i++;
            }
            return -1;
        }

        public void GetCoordinates(ref int line, ref int column)
        {
            Console.WriteLine("\nEnter column coordinate:");
            while (!int.TryParse(Console.ReadLine(), out column) || column > 3 || column < 1)
            {
                Console.WriteLine("\nEnter correct coordinate!");
            }

            Console.WriteLine("\nEnter line coordinate:");
            while (!int.TryParse(Console.ReadLine(), out line) || line > 3 || line < 1)
            {
                Console.WriteLine("\nEnter correct coordinate!");
            }
        }
    }
}
