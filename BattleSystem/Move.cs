using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PoorlyDrawnDungeons.SaveData;

namespace PoorlyDrawnDungeons.BattleSystem
{
    public enum Target
    {
        Self,
        Enemy
    }

    public enum AffectedStat
    {
        Health,
        MP,
        Strength,
        Intelligence,
        Dexterity,
        Stamina,
        Defense
    }

    public class Move
    {
        #region Variables

        protected string name;
        protected int staminaCost;
        protected Target target;
        protected AffectedStat stat;
        protected int amount; //Positive numbers for buffs, negative numbers for damage. Amount is average without any modifiers. 1/2 value down and up for range

        #endregion

        #region Constructor

        public Move() { }

        public Move(string name, int staminaCost, Target target, AffectedStat stat, int amount)
        {
            this.name = name;
            this.staminaCost = staminaCost;
            this.target = target;
            this.stat = stat;
            this.amount = amount;
        }

        public Move(MoveData data)
        {
            this.name = data.name;
            this.staminaCost = data.staminaCost;
            this.amount = data.amount;
            if (data.target == "Self")
            {
                this.target = Target.Self;
            }
            else
            {
                this.target = Target.Enemy;
            }
            switch(data.stat)
            {
                case "Health":
                    this.stat = AffectedStat.Health;
                    break;
                case "MP":
                    this.stat = AffectedStat.MP;
                    break;
                case "Strength":
                    this.stat = AffectedStat.Strength;
                    break;
                case "Intelligence":
                    this.stat = AffectedStat.Intelligence;
                    break;
                case "Dexterity":
                    this.stat = AffectedStat.Dexterity;
                    break;
                case "Stamina":
                    this.stat = AffectedStat.Stamina;
                    break;
                case "Defense":
                    this.stat = AffectedStat.Defense;
                    break;
            }
        }

        #endregion

        #region Properties

        public string Name
        {
            get { return name; }
        }

        public int StaminaCost
        {
            get { return staminaCost; }
        }

        public Target Target
        {
            get { return target; }
        }

        public AffectedStat Stat
        {
            get { return stat; }
        }

        public int Amount
        {
            get { return amount; }
        }

        #endregion
    }
}
