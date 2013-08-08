using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PoorlyDrawnEngine.Graphics;
using Microsoft.Xna.Framework;
using System.IO;
using System.Xml.Serialization;
using PoorlyDrawnDungeons.BattleSystem;
using PoorlyDrawnDungeons.SaveData;

namespace PoorlyDrawnDungeons.Entities
{
    public class Enemy : Character
    {
        #region Variables

        private int xpValue;

        #endregion

        #region Constructor

        public Enemy(EnemyData data, Move weapon, List<Move> items, List<MagicMove> magic, Sprite[] sprite)
            :base(sprite, new Vector2(Camera.ViewPortWidth - 100 - sprite[0].Width, 100), weapon, magic)
        {
            alive = true;
            this.name = data.name;
            //this.armour = armour;
            this.maxHealth = data.health;
            this.currentHealth = data.health;
            this.maxMP = data.mp;
            this.currentMP = data.mp;
            this.strength = data.strength;
            this.intelligence = data.intelligence;
            this.stamina = data.stamina;
            this.dexterity = data.dexterity;
            this.defense = data.defense;
            this.xpValue = data.xp;
            this.items = items;
        }

        #endregion

        #region Properties

        public int XPValue
        {
            get { return xpValue; }
        }

        #endregion
    }
}
