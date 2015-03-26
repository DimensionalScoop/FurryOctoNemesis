using System;
using System.IO;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace X45Game.Drawing
{
    public class Font
    {
        private static ContentManager content;
        public string Filename;

        private SpriteFont _spriteFont;

        public Font(string filename)
        {
            Filename = filename;
        }

        public Font(SpriteFont font)
        {
            SpriteFont = font;
            Filename = "";
        }

        public SpriteFont SpriteFont
        {
            get
            {
                if (_spriteFont != null)
                    return _spriteFont;
                Load();
                return _spriteFont;
            }
            protected set { _spriteFont = value; }
        }

        public static void Initialize(ContentManager content)
        {
            if (content == null) throw new ArgumentNullException("content");
            Font.content = content;
        }

        public void Load()
        {
            if (content == null)
                throw new Exception("The Font class must be initalized!");

            try
            {
                SpriteFont = content.Load<SpriteFont>(Filename);
            }
            catch
            {
                throw new FileNotFoundException(Filename);
            }
        }

        public static implicit operator SpriteFont(Font font)
        {
            return font.SpriteFont;
        }
    }
}