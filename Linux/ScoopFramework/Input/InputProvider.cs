using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace ScoopFramework.Input
{
    public class InputProvider:GameComponent
    {
        public MouseProvider Mouse;
        public KeyProvider Key;
        public GamePadProvider GamePad;

        public InputProvider(Game game)
            : base(game)
        {
            Game.Components.Add(this);

            Mouse = new MouseProvider();
            Key = new KeyProvider(Game);
            GamePad = new GamePadProvider(Game);

            Game.Services.AddService(typeof(InputProvider), this);
            Game.Services.AddService(typeof(MouseProvider), Mouse);
            Game.Services.AddService(typeof(KeyProvider), Key);
            Game.Services.AddService(typeof(GamePadProvider), GamePad);
        }

        public override void Initialize()
        {
            Key.Initialize();
            GamePad.Initialize();
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            Mouse.Update();
            Key.Update(gameTime);
            GamePad.Update(gameTime);
            base.Update(gameTime);
        }
    }
}
