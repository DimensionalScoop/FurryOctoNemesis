using System;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using Microsoft.Xna.Framework;
using System.Diagnostics;
using Microsoft.Xna.Framework.Content;

namespace X45Game.Drawing
{
    public class Sprite
    {
        public string Filename;
        public Texture2D Texture
        {
            get
            {
                if (_texture != null)
                    return _texture;
                Load();
                return _texture;
            }
            protected set
            {
                _texture = value;
            }
        }
        public Vector2 TextureOrigin
        {
            get
            {
                return new Vector2(Texture.Width / 2, Texture.Height / 2);
            }
        }
        Texture2D _texture;

        static ContentManager content;
        static GraphicsDevice device;

        public static void Initialize(ContentManager content, GraphicsDevice device)
        {
            if (content == null) throw new ArgumentNullException("content");
            if (device == null) throw new ArgumentNullException("device");
            Sprite.content = content;
            Sprite.device = device;
        }

        public Sprite(string filename)
        {
            Filename = filename;
        }

        public Sprite(Texture2D texture)
        {
            Texture = texture;
            Filename = "";
        }

        public void Load()
        {
            if (content == null || device == null)
                throw new Exception("The Sprite class must be initalized!");

            try
            {
                Texture = content.Load<Texture2D>(Filename);
            }
            catch
            {
                try
                {
                    using (Stream r = new FileStream(Filename, FileMode.Open))
                            Texture = Texture2D.FromStream(device, r);
                }
                catch
                {
                    try
                    {
                        using (Stream r = new FileStream("Sprite\\"+Filename, FileMode.Open))
                                Texture = Texture2D.FromStream(device, r);
                    }
                    catch
                    {
                        throw new FileNotFoundException(Filename);
                    }
                }
            }
        }

        public void SaveTo(string file, int width, int height)
        {
            using (FileStream stream = new FileStream(file, FileMode.CreateNew))
                Texture.SaveAsPng(stream, width, height);
        }

        public void SaveTo(string file)
        {
            SaveTo(file, Texture.Width, Texture.Height);
        }

        public static implicit operator Texture2D(Sprite sprite)
        {
            return sprite.Texture;
        }
    }
}
