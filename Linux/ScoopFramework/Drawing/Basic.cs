using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using X45Game.Extensions;

namespace X45Game.Drawing
{
    public class Basic
    {
        private static Sprite point;

        public static void DrawProgressBar(SpriteBatch spriteBatch, Vector2 position, int backgroundLenght,
                                           int foregroundLenght, int height, Color backgroundColor,
                                           Color foregroundColor)
        {
            if (point == null)
                point = new Sprite("drawing\\point.png");

            var barPosition = new Point((int) (position.X - backgroundLenght/2f), (int) position.Y - height);

            spriteBatch.Draw(point, new Rectangle(barPosition.X, barPosition.Y, backgroundLenght, height),
                             backgroundColor);
            spriteBatch.Draw(point, new Rectangle(barPosition.X, barPosition.Y, foregroundLenght, height),
                             foregroundColor);
        }

        public static void DrawLine(SpriteBatch spriteBatch, Vector2 startPosition, Vector2 endPosition, Color color)
        {
            if (point == null)
                point = new Sprite("drawing\\point.png");

            spriteBatch.Draw(point,
                             new Rectangle((int) Math.Round(startPosition.X), (int) Math.Round(startPosition.Y),
                                           (int) (endPosition - startPosition).Length(), 1), null, color,
                             endPosition.Angle(startPosition), Vector2.Zero, SpriteEffects.None, 0);
            //spriteBatch.Draw(point,startPosition, new Rectangle(0, 0, (int)Vector2.Distance(startPosition,endPosition),1), color, (float)endPosition.Angle(startPosition), Vector2.Zero,1, SpriteEffects.None, 0);
        }

        public static void DrawRectangle(SpriteBatch spriteBatch, Vector2 location, float width, float height,
                                         Color color)
        {
            if (point == null)
                point = new Sprite("drawing\\point.png");
            spriteBatch.Draw(point, new Rectangle((int) location.X, (int) location.Y, (int) width, (int) height), color);
        }

        public static void DrawBox(SpriteBatch spriteBatch, Vector2 position, int width, int height)
        {
            DrawBox(spriteBatch, position, width, height, new Color(245, 245, 180), new Color(10, 10, 75));
        }

        public static void DrawBox(SpriteBatch spriteBatch, Vector2 position, int width, int height,
                                   Color backgroundColor, Color boundsColor)
        {
            DrawRectangle(spriteBatch, position, width - 1, height, backgroundColor);

            DrawLine(spriteBatch, position, position + new Vector2(width, 0), boundsColor);
            DrawLine(spriteBatch, position + new Vector2(0, height - 2),
                     position + new Vector2(0, height - 2) + new Vector2(width, 0), boundsColor);
            DrawLine(spriteBatch, position + new Vector2(0, height - 1),
                     position + new Vector2(0, height - 1) + new Vector2(width, 0), boundsColor);
            DrawLine(spriteBatch, position, position + new Vector2(0, height), boundsColor);
            DrawLine(spriteBatch, position + new Vector2(width - 2, 0),
                     position + new Vector2(width - 2, 0) + new Vector2(0, height), boundsColor);
            DrawLine(spriteBatch, position + new Vector2(width - 1, 0),
                     position + new Vector2(width - 1, 0) + new Vector2(0, height), boundsColor);
        }
    }
}