using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ScoopFramework;
using ScoopFramework.Drawing;
using ScoopFramework.Input;
using ScoopFramework.Extensions;

namespace ScoopFramework.Drawing
{
    public class Particle
    {
        public static Random Random = new Random();

        public SpriteSheet Sprite;
        public Vector2 Origin;
        public Color Color = Color.White;
        public float Rotation;
        public float LayerDepth;

        public readonly TimeSpan CreationTime;
        public TimeSpan MaxAge;
        public TimeSpan Age(GameTime gameTime) { return gameTime.TotalGameTime - CreationTime - DelayFlag; }
        public float RelativeAge(GameTime gameTime) { return (float)(Age(gameTime).TotalMilliseconds / MaxAge.TotalMilliseconds); }
        public bool DeleteFlag;
        public TimeSpan DelayFlag;


        public Particle()
        {
            CreationTime = ParticleProvider.Default.LastUpdate;
        }

        public static void Add(Particle item)
        {
            ParticleProvider.Default.AddParticle(item);
        }

        public virtual void Delete()
        {
            DeleteFlag = true;
            ParticleProvider.Default.ParticlesToDelete = true;
        }

        protected virtual void PreDrawUpdate(GameTime gameTime) { }
        protected virtual Vector2 CalcPosition(GameTime gameTime,Vector2 camera) { return Origin+camera; }
        protected virtual Color CalcColor(GameTime gameTime) { return Color; }
        protected virtual float CalcRotation(GameTime gameTime) { return Rotation; }
        protected virtual float CalcScale(GameTime gameTime) { return 1; } 

        public virtual void Draw(SpriteBatch spriteBatch, Vector2 camera, GameTime gameTime)
        {
            if (RelativeAge(gameTime) > 1) { Delete(); return; }
            if (RelativeAge(gameTime) < 0) return;

            PreDrawUpdate(gameTime);
            if (Sprite != null)
            spriteBatch.Draw(
                Sprite,
                CalcPosition(gameTime, camera),
                gameTime,
                CalcColor(gameTime),
                Sprite.TextureOrigin,
                CalcRotation(gameTime),
                CalcScale(gameTime),
                LayerDepth,
                RelativeAge(gameTime));
        }
    }
}
