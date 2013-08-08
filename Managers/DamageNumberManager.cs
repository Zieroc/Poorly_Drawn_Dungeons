using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using PoorlyDrawnDungeons.BattleSystem;
using Microsoft.Xna.Framework;

namespace PoorlyDrawnDungeons.Managers
{
    public class DamageNumberManager
    {
        #region Variables

        //Keep track of how well the player does each wave
        private List<DamageNumber> damageNumbers;
        private SpriteFont font;

        #endregion

        #region Constructor

        public DamageNumberManager(SpriteFont font)
        {
            damageNumbers = new List<DamageNumber>();
            this.font = font;
        }

        #endregion

        #region Update

        public void Update(GameTime gameTime)
        {
            for (int i = damageNumbers.Count - 1; i >= 0; i--)
            {
                if (damageNumbers[i].Active)
                {
                    damageNumbers[i].Update(gameTime);
                }
                else
                {
                    damageNumbers.RemoveAt(i);
                }
            }
        }

        #endregion

        #region Draw

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (DamageNumber item in damageNumbers)
            {
                item.Draw(spriteBatch);
            }
        }

        #endregion

        #region Damage Indicator Methods

        public void AddDamageNumber(int amount, Vector2 position)
        {

            DamageNumber damageNumber = new DamageNumber(position, amount, font);
            damageNumbers.Add(damageNumber);
        }

        public void ClearDamageNumbers()
        {
            damageNumbers.Clear();
        }
        #endregion
    }
}
