using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Text;

namespace Farm
{
    class Vegetable
    {
        public int time, money;
        public string name;
        public DateTime plantingTime;
        public bool isReady;

        public Vegetable(int time, int money, string name)
        {
            this.time = time;
            this.money = money;
            this.name = name;
            isReady = false;
        }
    }
}
