using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PoorlyDrawnEngine.Graphics;
using Microsoft.Xna.Framework;
using PoorlyDrawnDungeons.BattleSystem;
using PoorlyDrawnEngine.StateManagement;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace PoorlyDrawnDungeons.Entities
{
    public class Player : Character
    {
        #region Variables

        private int level; //What level is the player
        private int xp;    //How much XP the player has
        private int xpReq; //How much XP is need for the next level

        #endregion

        #region Constructor

        public Player(Sprite[] sprite, Vector2 position, Move weapon, List<MagicMove> magic)
            : base(sprite, position, weapon, magic)
        {
            alive = true;
            level = 1;
            xp = 0;
            xpReq = 100;

            maxHealth = 30;
            currentHealth = 30;
            maxMP = 12;
            currentMP = 12;

            defense = 8; //Default defense without armour
            strength = 8;
            intelligence = 8;
            dexterity = 8;
            stamina = 6;

            speed = 250;

            items = new List<Move>();
        }

        public Player(Texture2D mapImage, Sprite[] sprite, Vector2 position, Move weapon, List<MagicMove> magic)
            : this(sprite, position, weapon, magic)
        {
            this.mapImage = mapImage;
        }

        #endregion

        #region Properties

        public int Level
        {
            get { return level; }
        }

        public int XP
        {
            get { return xp; }
        }

        public int XPReq
        {
            get { return xpReq; }
        }

        #endregion

        #region Collisions

        public override void CalcBounds()
        {
            bounds = new Rectangle((int)position.X, (int)position.Y, mapImage.Width, mapImage.Height);
        }

        #endregion

        #region HandleInput

        public void HandleInput(InputHandler input)
        {
            if (input.KeyDown(Keys.W) || input.KeyDown(Keys.Up))
            {
                velocity.Y = -speed;
            }
            else if (input.KeyDown(Keys.S) || input.KeyDown(Keys.Down))
            {
                velocity.Y = speed;
            }
            else
            {
                velocity.Y = 0;
            }
            if (input.KeyDown(Keys.A) || input.KeyDown(Keys.Left))
            {
                velocity.X = -speed;
            }
            else if (input.KeyDown(Keys.D) || input.KeyDown(Keys.Right))
            {
                velocity.X = speed;
            }
            else
            {
                velocity.X = 0;
            }
        }

        #endregion

        #region Update

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            RepositionCamera();
        }

        #endregion

        #region XP Methods

        public void GainXP(int amount)
        {
            xp += amount;
            if (xp >= xpReq)
            {
                LevelUp();
            }
        }

        public void LevelUp()
        {
            level++;
            xpReq *= 2; //Actual formula to follow
        }

        #endregion

        #region Private Methods

        private void RepositionCamera()
        {
            Vector2 cameraPosition = new Vector2(PositionCenter.X - Camera.ViewPortWidth / 2, PositionCenter.Y - Camera.ViewPortHeight / 2);

            Camera.Position = cameraPosition;
        }

        #endregion
    }
}
