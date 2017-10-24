using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mwuhahaha
{
    class Program
    {
        static void Main(string[] args)
        {

            ConsoleKey inpKey;
            do
            {
                Console.Clear();
                Console.Write("Welcome, evil overlord! Please choose your world size; Medium [m] or Small [s] or [Esc] to quit being evil: ");
                inpKey = Console.ReadKey().Key;
            } while (inpKey != ConsoleKey.M && inpKey != ConsoleKey.S && inpKey != ConsoleKey.Escape);

            World world = null;

            switch (inpKey)
            {
                case ConsoleKey.M:
                    world = new World("world_map_medium.txt");      // Create new world using the requested txt file
                    break;
                case ConsoleKey.S:
                    world = new World("world_map_small.txt");
                    break;
                case ConsoleKey.Escape:
                    Environment.Exit(0);
                    break;
            }

            /* 
             Custom Input is pretty cool here. Reads the key and prevents output unless it is an integer.
             I had to make a custom string to hold the values that are entered. 
             This is converted into an integer after the user presses enter.
             The user must enter something for the program to cotinue.
             */
            Console.WriteLine();
            Console.Write("Enter how many monsters (N) you want to roam this miserable planet: ");
            ConsoleKeyInfo key;
            string _val = "";       // Trick for custom input
            int number;     // Used for outputting int input & the number of monsters
            bool isValid = true;
            do
            {
                key = Console.ReadKey(true);    // Read input without printing result
                if (int.TryParse(key.KeyChar.ToString(), out number))   // Check if input is an int
                {
                    Console.Write(key.KeyChar.ToString());      // Write input to console
                    _val += key.KeyChar;        // Add key to string of the input
                } else if(key.Key == ConsoleKey.Backspace && _val.Length > 0)   // For removing characters
                {
                    _val = _val.Substring(0, (_val.Length - 1));       // Remove 1 char from string
                    Console.Write("\b \b");     // Remove previous
                }

                if(_val.Length > 0)
                {
                    isValid = true;
                } else
                {
                    isValid = false;
                }
            } while (key.Key != ConsoleKey.Enter || !isValid);

            number = int.Parse(_val);       // Amend number to represent (N)
            Console.WriteLine();
            Console.WriteLine();

            // Create all the monsters
            Monsters monsterArmy = new Monsters(number, world);

            // Run the program
            world.CommenceDestruction(world, monsterArmy);

            Console.ReadLine();
        }
    }
}
