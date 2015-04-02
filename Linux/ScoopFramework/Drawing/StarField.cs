using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ScoopFramework.Drawing
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
        private bool _hide;

        public float WarpFactor=0f;


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
            _starMultiplicator = starMultiplicator * 1.8f;

            Game.Components.Add(this);
            Game.Services.AddService(typeof(StarField),this);
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

            GenerateBrightnessTable();
        }

        public void Hide() { _hide = true; }
        public void Show() { _hide = false; }

        public void MoveCamera(Vector2 delta)
        {
            _offset += delta;
            _offset.Y += delta.Y * WarpFactor*10;
        }

        public override void Draw(GameTime gameTime)
        {
            if (_hide) return;

            if (_spriteBatch == null)
                _spriteBatch = new SpriteBatch(GraphicsDevice);
            if (point == null)
            {
                point = new Texture2D(GraphicsDevice, 1, 1);
                point.SetData(new[] {Color.White});
            }

            _spriteBatch.Begin(SpriteSortMode.Deferred,BlendState.Additive);

            for (int i = 0; i < _positions.Length; i++)
            {
                var maxW = WarpFactor * _colors[i].A / 255 * 20;
                for(int w=0;w<=maxW;w++)
                    DrawStar(_positions[i] + new Vector2(0, -w), _colors[i], w == 0 ? 0 : w / maxW);
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        #region MethodFileds
        static Vector2 mpos;
        static Color mcol;
        static float mx;
        static float my;

        float[] brightnessTable;

        private void GenerateBrightnessTable()
        {
            var count = GraphicsDevice.Viewport.Width + GraphicsDevice.Viewport.Height;
            brightnessTable = new float[count];
            for (int i = 0; i < count; i++)
                brightnessTable[i] = _brightness * (_random.Next((int)MathHelper.Lerp(0, 100, _flare), 100) / 100f);
        }
        #endregion
        private void DrawStar(Vector2 position, Color color, float aOverwrite)
        {
            mpos = position + _offset;
            mpos = new Vector2(mpos.X % GraphicsDevice.Viewport.Width, mpos.Y % GraphicsDevice.Viewport.Height);
            
            mx = mpos.X / GraphicsDevice.Viewport.Width;
            my = mpos.Y / GraphicsDevice.Viewport.Height;
            mpos.X += (mx - 0.5f) * my * 4000 * WarpFactor;
            if (mpos.X < 0 || mpos.X > GraphicsDevice.Viewport.Width ||
                mpos.Y < 0 || mpos.Y > GraphicsDevice.Viewport.Height)
                return;

            mcol = Color.Multiply(color, brightnessTable[(int)MathHelper.Clamp(mpos.X + mpos.Y,0,brightnessTable.Length-1)]);
            mcol = Color.Lerp(mcol, Color.Blue, WarpFactor * 0.5f);
            mcol.A = (byte)(mcol.A * (1 - aOverwrite));

            _spriteBatch.Draw(point, mpos, mcol);
        }
    }
}