using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Mwuhahaha
{
    class World
    {

        private static Dictionary<string, City> cities = new Dictionary<string, City>();       // Dictionary of all the cities in the world. Static so cities can be changed when making a new object.
        public static Random rand = new Random();     // For the random city picker


        public World(string file)
        {
            StreamReader worldFile = new StreamReader(System.AppDomain.CurrentDomain.BaseDirectory + "\\extFiles\\" + file);    // Open text file
            // AddCity to World for every line
            while (!worldFile.EndOfStream)
            {
                string cityInfo = worldFile.ReadLine();
                this.AddCity(cityInfo);
            }
            worldFile.Close();
        }

        // Adds a city to the cities List of World
        private void AddCity(string cityInfo)
        {
            string north = "",
                south = "",
                east = "",
                west = "";

            string[] cityBreakDown = cityInfo.Split(' ');

            // Assign each direction a value if one exists
            foreach (string element in cityBreakDown) {
                string[] sep = element.Split('=');

                switch(sep[0])
                {
                    case "north":
                        north = sep[1];
                        break;
                    case "south":
                        south = sep[1];
                        break;
                    case "east":
                        east = sep[1];
                        break;
                    case "west":
                        west = sep[1];
                        break;
                }
            }

            cities[cityBreakDown[0]] = new City(cityBreakDown[0], north, east, south, west);       // Adds to Dictionary
        }

        public Dictionary<string, City> Cities
        {
            get { return cities; }
            private set { }
        }

        // Used by the Monster class to assign a random city location
        public string randomCity()
        {
            return Cities.ElementAt(rand.Next(0, Cities.Count)).Value.Name;
        }

        // Drops a city
        public void DropCity(string cityName)
        {
            World.cities[cityName].Destroyed = true;
        }

        // Commence total destruction
        public void CommenceDestruction(World world, Monsters monsters)
        {
            while(true)
            {
                Dictionary<string, string> citiesWithMonsters = new Dictionary<string, string>();       // Emulates a 2D array but where the index canbe the city name. Stores City name and lists monsters in the city
                Dictionary<int, Monster> monsterCities = monsters._Monsters;        // List of all the monsters


                foreach(var monsterCity in monsterCities)
                {
                    // If city exists in the dictionary then add it to the key, if not create a new index for it
                    if(citiesWithMonsters.ContainsKey(monsterCity.Value.Location))
                    {
                        citiesWithMonsters[monsterCity.Value.Location] += "," + monsterCity.Value.Name.ToString();      // Seperate monsters by comma
                    } else
                    {
                        citiesWithMonsters[monsterCity.Value.Location] = "" + monsterCity.Value.Name.ToString();                        
                    }
                }

                // Loops through each dict entry to check if there are more than 1 monster in the city, if so kill all monsters and the city
                foreach(var city in citiesWithMonsters)
                {
                    //Console.WriteLine(city.Value);
                    if(city.Value.Split(',').Length > 1)
                    {
                        int[] monsterList = Array.ConvertAll(city.Value.Split(','), int.Parse);     // Converts the names of the monsters in the curr city to ints
                        string cityName = city.Key;     // gets the city name
                        Console.WriteLine(monsters.Fight(monsterList, cityName, world));        // Kills all the monsters involved and destroys the city
                    }
                }

                // If more than 1 monster is alive and moves are less than 10,000
                if(monsters.Moves < 10000 && monsters._Monsters.Count() > 1)
                {
                    foreach(var monster in monsters._Monsters)
                    {
                        monster.Value.Move(world);
                    }

                    monsters.Moves++;
                } else
                {
                    break;      // End the moves as move limit has been reached or only 1 or less monsters survives
                }
            }
            Console.WriteLine();
            Console.WriteLine("This petty world has been destroyed. It's what they deserved...");

            // Write out the new world txt file
            StreamWriter worldFile = new StreamWriter(System.AppDomain.CurrentDomain.BaseDirectory + "\\extFiles\\Destroyed_World.txt");
            foreach(var city in cities)
            {
                // Write to file "Destroyed_World.txt" if the city is still standing
                if(!city.Value.Destroyed)
                {
                    string cityName = city.Value.Name;
                    string north = city.Value.North;
                    string south = city.Value.South;
                    string east = city.Value.East;
                    string west = city.Value.West;
                    string entry = cityName;

                    if (north.Length > 0)       // If the city north exists
                    {
                        if(!cities[north].Destroyed)    // Only add if the city north is still standing
                        {
                            entry += " north=" + north;
                        }                        
                    }
                    if (south.Length > 0)
                    {
                        if (!cities[south].Destroyed)
                        {
                            entry += " south=" + south;
                        }
                    }
                    if (east.Length > 0)
                    {
                        if (!cities[east].Destroyed)
                        {
                            entry += " east=" + east;
                        }
                    }
                    if (west.Length > 0)
                    {
                        if (!cities[west].Destroyed)
                        {
                            entry += " west=" + west;
                        }
                    }
                    worldFile.WriteLine(entry);
                }
            }
            worldFile.Close();
            Console.WriteLine();
            Console.WriteLine("The Destroyed World can be viewed here: " + System.AppDomain.CurrentDomain.BaseDirectory + "extFiles\\Destroyed_World.txt");
            Console.WriteLine();
            Console.Write("Press [Enter] to open the text file or [Esc] to quit");
            ConsoleKeyInfo key;
            do
            {
                key = Console.ReadKey(true);

                if(key.Key == ConsoleKey.Enter)
                {
                    System.Diagnostics.Process.Start("notepad.exe", System.AppDomain.CurrentDomain.BaseDirectory + "extFiles\\Destroyed_World.txt");
                    Environment.Exit(0);
                }
            } while (key.Key != ConsoleKey.Escape);
            Environment.Exit(0);
        }
    }

    class City
    {
        private string north;
        private string east;
        private string south;
        private string west;
        private string name;

        private bool destroyed = false;     // If city is destroyed or not

        public City(string _name, string _north = "", string _east = "", string _south = "", string _west = "")
        {
            this.north = _north;
            this.east = _east;
            this.south = _south;
            this.west = _west;
            this.name = _name;
        }

        public bool Destroyed
        {
            get { return destroyed; }
            set { this.destroyed = value; }
        }

        public string North
        {
            get { return this.north; }
            set { this.north = value; }
        }

        public string East
        {
            get { return this.east; }
            set { this.east = value; }
        }

        public string South
        {
            get { return this.south; }
            set { this.south = value; }
        }

        public string West
        {
            get { return this.west; }
            set { this.west = value; }
        }

        public string Name
        {
            get { return this.name; }
            set { this.name = value; }
        }
    }
}
