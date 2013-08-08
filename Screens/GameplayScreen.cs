using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PoorlyDrawnEngine.StateManagement;
using PoorlyDrawnDungeons.Entities;
using PoorlyDrawnEngine.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using PoorlyDrawnDungeons.Menus;
using PoorlyDrawnDungeons.BattleSystem;
using PoorlyDrawnDungeons.SaveData;
using System.IO;
using System.Xml.Serialization;
using PoorlyDrawnDungeons.Managers;

namespace PoorlyDrawnDungeons.Screens
{
    public class GameplayScreen : GameScreen
    {
        #region Variables

        private ContentManager content;
        private TextureManager tMan;
        private LevelManager lvlMan;

        #region Monster Sprites

        Sprite[] goblinWarrior = new Sprite[1];

        #endregion

        #endregion

        #region Constructor

        public GameplayScreen(/*Player player*/)
        {
            tMan = TextureManager.GetInstance();
            lvlMan = LevelManager.GetInstance();
            //this.player = player;
        }

        #endregion

        #region Load/Unload

        public override void LoadContent(ContentManager content)
        {
            this.content = content;
            Sprite[] playerSprites = { new Sprite(tMan.PlayerTexture, 189, 128, 4, true, true, 300f) };
            List<MagicMove> magic = new List<MagicMove>();
            MagicData data = LoadMagicData(TitleContainer.OpenStream(@"Content\DataFiles\Spells\Fireball.spell"));
            for (int i = 0; i < 7; i++)
            {
                magic.Add(new MagicMove(data));
            }
            lvlMan.Player = new Player(tMan.PlayerMapImage, playerSprites, new Vector2(250, /*Camera.ViewPortHeight - 450*/ 250), new Move(LoadMoveData(TitleContainer.OpenStream(@"Content\DataFiles\Weapons\dagger.move"))), magic);
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
        }

        #endregion

        #region Update

        public override void Update(GameTime gameTime, bool hasFocus, bool coveredByScreen)
        {
            if (hasFocus)
            {
                lvlMan.Update(gameTime);
                /*goblinWarrior[0] = new Sprite(tMan.EnemyTextures["GoblinWarrior"], 179, 128, 4, true, true, 300f);
                BattleScreen battle = new BattleScreen(player, CreateEnemy(LoadEnemyData(TitleContainer.OpenStream(@"Content\DataFiles\Enemies\GoblinWarrior.baddie"))));
                screenManager.AddScreen(battle);
                screenManager.AddScreen(new BattleMenu(battle));*/
                base.Update(gameTime, hasFocus, coveredByScreen);
            }
        }

        public override void HandleInput(InputHandler input)
        {
            lvlMan.HandleInput(input);
            base.HandleInput(input);
        }

        #endregion

        #region Draw

        public override void Draw()
        {
            if (hasFocus)
            {
                lvlMan.Draw(ScreenManager.SpriteBatch);
            }
        }

        #endregion

        #region DataFileLoading

        public MoveData LoadMoveData(Stream fileStream)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(MoveData));
            return (MoveData)serializer.Deserialize(fileStream);
        }

        public MagicData LoadMagicData(Stream fileStream)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(MagicData));
            return (MagicData)serializer.Deserialize(fileStream);
        }

        public EnemyData LoadEnemyData(Stream fileStream)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(EnemyData));
            return (EnemyData)serializer.Deserialize(fileStream);
        }

        #endregion

        #region EnemyCreation

        public Enemy CreateEnemy(EnemyData data)
        {
            Move weapon;
            weapon = new Move(LoadMoveData(TitleContainer.OpenStream(@"Content\DataFiles\Weapons\" + data.weapon + ".move")));
            List<Move> item = new List<Move>();
            for (int i = 0; i < data.items.Count; i++)
            {
                item.Add(new Move(LoadMoveData(TitleContainer.OpenStream(@"Content\DataFiles\Items\" + data.items[i] + ".move"))));
            }
            List<MagicMove> magic = new List<MagicMove>();
            for (int i = 0; i < data.magic.Count; i++)
            {
                magic.Add(new MagicMove(LoadMagicData(TitleContainer.OpenStream(@"Content\DataFiles\Spells\" + data.magic[i] + ".spell"))));
            }
            Enemy enemy = new Enemy(data, weapon, item, magic, GetSprites(data.name, data.type));
            return enemy;
        }

        public Sprite[] GetSprites(string name, string type)
        {
            switch (name)
            {
                case "Goblin":
                    switch (type)
                    {
                        case "Warrior":
                            return goblinWarrior;
                        default:
                            return goblinWarrior;
                    }
                default:
                    return goblinWarrior;
            }
        }
        #endregion
    }
}
