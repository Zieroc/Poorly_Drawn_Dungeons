using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PoorlyDrawnEngine.Graphics;
using PoorlyDrawnEngine.GameEntities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PoorlyDrawnDungeons.BattleSystem;
using PoorlyDrawnDungeons.Managers;

namespace PoorlyDrawnDungeons.Entities
{
    public class Character : Entity
    {
        #region Variables

        protected string name;
        protected float speed;
        protected int transperancy;

        //Stats
        protected int maxHealth;
        protected int currentHealth;
        protected int maxMP;
        protected int currentMP;

        protected int defense;         //Affects how much damage a character takes
        protected int strength;        //Affects how much damage a character deals with a physical attack
        protected int intelligence;     //Affects how much damage a character deals with a magical attack
        protected int dexterity;       //Affects how likely a character is to be hit by an attack and how likely they are to go first in combat
        protected int stamina;         //Affects how much a character can do in a single turn

        //Items and Weaponry
        protected Move weapon;
        protected List<MagicMove> magic;
        protected List<Move> items;

        protected Texture2D mapImage; //The image used to represent this character on the map

        #endregion

        #region Constructor

        public Character(Sprite[] sprite, Vector2 position, Move weapon, List<MagicMove> magic)
        {
            transperancy = 255;
            this.sprite = sprite;
            spriteNum = 0;

            this.position = position;
            this.weapon = weapon;
            this.magic = magic;

            map = LevelManager.GetInstance().DungeonMap;
        }

        #endregion

        #region Properties

        public String Name
        {
            get { return name; }
        }

        public int MaxHealth
        {
            get { return maxHealth; }
            set { maxHealth = value; }
        }

        public int CurrentHealth
        {
            get { return currentHealth; }
            set { currentHealth = value; }
        }

        public int MaxMP
        {
            get { return maxMP; }
            set { maxMP = value; }
        }

        public int CurrentMP
        {
            get { return currentMP; }
            set { currentMP = value; }
        }

        public int Defense
        {
            get { return defense; }
            set { defense = value; }
        }

        public int Strength
        {
            get { return strength; }
            set { strength = value; }
        }

        public int Dexterity
        {
            get { return dexterity; }
            set { dexterity = value; }
        }

        public int Intelligence
        {
            get { return intelligence; }
            set { intelligence = value; }
        }

        public int Stamina
        {
            get { return stamina; }
            set { stamina = value; }
        }

        public Move Weapon
        {
            get { return weapon; }
            set { weapon = value; }
        }

        public List<Move> Items
        {
            get { return items; }
        }

        public List<MagicMove> Magic
        {
            get { return magic; }
        }

        public Texture2D MapImage
        {
            set { mapImage = value; }
        }

        #endregion

        #region Stat Methods

        public void ModDefence(int amount)
        {
            defense += amount;
        }

        public void ModHealth(int amount)
        {
            currentHealth += amount;
            if (currentHealth > maxHealth)
            {
                currentHealth = maxHealth;
            }
        }

        public void FillHealth()
        {
            currentHealth = maxHealth;
        }

        public void UpgradeHealth(int amount)
        {
            maxHealth += amount;
            currentHealth = maxHealth;
        }

        public void ModMP(int amount)
        {
            currentMP += amount;
            if (currentMP > maxMP)
            {
                currentMP = maxMP;
            }
        }

        public void FillMP()
        {
            currentMP = maxMP;
        }

        public void UpgradeMP(int amount)
        {
            maxMP += amount;
            currentMP = maxMP;
        }

        public void ModStrength(int amount)
        {
            strength += amount;
        }

        public void ModDexterity(int amount)
        {
            dexterity += amount;
        }

        public void ModIntelligence(int amount)
        {
            intelligence += amount;
        }

        public void ModStamina(int amount)
        {
            stamina += amount;
        }

        #endregion

        #region Kill, Revive

        public override void Kill()
        {
            if (transperancy <= 0)
            {
                base.Kill();
            }
        }

        public override void Revive()
        {
            currentHealth = MaxHealth;
            currentMP = MaxMP;

            base.Revive();
        }
        #endregion

        #region Update

        public override void Update(GameTime gameTime)
        {
            if (currentHealth <= 0)
            {
                if (transperancy > 150)
                {
                    transperancy--;
                }
                else
                {
                    transperancy -= 5;
                }

                if (transperancy <= 0)
                {
                    Kill();
                }
            }

            base.Update(gameTime);
        }

        #endregion

        #region Draw

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (alive)
            {
                Color colour = new Color(Color.White, transperancy);
                sprite[spriteNum].Draw(spriteBatch, Camera.LocationToScreen(position), depth, colour);
            }
        }

        public void MapDraw(SpriteBatch spriteBatch)
        {
            if (alive)
            {
                spriteBatch.Draw(mapImage, Camera.LocationToScreen(position), mapImage.Bounds, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, depth);
            }
        }

        #endregion

        #region List Methods

        public void AddItem(Move item)
        {
            items.Add(item);
        }

        public void RemoveItem(int index)
        {
            items.RemoveAt(index);
        }

        public void AddMagic(MagicMove spell)
        {
            items.Add(spell);
        }

        #endregion
    }
}
