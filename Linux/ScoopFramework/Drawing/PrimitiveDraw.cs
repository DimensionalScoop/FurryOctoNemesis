using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ScoopFramework.Drawing
{
    public class PrimitiveDraw : DrawableGameComponent
    {
        private readonly int _maxStructureSize; //XXX
        private readonly VertexPositionColor[][] _vertexArray = new VertexPositionColor[short.MaxValue][];
        private BasicEffect _basicEffect;
        private Rectangle _fieldOfView;

        private SpriteBatch _spriteBatch;
        private int _vertexCounter;


        public PrimitiveDraw(Game game, int maxStructureSize = 5000)
            : base(game)
        {
            _maxStructureSize = maxStructureSize;
            game.Components.Add(this);
        }

        public Matrix ViewMatrix { get; private set; }
        public Matrix ProjectionMatrix { get; private set; }
        public Matrix WorldMatrix { get; private set; }
        public Vector3 CameraPosition { get; private set; }


        public override void Initialize()
        {
            base.Initialize();

            ViewMatrix = Matrix.CreateLookAt(new Vector3(0.0f, 0.0f, 1.0f), Vector3.Zero, Vector3.Up);
            ProjectionMatrix = Matrix.CreateOrthographicOffCenter(0, 0, 0, 0, 0.1f, 1000.0f);
            WorldMatrix = Matrix.CreateTranslation(0, 0, 0);

            _basicEffect = new BasicEffect(GraphicsDevice)
                {
                    VertexColorEnabled = true,
                    World = WorldMatrix,
                    View = ViewMatrix,
                    Projection = ProjectionMatrix
                };

            _spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        public bool IsVisible(Vector2 point)
        {
            return _fieldOfView.Contains((int) point.X, (int) point.Y);
        }

        private void DetermineFieldOfView()
        {
            Viewport viewport = GraphicsDevice.Viewport;

            Vector3 leftTop = viewport.Unproject(new Vector3(Vector2.Zero, 0), ProjectionMatrix, ViewMatrix, WorldMatrix);
            Vector3 rightBottom = viewport.Unproject(new Vector3(new Vector2(viewport.Width, viewport.Height), 0),
                                                     ProjectionMatrix, ViewMatrix, WorldMatrix);

            _fieldOfView= new Rectangle((int) leftTop.X, (int) leftTop.Y,
                                 (int) (rightBottom.X - leftTop.X),
                                 (int) (rightBottom.Y - leftTop.Y));

            _fieldOfView.Inflate(_maxStructureSize,_maxStructureSize);
        }

        public static int CountCornersOfCircle(float radius, float edgeMultiplicator = 0.1f)
        {
            return (int) ((radius*MathHelper.TwoPi)*edgeMultiplicator) + 4;
        }

        public void SetCamera(float x, float y, float z)
        {
            ProjectionMatrix = Matrix.CreateOrthographicOffCenter(-(float) GraphicsDevice.Viewport.Width*z,
                                                                  GraphicsDevice.Viewport.Width*z,
                                                                  GraphicsDevice.Viewport.Height*z,
                                                                  -(float) GraphicsDevice.Viewport.Height*z, 0.1f,
                                                                  1000.0f);
            WorldMatrix = Matrix.CreateTranslation(x, y, 0);

            _basicEffect.Projection = ProjectionMatrix;
            _basicEffect.World = WorldMatrix;

            CameraPosition = new Vector3(x, y, z);
        }

        public void Setup2D()
        {
            SetCamera(0, 0, 0);
        }

        /// <summary>
        ///     Draws a circle.
        /// </summary>
        public void DrawCircle(Vector2 position, float radius, float rotation, Color color,
                               float edgeMultiplicator = 0.1f)
        {
            if (!_fieldOfView.Contains((int) position.X, (int) position.Y))
                return;

            Debug.Assert(edgeMultiplicator >= 0);
            int newVerticles = (int) ((radius*MathHelper.TwoPi)*edgeMultiplicator) + 4;
            _vertexArray[_vertexCounter] = new VertexPositionColor[newVerticles];

            for (int i = 0; i < newVerticles; i++)
            {
                float angle = (float) i/(newVerticles - 1)*MathHelper.TwoPi + rotation;
                _vertexArray[_vertexCounter][i] =
                    new VertexPositionColor(
                        new Vector3((float) (Math.Sin(angle)*radius) + position.X,
                                    (float) (Math.Cos(angle)*radius) + position.Y, 0), color);
            }
            _vertexCounter++;
        }

        public void DrawLine(Vector2 start, Vector2 end, Color color, float depth = 0)
        {
            if (!IsVisible(start))
                return;

            _vertexArray[_vertexCounter] = new VertexPositionColor[2];

            _vertexArray[_vertexCounter][0] = new VertexPositionColor(new Vector3(start, depth), color);
            _vertexArray[_vertexCounter][1] = new VertexPositionColor(new Vector3(end, depth), color);

            _vertexCounter++;
        }

        public void DrawLine(Vector2 start, Vector2 end, Color startColor,Color endColor,float depth = 0)
        {
            if (!IsVisible(start))
                return;

            _vertexArray[_vertexCounter] = new VertexPositionColor[2];

            _vertexArray[_vertexCounter][0] = new VertexPositionColor(new Vector3(start, depth), startColor);
            _vertexArray[_vertexCounter][1] = new VertexPositionColor(new Vector3(end, depth), endColor);

            _vertexCounter++;
        }

        public void DrawSpriteActualSize(Sprite sprite, Vector2 position, Color color, float rotation, Vector2 origin)
        {
            Vector3 position3 = GraphicsDevice.Viewport.Project(new Vector3(position, 0), ProjectionMatrix, ViewMatrix,
                                                                WorldMatrix);
            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
            _spriteBatch.Draw(sprite, new Vector2(position3.X, position3.Y), null, Color.White, rotation, origin, 1,
                              SpriteEffects.None, 0);
            _spriteBatch.End();
        }

        public void DrawSprite(Sprite sprite, Vector2 position, Color color, float rotation, Vector2 origin, float size)
        {
            Vector3 position3 = GraphicsDevice.Viewport.Project(new Vector3(position, 0), ProjectionMatrix, ViewMatrix,
                                                                WorldMatrix);
            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
            _spriteBatch.Draw(sprite, new Vector2(position3.X, position3.Y), null, Color.White, rotation, origin,
                              size/CameraPosition.Z, SpriteEffects.None, 0);
            _spriteBatch.End();
        }

        public override void Draw(GameTime gameTime)
        {
            DetermineFieldOfView();

            foreach (EffectPass pass in _basicEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                for (int i = 0; i < _vertexCounter; i++)
                    GraphicsDevice.DrawUserPrimitives(PrimitiveType.LineStrip, _vertexArray[i], 0,
                                                      _vertexArray[i].Length - 1);
            }

            _vertexCounter = 0;

            base.Draw(gameTime);
        }
    }
}