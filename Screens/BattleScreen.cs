using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PoorlyDrawnEngine.StateManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using PoorlyDrawnEngine.Graphics;
using Microsoft.Xna.Framework.Graphics;
using PoorlyDrawnDungeons.Entities;
using PoorlyDrawnDungeons.BattleSystem;
using PoorlyDrawnDungeons.Managers;

namespace PoorlyDrawnDungeons.Screens
{
    public class BattleScreen : GameScreen
    {
        #region Variables

        private Player player; //Will be replaced with a player object
        private Enemy enemy;  //Will be replaced with an enemy object
        private DamageNumberManager damNumMan;

        //UI
        private Texture2D sideBox;
        private Texture2D background;
        private SpriteFont sideBarFont;
        private Texture2D healthBar;
        private Texture2D mainBox;

        //Battle Logic
        private int turn; //1 for player, 2 for enemy
        private bool battleOver;

        //Player Bonuses
        private int pStrBonus;
        private int pIntBonus;
        private int pDexBonus;
        private int pStaBonus;
        private int pDefBonus;
        private int pStaUsed; //How much stamina the player has used

        //Enemy Bonuses
        private int eStrBonus;
        private int eIntBonus;
        private int eDexBonus;
        private int eStaBonus;
        private int eDefBonus;
        private int eStaUsed; //How much stamina the enemy has used

        #endregion

        #region Constructor

        public BattleScreen(Player player, Enemy enemy)
        {
            this.player = player;
            this.enemy = enemy;
            turn = 1;
            battleOver = false;

            pStrBonus = 5;
            pIntBonus = 0;
            pDexBonus = 0;
            pStaBonus = 0;
            pDefBonus = 0;

            eStrBonus = 0;
            eIntBonus = 0;
            eDexBonus = 0;
            eStaBonus = 0;
            eDefBonus = 0;

            mainBox = TextureManager.GetInstance().UIMenu;
            sideBox = TextureManager.GetInstance().UIMenu2;
            background = TextureManager.GetInstance().BattleBackground;
            healthBar = TextureManager.GetInstance().HealthBar;
        }

        #endregion

        #region Properties

        public Player Player
        {
            get { return player; }
        }

        public Enemy Enemy
        {
            get { return enemy; }
        }

        public int Turn
        {
            get { return turn; }
            set { turn = value; }
        }

        public int PStaUsed
        {
            get { return pStaUsed; }
        }

        #endregion

        #region Load/Unload

        public override void LoadContent(ContentManager content)
        {
            damNumMan = new DamageNumberManager(screenManager.SpriteFont); //Done here because screenmanager has been assigned
            sideBarFont = content.Load<SpriteFont>("Font/SideBarFont");
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
        }

        #endregion

        #region Update

        public override void Update(GameTime gameTime, bool hasFocus, bool coveredByScreen)
        {
            if (!player.Alive || !enemy.Alive)
            {
                battleOver = true;
            }

            player.Update(gameTime);
            enemy.Update(gameTime);
            damNumMan.Update(gameTime);

            if (turn == 2)
            {
                UseMove(enemy.Weapon, 1);
            }

            base.Update(gameTime, hasFocus, coveredByScreen);

            if (battleOver)
            {
                screenManager.Game.Exit();
            }
        }

        public override void HandleInput(InputHandler input)
        {
            base.HandleInput(input);
        }

        #endregion

        #region Draw

