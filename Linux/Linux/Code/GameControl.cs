using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.IO;
using System.Diagnostics;

namespace Linux.Code
{
    public class GameControl:DrawableGameComponent
    {
        SpriteBatch spriteBatch;
        Random random = new Random();
        public static GameTime LastUpdate = new GameTime();

        DirectoryInfo path = new DirectoryInfo(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location));


        public static Player LocalPlayer;
        public static List<Player> AllPlayers = new List<Player>();
        public static Player GetPlayerByID(byte netID)
        {
            Debug.Assert(netID != 0);
            return AllPlayers.Find(p => p.NetID == netID);
        }


        public GameControl(Game game) : base(game) 
        {
            game.Components.Add(new Server(game));

            GameControl.LocalPlayer = new Player();
            AllPlayers.Add(LocalPlayer);
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }

        protected override void OnEnabledChanged(object sender, EventArgs args)
        {
            base.OnEnabledChanged(sender, args);
        }
    }
}
