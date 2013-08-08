using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PoorlyDrawnEngine.Interface;
using Microsoft.Xna.Framework;
using PoorlyDrawnEngine.Graphics;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using PoorlyDrawnDungeons.Screens;
using PoorlyDrawnEngine.StateManagement;
using Microsoft.Xna.Framework.Input;
using PoorlyDrawnDungeons.Managers;

namespace PoorlyDrawnDungeons.Menus
{
    public class ItemMenu : Menu
    {
        #region Variables

        private BattleScreen battle; //Reference to the BattleScreen this menu is a part of
        private Vector2 backgroundPos;

        //Rectangles for mouse menu selection
        private List<string> itemNames;
        private Rectangle[] rectangles = new Rectangle[4];
        private int topIndex;
        private int numItems; //Number of items available
        private int numShown; //Number of items on screen
        private Texture2D arrow;
        private Texture2D backArrow;
        private List<Texture2D> itemTextures = new List<Texture2D>();
        private Texture2D button;
        private Rectangle[] arrowRectangles = new Rectangle[3];
        private bool drawTopArrow;
        private bool drawBottomArrow;

        #endregion

        #region Constructor

        public ItemMenu(BattleScreen battle)
            :base("")
        {
            popUp = true;
            itemNames = new List<string>();
            topIndex = 0;
            numItems = 0;
            for (int i = 0; i < battle.Player.Items.Count; i++)
            {
                itemNames.Add(battle.Player.Items[i].Name);
                itemTextures.Add(TextureManager.GetInstance().ItemTextures[battle.Player.Items[i].Name]);
                numItems++;
            }

            for (int i = 0; i < numItems; i++)
            {
                MenuItem menuItem = new MenuItem("", false);
                MenuItems.Add(menuItem);
                menuItem.Selected += ItemSelected;
            }

            numShown = Math.Min(numItems - topIndex, 4);
            this.battle = battle;


            arrow = TextureManager.GetInstance().Arrow;
            backArrow = TextureManager.GetInstance().BackArrow;
            button = TextureManager.GetInstance().BattleMenuButton;
        }

        #endregion

        #region HandleInput

        protected override void OnCancel(object sender, EventArgs e)
        {
            Exit();
            base.OnCancel(sender, e);
        }

        private void ItemSelected(object sender, EventArgs e)
        {
            int stamina = battle.PStaUsed;
            battle.UseMove(battle.Player.Items[selectedIndex], 3);
            //If move worked close menu otherwise allow a new move to be chosen
            if (stamina < battle.PStaUsed)
            {
                Exit();
            }
        }

        public override void HandleInput(InputHandler input)
        {
            if (hasFocus)
            {
                if (input.MousePressed(1))
                {
                    Point mouseLoc = input.MouseLoc();
                    if (rectangles[0].Contains(mouseLoc) && numShown > 0)
                    {
                        OnSelectItem(0);
                    }
                    else if (rectangles[1].Contains(mouseLoc) && numShown > 1)
                    {
                        OnSelectItem(1);
                    }
                    else if (rectangles[2].Contains(mouseLoc) && numShown > 2)
                    {
                        OnSelectItem(2);
                    }
                    else if (rectangles[3].Contains(mouseLoc) && numShown > 3)
                    {
                        OnSelectItem(3);
                    }
                    else if (arrowRectangles[0].Contains(mouseLoc) && drawTopArrow)
                    {
                        topIndex -= 2;
                        numShown = Math.Min(numItems - topIndex, 4);
                    }
                    else if (arrowRectangles[1].Contains(mouseLoc) && drawBottomArrow)
                    {
                        topIndex += 2;
                        numShown = Math.Min(numItems - topIndex, 4);
                    }
                    else if (arrowRectangles[2].Contains(mouseLoc))
                    {
                        OnCancel(this, new EventArgs());
                    }
                }

                if (input.MouseMoved())
                {
                    Point mouseLoc = input.MouseLoc();
                    if (rectangles[0].Contains(mouseLoc) && numShown > 0)
                    {
                        selectedIndex = 0 + topIndex;
                    }
                    else if (rectangles[1].Contains(mouseLoc) && numShown > 1)
                    {
                        selectedIndex = 1 + topIndex;
                    }
                    else if (rectangles[2].Contains(mouseLoc) && numShown > 2)
                    {
                        selectedIndex = 2 + topIndex;
                    }
                    else if (rectangles[3].Contains(mouseLoc) && numShown > 3)
                    {
                        selectedIndex = 3 + topIndex;
                    }
                }
            }
        }

        #endregion

        #region Load

        public override void LoadContent(ContentManager content)
        {
            base.LoadContent(content);
        }

        #endregion

        #region Initialize

        public override void Initialize()
        {
            InitalRectangles();
            base.Initialize();
        }