        public override void Draw()
        {
            screenManager.SpriteBatch.Begin();

            //UI
            screenManager.SpriteBatch.Draw(background, Vector2.Zero, new Color(Color.White, 100f));
            screenManager.SpriteBatch.Draw(sideBox, new Vector2(0, Camera.ViewPort.Height - sideBox.Height), Color.White);
            screenManager.SpriteBatch.Draw(mainBox, new Vector2(0 + sideBox.Width, Camera.ViewPort.Height - mainBox.Height), Color.White);
            screenManager.SpriteBatch.Draw(sideBox, new Vector2(Camera.ViewPort.Width -  sideBox.Width, Camera.ViewPort.Height - sideBox.Height), sideBox.Bounds, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.FlipHorizontally, 0f);

            //Characters
            player.Draw(screenManager.SpriteBatch);
            enemy.Draw(screenManager.SpriteBatch);

            //Healthbars
            Color healthBarColour;
            int hBWidth = 0;

            //Player
            if (player.Alive)
            {
                screenManager.SpriteBatch.Draw(healthBar, new Vector2(player.Position.X + 15, player.Position.Y - 5 - healthBar.Height), Color.WhiteSmoke);
                if (player.CurrentHealth > player.MaxHealth / 2)
                {
                    healthBarColour = Color.Green;
                }
                else if (player.CurrentHealth > player.MaxHealth / 4)
                {
                    healthBarColour = Color.Orange;
                }
                else
                {
                    healthBarColour = Color.Red;
                }
                if (player.CurrentHealth > 0)
                {
                    hBWidth = Math.Max(1, (int)Math.Round(healthBar.Width * ((float)player.CurrentHealth / player.MaxHealth)));
                }
                screenManager.SpriteBatch.Draw(healthBar, new Rectangle((int)player.Position.X + 15, (int)player.Position.Y - 5 - healthBar.Height, hBWidth, healthBar.Height), new Rectangle(healthBar.Bounds.X, healthBar.Bounds.Y, hBWidth, healthBar.Height), healthBarColour);
            }

            //Enemy
            if (enemy.Alive)
            {
                screenManager.SpriteBatch.Draw(healthBar, new Vector2(enemy.Position.X, enemy.Position.Y - 5 - healthBar.Height), Color.WhiteSmoke);
                if (enemy.CurrentHealth > enemy.MaxHealth / 2)
                {
                    healthBarColour = Color.Green;
                }
                else if (enemy.CurrentHealth > enemy.MaxHealth / 4)
                {
                    healthBarColour = Color.Orange;
                }
                else
                {
                    healthBarColour = Color.Red;
                }
                hBWidth = 0;
                if (enemy.CurrentHealth > 0)
                {
                    hBWidth = Math.Max(1, (int)Math.Round(healthBar.Width * ((float)enemy.CurrentHealth / enemy.MaxHealth)));
                }
                screenManager.SpriteBatch.Draw(healthBar, new Rectangle((int)enemy.Position.X, (int)enemy.Position.Y - 5 - healthBar.Height, hBWidth, healthBar.Height), new Rectangle(healthBar.Bounds.X, healthBar.Bounds.Y, hBWidth, healthBar.Height), healthBarColour);
            }


            //Damage/Buff Numbers
            damNumMan.Draw(screenManager.SpriteBatch);

            //SidebarInfo
            screenManager.SpriteBatch.DrawString(sideBarFont, "Remaining Stamina: " + (player.Stamina + pStaBonus - pStaUsed), new Vector2(Camera.ViewPort.Width -  sideBox.Width + 10, Camera.ViewPort.Height - sideBox.Height + 10), Color.White);

            screenManager.SpriteBatch.End();
        }

        #endregion

        #region Battle Logic

