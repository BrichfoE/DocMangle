using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrMangle
{
    public static class StaticUtility
    {

        public static void TalkPause(string message)
        {
            Console.WriteLine(message);
            Console.ReadKey(true);
        }

        public static int CheckInput(int low, int high)
        {
            string strInput = "";
            int input = 0;
            bool halt = true;
            bool formatError = false;
            while (halt)
            {
                formatError = false;
                strInput = Console.ReadLine();
                if (strInput == null)
                {
                    Console.WriteLine("Please choose a number " + low + "-" + high);
                }
                else
                {
                    try
                    {
                        input = Convert.ToInt32(strInput);
                    }
                    catch (System.FormatException)
                    {
                        Console.WriteLine("Please choose a number " + low + "-" + high);
                        formatError = true;
                    }
                    if (input < low || input > high && formatError == false)
                    {
                        Console.WriteLine("Please choose a number " + low + "-" + high);
                    }
                    else if (formatError == false)
                    {
                        halt = false;
                    }
                }


            }

            return input;
        }

        //public static float RNG(int min, int max, bool percent)
        //{
        //    float number;
        //    Random r = new Random();
        //    if (percent)
        //    {
        //        number = (r.Next(min, max) / max);
        //    }
        //    else
        //    {
        //        number = r.Next(min, max);
        //    }

        //    return number;
        //}

        //public static int RNG(int min, int max)
        //{
        //    int number;
        //    Random r = new Random();
        //    number = r.Next(min, max);
        //    return number;
        //}

    }

    public static class StaticReference
    {
        public static string[] typeList = new string[6]
            {
                  "Head"
                , "Torso"
                , "Left Arm"
                , "Right Arm"
                , "Left Leg"
                , "Right Leg"
            };
        public static string[] structureList = new string[5]
            {
                  "Magical"
                , "Animal"
                , "Human"
                , "Mechanical"
                , "Rock"
            };
        public static string[] rarityList = new string[6]
            {
                 "Unicorn"
               , "Mythic"
               , "Legendary"
               , "Epic"
               , "Rare"
               , "Common"
            };
        public static string[] statList = new string[4]
            {
                  "Alacrity"
                , "Strength"
                , "Endurance"
                , "Technique"
            };

        public static string[] adjectives = new string[10] { "Cool", "Nice", "Mad", "Helpful", "Thin", "Dirty", "Slick", "Ugly", "Super", "Octogenarian" };
        public static string[] names = new string[10] { "Luke", "Matilda", "Martha", "Hannah", "Pete", "Harry", "Rick", "Veronica", "Susan", "Maynard" };
    }
}