        private void InitalRectangles()
        {
            Texture2D background = TextureManager.GetInstance().UIMenu;
            backgroundPos = new Vector2(Camera.ViewPortWidth / 2 - background.Width / 2, Camera.ViewPortHeight - background.Height);
            rectangles[0] = new Rectangle((int)backgroundPos.X + 38, (int)backgroundPos.Y + 10, button.Width, button.Height);
            if (numShown > 1)
            {
                rectangles[1] = new Rectangle((int)backgroundPos.X + 38, (int)backgroundPos.Y + background.Height - button.Height - 10, button.Width, button.Height);
            }
            if (numShown > 2)
            {
                rectangles[2] = new Rectangle((int)backgroundPos.X + background.Width - 38 - button.Width, (int)backgroundPos.Y + 10, button.Width, button.Height);
            }
            if (numShown > 3)
            {
                rectangles[3] = new Rectangle((int)backgroundPos.X + background.Width - 38 - button.Width, (int)backgroundPos.Y + background.Height - button.Height - 10, button.Width, button.Height);
            }

            arrowRectangles[0] = new Rectangle((int)backgroundPos.X + background.Width - arrow.Width - 10, (int)backgroundPos.Y + 15, arrow.Width, arrow.Height);
            arrowRectangles[1] = new Rectangle((int)backgroundPos.X + background.Width - arrow.Width - 10, (int)backgroundPos.Y + background.Height - arrow.Height - 10, arrow.Width, arrow.Height);
            arrowRectangles[2] = new Rectangle((int)backgroundPos.X + 10, (int)backgroundPos.Y + 15, backArrow.Width - 5, backArrow.Height);
        }

        #endregion

        #region Update and Draw

        protected override void UpdateMenuItemPositions()
        {
            //Remove other menu Items from the screen
            for (int i = 0; i < menuItems.Count; i++)
            {
                if (selectedIndex > numShown + topIndex)
                {
                    selectedIndex = numShown + topIndex;
                }
                if (selectedIndex < topIndex)
                {
                    selectedIndex = topIndex;
                }
                menuItems[i].Position = new Vector2(-500, -500);
            }

            //May have no items so need to check to see if anything should be shown
            if (numShown > 0)
            {
                menuItems[topIndex].Position = new Vector2(rectangles[0].X, rectangles[0].Y);
            }
            if (numShown > 1)
            {
                menuItems[topIndex + 1].Position = new Vector2(rectangles[1].X, rectangles[1].Y);
            }
            if (numShown > 2)
            {
                menuItems[topIndex + 2].Position = new Vector2(rectangles[2].X, rectangles[2].Y);
            }
            if (numShown > 3)
            {
                menuItems[topIndex + 3].Position = new Vector2(rectangles[3].X, rectangles[3].Y);
            }
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            UpdateMenuItemPositions();

            if (topIndex > 0)
            {
                drawTopArrow = true;
            }
            else
            {
                drawTopArrow = false;
            }
            if (topIndex + numShown < numItems)
            {
                drawBottomArrow = true;
            }
            else
            {
                drawBottomArrow = false;
            }

            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
        }

        public override void Draw()
        {
            screenManager.SpriteBatch.Begin();

            Point mouseLoc = new Point(Mouse.GetState().X, Mouse.GetState().Y);
            Color arrowColour = Color.White;
            if (arrowRectangles[2].Contains(mouseLoc))
            {
                arrowColour = Color.Yellow;
            }
            screenManager.SpriteBatch.Draw(backArrow, arrowRectangles[2], backArrow.Bounds, arrowColour, 0f, Vector2.Zero, SpriteEffects.None, 1f);
            if (drawTopArrow)
            {
                if (arrowRectangles[0].Contains(mouseLoc))
                {
                    arrowColour = Color.Yellow;
                }
                else
                {
                    arrowColour = Color.White;
                }
                screenManager.SpriteBatch.Draw(arrow, arrowRectangles[0], arrow.Bounds, arrowColour, 0f, Vector2.Zero, SpriteEffects.None, 1f);
            }
            if (drawBottomArrow)
            {
                if (arrowRectangles[1].Contains(mouseLoc))
                {
                    arrowColour = Color.Yellow;
                }
                else
                {
                    arrowColour = Color.White;
                }
                screenManager.SpriteBatch.Draw(arrow, arrowRectangles[1], arrow.Bounds, arrowColour, 0f, Vector2.Zero, SpriteEffects.FlipVertically, 1f);
            }

            for (int i = 0; i < numShown; i++)
            {
                if (rectangles[i].Contains(mouseLoc))
                {
                    screenManager.SpriteBatch.Draw(button, rectangles[i], Color.DarkGray);
                }
                else
                {
                    screenManager.SpriteBatch.Draw(button, rectangles[i], Color.LightGray);
                }
            }

            //Draw Button Images
            for (int i = 0; i < numShown; i++)
            {
                screenManager.SpriteBatch.Draw(itemTextures[topIndex + i], new Vector2(rectangles[i].X + button.Width / 2 - itemTextures[topIndex + i].Width / 2, rectangles[i].Y + button.Height / 2 - itemTextures[topIndex + i].Height / 2), Color.White);
            }

            ScreenManager.SpriteBatch.End();

            base.Draw();
        }

        #endregion
    }
}