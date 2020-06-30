using System;
using System.Collections.Generic;
using System.Text;

namespace Farm
{
    class CurrentVegetable
    {
        private string name;
        private int countHarvest, countSeeds;
        
        public CurrentVegetable()
        {
            name = "";
            countHarvest = 0;
            countSeeds = 0;
        }

        public string Name
        {
            set
            {
                this.name = value;
            }
            get
            {
                return name;
            }
        }

        public int CountHarvest
        {
            set
            {
                this.countHarvest = value;
            }
            get
            {
                return countHarvest;
            }
        }

        public int CountSeeds
        {
            set
            {
                this.countSeeds = value;
            }
            get
            {
                return countSeeds;
            }
        }
    }
}
