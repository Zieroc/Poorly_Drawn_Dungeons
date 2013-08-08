using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PoorlyDrawnEngine.StateManagement;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using PoorlyDrawnDungeons.Managers;
using PoorlyDrawnEngine.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace PoorlyDrawnDungeons.Screens
{
    public class MessageBox : GameScreen
    {
        #region Variables

        private Texture2D messageBox;
        private string message;
        private Rectangle closeButton;
        private Vector2 position;
        private Vector2 messagePos;

        #endregion

        #region Constructor

        public MessageBox(string message)
        {
            popUp = true;
            this.message = message;
            messageBox = TextureManager.GetInstance().MessageBox;
            position = new Vector2(Camera.ViewPortWidth / 2 - messageBox.Width / 2, Camera.ViewPortHeight / 3 - messageBox.Height / 2);
            closeButton = new Rectangle(Camera.ViewPortWidth / 2 - TextureManager.GetInstance().CloseButton.Width / 2, (int)position.Y + messageBox.Height - TextureManager.GetInstance().CloseButton.Width - 15, TextureManager.GetInstance().CloseButton.Width, TextureManager.GetInstance().CloseButton.Height);
        }

        #endregion

        #region HandleInput

        public override void HandleInput(InputHandler input)
        {
            if (input.MousePressed(1))
            {
                if (closeButton.Contains(input.MouseLoc()))
                {
                    Exit();
                }
            }
            base.HandleInput(input);
        }

        #endregion

        #region Load

        public override void LoadContent(ContentManager content)
        {
            //Done here so screen manager will have been assigned
            messagePos = new Vector2(position.X + messageBox.Width / 2 - screenManager.SpriteFont.MeasureString(message).X / 2, position.Y + messageBox.Height / 2 - screenManager.SpriteFont.MeasureString(message).Y / 2);
            base.LoadContent(content);
        }

        #endregion

        #region Draw

        public override void Draw()
        {
            screenManager.SpriteBatch.Begin();
            screenManager.SpriteBatch.Draw(messageBox, position, Color.White);
            screenManager.SpriteBatch.DrawString(screenManager.SpriteFont, message, messagePos, Color.Black);
            if (closeButton.Contains(Mouse.GetState().X, Mouse.GetState().Y))
            {
                screenManager.SpriteBatch.Draw(TextureManager.GetInstance().CloseButton, closeButton, Color.Gray);
            }
            else
            {
                screenManager.SpriteBatch.Draw(TextureManager.GetInstance().CloseButton, closeButton, Color.White);
            }
            screenManager.SpriteBatch.End();

            base.Draw();
        }

        #endregion
    }
}
