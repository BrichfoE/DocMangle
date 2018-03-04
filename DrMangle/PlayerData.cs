using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrMangle
{
    public class PlayerData
    {

        public string name;
        public int wins;
        public int fights;
        public MonsterData monster;
        public PartData[] bag;
        public PartData[] workshop;
        public int luck;
        //biomatter
        //components
        //rocks
        //ether
        //money

        public PlayerData(string playerName)
        {
            name = playerName;
            wins = 0;
            fights = 0;
            monster = null;
            bag = new PartData[5];
            workshop = new PartData[20];
        }

        public PlayerData(int randInt)
        {
            name = RandomName(randInt);
            wins = 0;
            fights = 0;
            monster = null;
            bag = new PartData[5];
            workshop = new PartData[20];
        }

        public PlayerData(string playerName, int fightsWon, int fightsParticipated, MonsterData currentMonster, PartData[] currentBag, PartData[] currentShop)
        {
            name = playerName;
            wins = fightsWon;
            fights = fightsParticipated;
            monster = currentMonster;
            bag = currentBag;
            workshop = currentShop;
        }



        //Methods
            //add part to inventory
            //remove part from inventory
            //dump bag parts into inventory
            //display inventory
            //scrap inventory item        

            //display monster
            //add part to monster
            //remove part from monster
            //activate monster
            //scrap monster
            //repair mosnter

            //generate random opponent data
            //display record



        private string[] adjectives;
        private string[] names;

        public string RandomName(int input)
        {
            string result = "";
            Random r = new Random();

            int adjInt;
            int namInt;

            if (adjectives == null || names == null)
            {
                adjectives = new string[10] { "Cool", "Nice", "Mad", "Helpful", "Thin", "Dirty", "Slick", "Ugly", "Super", "Octogenarian" };
                names = new string[10] { "Luke", "Matilda", "Martha", "Hannah", "Pete", "Harry", "Rick", "Veronica", "Susan", "Maynard" };
            }

            adjInt = (input * r.Next()) % 10;
            namInt = (input * r.Next()) % 10;

            result = adjectives[adjInt] + " " + names[namInt];

            return result;
        }
    }
}





//public string Name
//{
//    get
//    {
//        return _name;
//    }
//    set
//    {
//        if (String.IsNullOrEmpty(value))
//        {
//            throw new ArgumentException("Name cannot be empty");
//        }

//        if (_name != value && NameChanged != null)
//        {
//            NameChangedEventArgs args = new NameChangedEventArgs();
//            args.ExistingName = _name;
//            args.NewName = value;

//            NameChanged(this, args);
//        }

//        _name = value;

//    }

//}

