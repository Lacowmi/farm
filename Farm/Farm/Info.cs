using System;
using System.Collections.Generic;
using System.Text;

namespace Farm
{
    class Info
    {
        public int userMoney = 100;

        private static Vegetable carrot = new Vegetable(10, 40, "Carrot");
        private static Vegetable potato = new Vegetable(15, 50, "Potato");
        private static Vegetable tomato = new Vegetable(20, 60, "Tomato");
        private static Vegetable cucumber = new Vegetable(5, 10, "Cucumber");
        private static Vegetable onion = new Vegetable(50, 100, "Onion");

        public  Vegetable[] vegetables = { carrot, potato, tomato, cucumber, onion };
        public  Vegetable[,] farm = new Vegetable[3,3];
        public  CurrentVegetable[] inventory = new CurrentVegetable[5];
        public float profit = 1.65f;

        public Info()
        {
            for(int i = 0; i < 5; i++)
            {
                inventory[i] = new CurrentVegetable();
            }

            inventory[0].Name = "Carrot";
            inventory[1].Name = "Potato";
            inventory[2].Name = "Tomato";
            inventory[3].Name = "Cucumber";
            inventory[4].Name = "Onion";
        }


        public void ShowFarm()          //вывод фермы пользователя
        {
            string cell = "";
            Console.Write("\n");

            for (int i = 0; i < 3; i++)
            {
                Console.Write("\t");
                for (int j = 0; j < 3; j++)
                {
                    if(farm[i,j] != null)
                    {
                        TimeSpan interval = DateTime.Now - farm[i, j].plantingTime;


                        if((int)interval.TotalSeconds < farm[i, j].time)
                        {
                            cell = $"{farm[i, j].name}({(int)interval.TotalSeconds}/{farm[i, j].time})";
                        }

                        if ((int)interval.TotalSeconds >= farm[i, j].time)
                        {
                            cell = $"{farm[i, j].name}(ready)";
                            farm[i, j].isReady = true;
                        }
                    }
                    else
                    {
                        cell = "none";
                    }


                    Console.Write(cell);
                    for (int k = 0; k < 23 - cell.Length; k++)
                    {
                        Console.Write(" ");
                    }
                }
                Console.WriteLine("\n");
            }
        }
    }
}
