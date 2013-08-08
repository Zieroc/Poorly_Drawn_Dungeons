using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PoorlyDrawnEngine.TileEngine;
using PoorlyDrawnDungeons.SaveData;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using System.Xml.Serialization;
using PoorlyDrawnDungeons.Entities;
using PoorlyDrawnEngine.StateManagement;

namespace PoorlyDrawnDungeons.Managers
{
    public class LevelManager
    {
        #region Variables

        private Map dungeonMap;
        private int level;
        private EncounterData encounterTable;
        private static LevelManager instance;
        private Player player;

        #endregion

        #region Constructor

        private LevelManager()
        {
            dungeonMap = new Map(TextureManager.GetInstance().Tiles);
            level = 1;
            LoadLevel();
        }

        public static LevelManager GetInstance()
        {
            if (instance == null)
            {
                instance = new LevelManager();
            }

            return instance;
        }

        #endregion

        #region Properties

        public Player Player
        {
            get { return player; }
            set { player = value; }
        }

        public Map DungeonMap
        {
            get { return dungeonMap; }
        }

        #endregion

        #region Update

        public void Update(GameTime gameTime)
        {
            player.Update(gameTime);
        }

        #endregion

        #region HandleInput

        public void HandleInput(InputHandler input)
        {
            player.HandleInput(input);
        }

        #endregion

        #region Draw

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
            dungeonMap.Draw(spriteBatch);
            player.MapDraw(spriteBatch);
            spriteBatch.End();
        }

        #endregion

        #region Loading

        public LevelData LoadLevelData(Stream stream)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(LevelData));
            return (LevelData)serializer.Deserialize(stream);
        }

        public void LoadLevel()
        {
            try
            {
                LevelData levelData = LoadLevelData(TitleContainer.OpenStream(@"Content\DataFiles\Levels\Level" + level + ".lvl"));
                encounterTable = levelData.encounterTable;
                dungeonMap.LoadMap(TitleContainer.OpenStream(@"Content\DataFiles\Maps\" + levelData.map));
            }
            catch
            {
                
            }
        }

        #endregion
    }
}
