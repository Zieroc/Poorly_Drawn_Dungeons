using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PoorlyDrawnDungeons.SaveData;

namespace PoorlyDrawnDungeons.BattleSystem
{
    public class MagicMove : Move
    {
        #region Variables

        private int mpCost;

        #endregion

        #region Constructor

        public MagicMove(string name, int staminaCost, Target target, AffectedStat stat, int amount, int mpCost)
            :base(name, staminaCost, target, stat, amount)
        {
            this.mpCost = mpCost;
        }

        public MagicMove(MagicData data)
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
            switch (data.stat)
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
            this.mpCost = data.mpAmount;
            
        }

        #endregion

        #region Properties

        public int MPCost
        {
            get { return mpCost; }
        }

        #endregion
    }
}
