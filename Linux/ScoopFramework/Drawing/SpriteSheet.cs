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
    public class SpriteSheet:Sprite
    {
        public readonly int FrameLength;
        public int CountFrames { get { return Texture.Width / FrameLength; } }
        public readonly TimeSpan FrameDuration;

        static readonly TimeSpan defaultFrameDuration = TimeSpan.FromMilliseconds(150);

        public SpriteSheet(string filename)
            : base(filename)
        {
            FrameLength = -1;
        }

        public SpriteSheet(string filename, int frameLength)
            : base(filename)
        {
            FrameLength = frameLength;
            FrameDuration = defaultFrameDuration;
        }

        public SpriteSheet(string filename,TimeSpan frameDuration ,int frameLength = 4)
            : base(filename)
        {
            FrameLength = frameLength;
            FrameDuration = frameDuration;
        }

        public override Vector2 TextureOrigin
        {
            get
            {
                if (FrameLength == -1) return base.TextureOrigin;
                return new Vector2(FrameLength / 2, Texture.Height / 2);
            }
        }
    }
}
