using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mwuhahaha
{
    class Monsters
    {
        private Dictionary<int, Monster> monsters = new Dictionary<int, Monster>();
        private static int moves = 0;       // The moves monsters have taken. As all of them have to move each turn this var can account for all individual monster moves.

        public Monsters(int count, World world)
        {
            for(int i = 0; i < count; i++)
            {
                this.monsters[Monster.monsterCount] = new Monster(world);       // As monsterCount is also the monster name this is a good way to assign them to the array
            }
        }

        public Dictionary<int, Monster> _Monsters
        {
            get { return this.monsters; }
            private set { }
        }

        // Accepts a list for the rare case that more than 2 monsters end up in the same city.
        public string Fight(int[] monsters, string city, World world)
        {
            List<string> nameList = new List<string>();     // Stores the names of the monsters for the console log
            // Kills all the monsters
            for(int i=0; i<monsters.Length; i++)
            {
                nameList.Add(monsters[i].ToString());       // Add monster name to list of names
                this.monsters.Remove(monsters[i]);        // Remove monster from list of monsters (ie. kill the monster)
            }
            world.DropCity(city);       // Removes city from the World object

            string _val = nameList[0];
            int count = 1;
            while (count < nameList.Count())
            {
                 _val += " and monster " + nameList[count];
                count++;
            }
            return city + "has been destroyed by monster " + _val + "!";
        }

        public int Moves
        {
            get { return Monsters.moves; }
            set { Monsters.moves = value; }
        }
    }

    class Monster
    {
        public static int monsterCount = 0;
        
        public Monster(World world)
        {
            this.Name = Monster.monsterCount;
            Monster.monsterCount++;     // Increase so each monster name is unique (monsterName = monster Name)
            Location = world.randomCity();      // Place in a random city
        }

        public int Name { private set; get; }

        public string Location { set; get; }

        public void Move(World world)
        {
            bool north = true,
                south = true,
                east = true,
                west = true;        // The bools will be set to false if the corresponding city is destroyed or non-existent
            int dir = 0;
            string myLoc = this.Location;
            bool validDir = false;
            Random rand = new Random();
            do
            {
                dir = rand.Next(0, 4);
                switch (dir)
                {
                    case 0:
                        if (world.Cities[myLoc].North.Length > 0)
                        {
                            if (world.Cities[world.Cities[myLoc].North].Destroyed)       // sets north to false if the city north is destroyed
                            {
                                north = false;
                            }
                            else
                            {
                                validDir = true;
                            }
                        }
                        else
                        {
                            north = false;      // sets north to false if there is no road north
                        }
                        break;
                    case 1:
                        if (world.Cities[myLoc].East.Length > 0)
                        {
                            if (world.Cities[world.Cities[myLoc].East].Destroyed)
                            {
                                east = false;
                            }
                            else
                            {
                                validDir = true;
                            }
                        }
                        else
                        {
                            east = false;
                        }
                        break;
                    case 2:
                        if (world.Cities[myLoc].South.Length > 0)
                        {
                            if (world.Cities[world.Cities[myLoc].South].Destroyed)
                            {
                                south = false;
                            }
                            else
                            {
                                validDir = true;
                            }
                        }
                        else
                        {
                            south = false;
                        }
                        break;
                    case 3:
                        if (world.Cities[myLoc].West.Length > 0)
                        {
                            if (world.Cities[world.Cities[myLoc].West].Destroyed)
                            {
                                west = false;
                            } else
                            {
                                validDir = true;
                            }
                        }
                        else
                        {
                            west = false;
                        }
                        break;
                }
            } while (!validDir && (north || east || south || west));
            
            /* if validDir is true and n,e,s,w are all false then the monster is trapped in their current city and cannot move */

            // If a valid direction was found update monster location
            if(validDir)
            {
                switch(dir)
                {
                    case 0:
                        Location = world.Cities[myLoc].North;
                        break;
                    case 1:
                        Location = world.Cities[myLoc].East;
                        break;
                    case 2:
                        Location = world.Cities[myLoc].South;
                        break;
                    case 3:
                        Location = world.Cities[myLoc].West;
                        break;
                }
            }
        }
    }
}
