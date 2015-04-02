using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScoopFramework.Drawing;

namespace ScoopFramework.Effect
{
    public class Effect
    {
        public Vector2 Position;
        public Sprite Sprite;
        public Color Color;
        public float Rotation;
        public float Angle;
        public float Speed;
        public float Zoom = 1;
        public bool DeleteFlag;
        public readonly DateTime CreationTime;

        public TimeSpan Age { get { return DateTime.Now - CreationTime; } }

        public List<Behavior> Behavior = new List<Behavior>();


        public Effect() { CreationTime = DateTime.Now; }

        public void Delete() { DeleteFlag = true; }

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (DeleteFlag) return;
            Behavior.ForEach(p => p.Update(gameTime, this));
            Behavior.RemoveAll(p => p.DeleteFlag);
            if (Behavior.Count == 0) Delete();

            if (Sprite != null && !DeleteFlag) spriteBatch.Draw(Sprite, Position, null, Color, Rotation, Sprite.TextureOrigin, Zoom, SpriteEffects.None, 0);
        }
    }
}