        //1 - Weapon, 2 - Magic, 3 - Item
        public void UseMove(Move move, int type)
        {
            if (turn == 1)
            {
                if ((player.Stamina + pStaBonus) - pStaUsed - move.StaminaCost >= 0)
                {
                    int amount;
                    if (move.Amount > 0)
                    {
                        amount = move.Amount / 2 + new Random().Next(move.Amount);
                    }
                    else
                    {
                        amount = move.Amount / 2 - new Random().Next(Math.Abs(move.Amount));
                    }

                    if (type == 1)
                    {
                        if (amount > 0)
                        {
                            amount += pStrBonus;
                        }
                        else
                        {
                            amount -= pStrBonus;
                        }
                    }
                    else if (type == 2)
                    {
                        if (amount > 0)
                        {
                            amount += pIntBonus;
                        }
                        else
                        {
                            amount -= pIntBonus;
                        }
                    }

                    if (move.Target == Target.Self)
                    {
                        damNumMan.AddDamageNumber(amount, new Vector2(player.Position.X, player.Position.Y - 10));

                        switch (move.Stat)
                        {
                            case AffectedStat.Health:
                                player.ModHealth(amount);
                                break;
                            case AffectedStat.MP:
                                player.ModMP(amount);
                                break;
                            case AffectedStat.Strength:
                                pStrBonus += amount;
                                break;
                            case AffectedStat.Intelligence:
                                pIntBonus += amount;
                                break;
                            case AffectedStat.Dexterity:
                                pDexBonus += amount;
                                break;
                            case AffectedStat.Stamina:
                                pStaBonus += amount;
                                break;
                            case AffectedStat.Defense:
                                pDefBonus += amount;
                                break;
                        }
                    }
                    else
                    {
                        damNumMan.AddDamageNumber(amount, new Vector2(enemy.Position.X + enemy.Sprite.Width / 2 - 20, enemy.Position.Y + 15));

                        switch (move.Stat)
                        {
                            case AffectedStat.Health:
                                enemy.ModHealth(amount);
                                break;
                            case AffectedStat.MP:
                                enemy.ModMP(amount);
                                break;
                            case AffectedStat.Strength:
                                eStrBonus += amount;
                                break;
                            case AffectedStat.Intelligence:
                                eIntBonus += amount;
                                break;
                            case AffectedStat.Dexterity:
                                eDexBonus += amount;
                                break;
                            case AffectedStat.Stamina:
                                eStaBonus += amount;
                                break;
                            case AffectedStat.Defense:
                                eDefBonus += amount;
                                break;
                        }
                    }
                    pStaUsed += move.StaminaCost;
                    if ((pStaBonus + player.Stamina) - pStaUsed == 0)
                    {
                        //End player's turn. Might not automate this, might just have player click button anyway
                    }
                }
                else
                {
                    ScreenManager.AddScreen(new MessageBox("NOT ENOUGH STAMINA!"));
                }
            }
            else
            {
                if ((enemy.Stamina + eStaBonus) - eStaUsed - move.StaminaCost >= 0)
                {
                    int amount;
                    if (move.Amount > 0)
                    {
                        amount = move.Amount / 2 + new Random().Next(move.Amount);
                    }
                    else
                    {
                        amount = move.Amount / 2 - new Random().Next(Math.Abs(move.Amount));
                    }

                    if (type == 1)
                    {
                        if (amount > 0)
                        {
                            amount += eStrBonus;
                        }
                        else
                        {
                            amount -= eStrBonus;
                        }
                    }
                    else if (type == 2)
                    {
                        if (amount > 0)
                        {
                            amount += eIntBonus;
                        }
                        else
                        {
                            amount -= eIntBonus;
                        }
                    }

                    if (move.Target == Target.Self)
                    {
                        damNumMan.AddDamageNumber(amount, new Vector2(enemy.Position.X + enemy.Sprite.Width / 2 - 20, enemy.Position.Y + 15));

                        switch (move.Stat)
                        {
                            case AffectedStat.Health:
                                enemy.ModHealth(amount);
                                break;
                            case AffectedStat.MP:
                                enemy.ModMP(amount);
                                break;
                            case AffectedStat.Strength:
                                eStrBonus += amount;
                                break;
                            case AffectedStat.Intelligence:
                                eIntBonus += amount;
                                break;
                            case AffectedStat.Dexterity:
                                eDexBonus += amount;
                                break;
                            case AffectedStat.Stamina:
                                eStaBonus += amount;
                                break;
                            case AffectedStat.Defense:
                                eDefBonus += amount;
                                break;
                        }
                    }
                    else
                    {
                        damNumMan.AddDamageNumber(amount, new Vector2(player.Position.X, player.Position.Y - 10));

                        switch (move.Stat)
                        {
                            case AffectedStat.Health:
                                player.ModHealth(amount);
                                break;
                            case AffectedStat.MP:
                                player.ModMP(amount);
                                break;
                            case AffectedStat.Strength:
                                pStrBonus += amount;
                                break;
                            case AffectedStat.Intelligence:
                                pIntBonus += amount;
                                break;
                            case AffectedStat.Dexterity:
                                pDexBonus += amount;
                                break;
                            case AffectedStat.Stamina:
                                pStaBonus += amount;
                                break;
                            case AffectedStat.Defense:
                                pDefBonus += amount;
                                break;
                        }
                    }
                    eStaUsed += move.StaminaCost;
                    if ((eStaBonus + enemy.Stamina) - eStaUsed == 0)
                    {
                        EndTurn();
                    }
                }
                else
                {
                    EndTurn();
                }
            }
        }

        public void UseMagic(MagicMove magic)
        {
            if (turn == 1)
            {
                if (player.CurrentMP >= magic.MPCost)
                {
                    Move move = new Move(magic.Name, magic.StaminaCost, magic.Target, magic.Stat, magic.Amount);
                    UseMove(move, 2);
                    player.ModMP(-magic.MPCost);
                }
                else
                {
                    ScreenManager.AddScreen(new MessageBox("NOT ENOUGH MP!"));
                }
            }
            else
            {
                if (enemy.CurrentMP >= magic.MPCost)
                {
                    Move move = new Move(magic.Name, magic.StaminaCost, magic.Target, magic.Stat, magic.Amount);
                    UseMove(move, 2);
                }
                else
                {
                    //Not enough MP warning - Enemy won't need this, will search for another possible attack instead
                }
            }
        }

        public void EndTurn()
        {
            if (turn == 1)
            {
                turn++;
                eStaUsed = 0;
            }
            else
            {
                turn--;
                pStaUsed = 0;
            }
        }

        #endregion
    }
}
