using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace PoorlyDrawnDungeons.Managers
{
    public class TextureManager
    {
        #region Variables

        private static TextureManager instance;
        private Dictionary<string, Texture2D> spellTextures;
        private Dictionary<string, Texture2D> enemyTextures;
        private Dictionary<string, Texture2D> itemTextures;
        private Texture2D battleMenuButton;
        private Texture2D circRecButton;
        private Texture2D[] battleMenuImages;
        private Texture2D arrow;
        private Texture2D backArrow;
        private Texture2D healthBar;
        private Texture2D sandTimer;
        private Texture2D uiMenu;
        private Texture2D uiMenu2;
        private Texture2D battleBackground;
        private Texture2D playerTexture;
        private Texture2D messageBox;
        private Texture2D closeButton;
        private Texture2D tiles;
        private Texture2D playerMapImage;

        #endregion

        #region Constuctor

        private TextureManager()
        {
            spellTextures = new Dictionary<string, Texture2D>();
            enemyTextures = new Dictionary<string, Texture2D>();
            itemTextures = new Dictionary<string, Texture2D>();
            battleMenuImages = new Texture2D[4];
        }

        public static TextureManager GetInstance()
        {
            if(instance == null)
            {
                instance = new TextureManager();
            }

            return instance;
        }

        #endregion

        #region Properties

        public Dictionary<string, Texture2D> SpellTextures
        {
            get { return spellTextures; }
        }

        public Dictionary<string, Texture2D> EnemyTextures
        {
            get { return enemyTextures; }
        }

        public Dictionary<string, Texture2D> ItemTextures
        {
            get { return itemTextures; }
        }

        public Texture2D CircRecButton
        {
            get { return circRecButton; }
        }

        public Texture2D BattleMenuButton
        {
            get { return battleMenuButton; }
        }

        public Texture2D[] BattleMenuImages
        {
            get { return battleMenuImages; }
        }

        public Texture2D Arrow
        {
            get { return arrow; }
        }

        public Texture2D BackArrow
        {
            get { return backArrow; }
        }

        public Texture2D HealthBar
        {
            get { return healthBar; }
        }

        public Texture2D SandTimer
        {
            get { return sandTimer; }
        }

        public Texture2D UIMenu
        {
            get { return uiMenu; }
        }

        public Texture2D UIMenu2
        {
            get { return uiMenu2; }
        }

        public Texture2D BattleBackground
        {
            get { return battleBackground; }
        }

        public Texture2D PlayerTexture
        {
            get { return playerTexture; }
        }

        public Texture2D MessageBox
        {
            get { return messageBox; }
        }

        public Texture2D CloseButton
        {
            get { return closeButton; }
        }

        public Texture2D Tiles
        {
            get { return tiles; }
        }

        public Texture2D PlayerMapImage
        {
            get { return playerMapImage; }
        }

        #endregion

        #region Load Content

        public void LoadContent(ContentManager content)
        {
            circRecButton = content.Load<Texture2D>("UI/CircRecButton");
            battleMenuButton = content.Load<Texture2D>("UI/BattleMenuButton");
            arrow = content.Load<Texture2D>("UI/Arrow");
            backArrow = content.Load<Texture2D>("UI/BackArrow");
            healthBar = content.Load<Texture2D>("UI/healthbar");
            sandTimer = content.Load<Texture2D>("UI/SandTimer");
            uiMenu = content.Load<Texture2D>("UI/UIMenu");
            uiMenu2 = content.Load<Texture2D>("UI/UIMenu2");
            battleBackground = content.Load<Texture2D>("Backgrounds/DungeonWall");
            battleMenuImages[0] = content.Load<Texture2D>("UI/Attack");
            //battleMenuImages[1] = content.Load<Texture2D>("UI/Flee");
            battleMenuImages[2] = content.Load<Texture2D>("UI/Item");
            battleMenuImages[3] = content.Load<Texture2D>("UI/Magic");
            messageBox = content.Load<Texture2D>("UI/MessageBox");
            closeButton = content.Load<Texture2D>("UI/closeButton");
            tiles = content.Load<Texture2D>("Tiles/Tiles");
            playerMapImage = content.Load<Texture2D>("MapImages/player");

            enemyTextures.Add("GoblinWarrior", content.Load<Texture2D>("BattleSprites/BattleGoblin"));
            spellTextures.Add("Fireball", content.Load<Texture2D>("UI/Spells/Fireball"));
            playerTexture = content.Load<Texture2D>("BattleSprites/BattleBob");
        }

        #endregion
    }
}
