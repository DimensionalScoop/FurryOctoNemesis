using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace X45Game.Drawing
{
    public class StarField : DrawableGameComponent
    {
        private const int DefaultStarDensity = 100;
        private static Texture2D point;
        private readonly float _brightness;
        private readonly float _flare;
        private readonly float _starMultiplicator;
        private Color[] _colors;
        private Vector2 _offset;
        private Vector2[] _positions;
        private Random _random;
        private SpriteBatch _spriteBatch;


        /// <param name="game"></param>
        /// <param name="starMultiplicator">How many stars will appear. The higher the number the less stars.</param>
        /// <param name="brightness">How white the stars will be. Use 0 for a black sky.</param>
        /// <param name="flare">How much the stars will twinkle. Use 0 for maximum flare and 1 for no flare.</param>
        public StarField(Game game, float starMultiplicator, float brightness, float flare)
            : base(game)
        {
            Debug.Assert(flare >= 0);
            Debug.Assert(brightness >= 0);
            Debug.Assert(brightness <= 1);

            _brightness = brightness;
            _flare = flare;
            _starMultiplicator = starMultiplicator;
        }

        public override void Initialize()
        {
            base.Initialize();

            _random = new Random();
            var starList = new List<Vector2>();

            for (int x = 0; x < GraphicsDevice.Viewport.Width; x++)
                for (int y = 0; y < GraphicsDevice.Viewport.Height; y++)
                {
                    if (_random.Next((int) (DefaultStarDensity*_starMultiplicator)) == 0)
                        starList.Add(new Vector2(x, y));
                }

            _positions = starList.ToArray();
            _colors = new Color[_positions.Length];

            for (int i = 0; i < _colors.Length; i++)
            {
                _colors[i] = Color.Multiply(Color.White, (float) _random.NextDouble());
            }

            _offset += new Vector2(10000, 10000); //XXX
        }

        public void MoveCamera(Vector2 delta)
        {
            _offset += delta;
        }

        public override void Draw(GameTime gameTime)
        {
            if (_spriteBatch == null)
                _spriteBatch = new SpriteBatch(GraphicsDevice);
            if (point == null)
            {
                point = new Texture2D(GraphicsDevice, 1, 1);
                point.SetData(new[] {Color.White});
            }

            _spriteBatch.Begin();

            for (int i = 0; i < _positions.Length; i++)
            {
                Vector2 pos = _positions[i] + _offset;
                _spriteBatch.Draw(point,
                                 new Vector2(pos.X%GraphicsDevice.Viewport.Width, pos.Y%GraphicsDevice.Viewport.Height),
                                 Color.Multiply(_colors[i],
                                                _brightness*(_random.Next((int) MathHelper.Lerp(0, 100, _flare), 100)/100f)));
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}