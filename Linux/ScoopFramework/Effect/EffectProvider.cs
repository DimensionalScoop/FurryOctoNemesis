using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ScoopFramework.Effect
{
    public class EffectProvider : DrawableGameComponent
    {
        List<Effect> _effects = new List<Effect>();
        SpriteBatch _spriteBatch;

        static EffectProvider DefaultProvider;

        private EffectProvider(Game game) : base(game) 
        {
            Game.Components.Add(this);
            Game.Services.AddService(typeof(EffectProvider), this);
        }

        public static EffectProvider Initialize(Game game)
        {
            DefaultProvider = new EffectProvider(game);
            return DefaultProvider;
        }

        public static void AddEffect(Effect item) { DefaultProvider._effects.Add(item); }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied);
            _effects.ForEach(p => p.Draw(gameTime, _spriteBatch));
            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}