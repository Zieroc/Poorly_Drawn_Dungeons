using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PoorlyDrawnDungeons.SaveData
{
    public struct EnemyData
    {
        public string name;
        public string type;
        public string weapon;
        public string armour;
        public int health;
        public int mp;
        public int strength;
        public int intelligence;
        public int dexterity;
        public int stamina;
        public int defense;
        public int xp;
        public List<string> magic;
        public List<string> items;
    }
}
