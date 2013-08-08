using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PoorlyDrawnEngine.Graphics;

namespace PoorlyDrawnDungeons.BattleSystem
{
    public class DamageNumber
    {
        #region Variables

        private Vector2 position;
        private string text;
        private bool active;
        private float timer;
        private float interval;
        private SpriteFont font;
        private Color colour;

        #endregion

        #region Properties

        public bool Active
        {
            get { return active; }
        }

        #endregion

        #region Constructor

        public DamageNumber(Vector2 _position, int number, SpriteFont font)
        {
            this.position = _position;
            this.text = number.ToString(); ;
            active = true;
            interval = 500f;
            timer = 0f;
            this.font = font;
            if (number > 0)
            {
                colour = Color.Green;
            }
            else
            {
                colour = Color.Red;
            }
        }

        #endregion

        #region Update

        public void Update(GameTime gameTime)
        {
            if (timer < interval)
            {
                position.Y--;
                timer += gameTime.ElapsedGameTime.Milliseconds;
            }
            else
            {
                active = false;
            }
        }

        #endregion

        #region Draw

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(font, text, Camera.LocationToScreen(position), colour, 0f, Vector2.Zero, 0.5f, SpriteEffects.None, 0);
        }

        #endregion
    }
}
