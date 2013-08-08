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
    public class BattleMenu : Menu
    {
        #region Variables

        private Texture2D circRecButton;
        private Texture2D sandTimer;
        private Texture2D battleMenuButton;
        private Texture2D attackTexture;
        //private Texture2D fleeTexture;
        private Texture2D magicTexture;
        private Texture2D itemTexture;

        private BattleScreen battle; //Reference to the BattleScreen this menu is a part of
        private Vector2 backgroundPos;

        //Rectangles for mouse menu selection
        private Rectangle attackRec;
        private Rectangle fleeRec;
        private Rectangle magicRec;
        private Rectangle itemRec;
        private Rectangle endTurnRec;

        #endregion

        #region Constructor

        public BattleMenu(BattleScreen battle)
            :base("")
        {
            popUp = true;

            MenuItem attackMenuItem = new MenuItem("", true);
            MenuItem fleeMenuItem = new MenuItem("FLEE", false);
            MenuItem magicMenuItem = new MenuItem("", false);
            MenuItem itemsMenuItem = new MenuItem("", false);

            //Set up menu events.
            attackMenuItem.Selected += AttackItemSelected;
            fleeMenuItem.Selected += FleeItemSelected;
            magicMenuItem.Selected += MagicItemSelected;
            itemsMenuItem.Selected += ItemsItemSelected;

            // Add entries to the menu.
            MenuItems.Add(attackMenuItem);
            MenuItems.Add(fleeMenuItem);
            MenuItems.Add(magicMenuItem);
            MenuItems.Add(itemsMenuItem);

            this.battle = battle;

            circRecButton = TextureManager.GetInstance().CircRecButton;
            sandTimer = TextureManager.GetInstance().SandTimer;
            battleMenuButton = TextureManager.GetInstance().BattleMenuButton;
            attackTexture = TextureManager.GetInstance().BattleMenuImages[0];
            itemTexture = TextureManager.GetInstance().BattleMenuImages[2];
            magicTexture = TextureManager.GetInstance().BattleMenuImages[3];
        }

        #endregion

        #region HandleInput

        private void AttackItemSelected(object sender, EventArgs e)
        {
            battle.UseMove(battle.Player.Weapon, 1);
        }

        private void FleeItemSelected(object sender, EventArgs e)
        {
            //Do Flee
        }

        private void MagicItemSelected(object sender, EventArgs e)
        {
            ScreenManager.AddScreen(new MagicMenu(battle));
        }

        private void ItemsItemSelected(object sender, EventArgs e)
        {
            ScreenManager.AddScreen(new ItemMenu(battle));
        }

        public override void HandleInput(InputHandler input)
        {
            if (hasFocus && battle.Turn == 1)
            {
                if (input.MousePressed(1))
                {
                    Point mouseLoc = input.MouseLoc();
                    if (attackRec.Contains(mouseLoc))
                    {
                        OnSelectItem(0);
                    }
                    else if (fleeRec.Contains(mouseLoc))
                    {
                        OnSelectItem(1);
                    }
                    else if (magicRec.Contains(mouseLoc))
                    {
                        OnSelectItem(2);
                    }
                    else if (itemRec.Contains(mouseLoc))
                    {
                        OnSelectItem(3);
                    }
                    else if (endTurnRec.Contains(mouseLoc))
                    {
                        battle.EndTurn();
                    }
                }

                if (input.MouseMoved())
                {
                    Point mouseLoc = input.MouseLoc();
                    if (attackRec.Contains(mouseLoc))
                    {
                        selectedIndex = 0;
                    }
                    else if (fleeRec.Contains(mouseLoc))
                    {
                        selectedIndex = 1;
                    }
                    else if (magicRec.Contains(mouseLoc))
                    {
                        selectedIndex = 2;
                    }
                    else if (itemRec.Contains(mouseLoc))
                    {
                        selectedIndex = 3;
                    }
                }

                base.HandleInput(input);
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
            Texture2D background = TextureManager.GetInstance().UIMenu;
            backgroundPos = new Vector2(Camera.ViewPortWidth / 2 - background.Width / 2, Camera.ViewPortHeight - background.Height);
            attackRec = new Rectangle((int)backgroundPos.X + 38, (int)backgroundPos.Y + 10, battleMenuButton.Width, battleMenuButton.Height);
            fleeRec = new Rectangle((int)backgroundPos.X + 38, (int)backgroundPos.Y + background.Height - battleMenuButton.Height - 10, battleMenuButton.Width, battleMenuButton.Height);
            magicRec = new Rectangle((int)backgroundPos.X + background.Width - 38 - battleMenuButton.Width, (int)backgroundPos.Y + 10, battleMenuButton.Width, battleMenuButton.Height);
            itemRec = new Rectangle((int)backgroundPos.X + background.Width - 38 - battleMenuButton.Width, (int)backgroundPos.Y + background.Height - battleMenuButton.Height - 10, battleMenuButton.Width, battleMenuButton.Height);
            endTurnRec = new Rectangle(1040, (int)backgroundPos.Y + background.Height/2 - circRecButton.Height/2, circRecButton.Width, circRecButton.Height);

            base.Initialize();
        }
        #endregion

        #region Update and Draw

        protected override void UpdateMenuItemPositions()
        {
            menuItems[0].Position = new Vector2(attackRec.X, attackRec.Y);
            menuItems[1].Position = new Vector2(fleeRec.X + battleMenuButton.Width / 2 - screenManager.SpriteFont.MeasureString("Flee").X / 2, fleeRec.Y + battleMenuButton.Height / 2 - screenManager.SpriteFont.MeasureString("Flee").Y / 2);
            menuItems[2].Position = new Vector2(magicRec.X, magicRec.Y);
            menuItems[3].Position = new Vector2(itemRec.X, itemRec.Y);
        }

        public override void Draw()
        {
            if (hasFocus || (screenManager.Screens[screenManager.Screens.Count - 1].GetType() == typeof(MessageBox) && screenManager.Screens[screenManager.Screens.Count - 2].GetType() == typeof(BattleMenu)))
            {
                screenManager.SpriteBatch.Begin();
                Point mouseLoc = new Point(Mouse.GetState().X, Mouse.GetState().Y);
                if (endTurnRec.Contains(mouseLoc) && battle.Turn == 1 && hasFocus)
                {
                    screenManager.SpriteBatch.Draw(circRecButton, endTurnRec, Color.DarkGray);
                }
                else
                {
                    screenManager.SpriteBatch.Draw(circRecButton, endTurnRec, Color.LightGray);
                }

                if (attackRec.Contains(mouseLoc) && battle.Turn == 1 && hasFocus)
                {
                    screenManager.SpriteBatch.Draw(battleMenuButton, attackRec, Color.DarkGray);
                }
                else
                {
                    screenManager.SpriteBatch.Draw(battleMenuButton, attackRec, Color.LightGray);
                }

                if (fleeRec.Contains(mouseLoc) && battle.Turn == 1 && hasFocus)
                {
                    screenManager.SpriteBatch.Draw(battleMenuButton, fleeRec, Color.DarkGray);
                }
                else
                {
                    screenManager.SpriteBatch.Draw(battleMenuButton, fleeRec, Color.LightGray);
                }

                if (itemRec.Contains(mouseLoc) && battle.Turn == 1 && hasFocus)
                {
                    screenManager.SpriteBatch.Draw(battleMenuButton, itemRec, Color.DarkGray);
                }
                else
                {
                    screenManager.SpriteBatch.Draw(battleMenuButton, itemRec, Color.LightGray);
                }

                if (magicRec.Contains(mouseLoc) && battle.Turn == 1)
                {
                    screenManager.SpriteBatch.Draw(battleMenuButton, magicRec, Color.DarkGray);
                }
                else
                {
                    screenManager.SpriteBatch.Draw(battleMenuButton, magicRec, Color.LightGray);
                }

                //Draw Button Images
                screenManager.SpriteBatch.Draw(sandTimer, new Vector2(endTurnRec.X + circRecButton.Width / 2 - sandTimer.Width / 2, endTurnRec.Y + circRecButton.Height / 2 - sandTimer.Height / 2), Color.White);
                screenManager.SpriteBatch.Draw(attackTexture, new Vector2(attackRec.X + battleMenuButton.Width / 2 - attackTexture.Width / 2, attackRec.Y + battleMenuButton.Height / 2 - attackTexture.Height / 2), Color.White);
                screenManager.SpriteBatch.Draw(itemTexture, new Vector2(itemRec.X + battleMenuButton.Width / 2 - itemTexture.Width / 2, itemRec.Y + battleMenuButton.Height / 2 - itemTexture.Height / 2), Color.White);
                screenManager.SpriteBatch.Draw(magicTexture, new Vector2(magicRec.X + battleMenuButton.Width / 2 - magicTexture.Width / 2, magicRec.Y + battleMenuButton.Height / 2 - magicTexture.Height / 2), Color.White);

                screenManager.SpriteBatch.End();
                base.Draw();
            }

        }

        #endregion
    }
}
