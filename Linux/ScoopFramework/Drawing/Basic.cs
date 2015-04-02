using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ScoopFramework.Extensions;

namespace ScoopFramework.Drawing
{
    [Obsolete("Use the SpriteBatch extensions intead")]
    public class Basic
    {
        private static Sprite point;

        public static void DrawProgressBar(SpriteBatch spriteBatch, Vector2 position, int backgroundLength,
                                           int foregroundLength, int height, Color backgroundColor,
                                           Color foregroundColor)
        {
            if (point == null)
                point = new Sprite("drawing\\resources\\point.png");

            var barPosition = new Point((int) (position.X - backgroundLength/2f), (int) position.Y - height);

            spriteBatch.Draw(point, new Rectangle(barPosition.X, barPosition.Y, backgroundLength, height),
                             backgroundColor);
            spriteBatch.Draw(point, new Rectangle(barPosition.X, barPosition.Y, foregroundLength, height),
                             foregroundColor);
        }

        public static void DrawLine(SpriteBatch spriteBatch, Vector2 startPosition, Vector2 endPosition, Color color,float layerDepth=0)
        {
            if (point == null)
                point = new Sprite("drawing\\resources\\point.png");

            spriteBatch.Draw(point,
                             new Rectangle((int) Math.Round(startPosition.X), (int) Math.Round(startPosition.Y),
                                           (int) (endPosition - startPosition).Length(), 1), null, color,
                             endPosition.Angle(startPosition), Vector2.Zero, SpriteEffects.None, layerDepth);
            //spriteBatch.Draw(point,startPosition, new Rectangle(0, 0, (int)Vector2.Distance(startPosition,endPosition),1), color, (float)endPosition.Angle(startPosition), Vector2.Zero,1, SpriteEffects.None, 0);
        }

        public static void DrawRectangle(SpriteBatch spriteBatch, Vector2 location, float width, float height,
                                         Color color,float drawOrder=0)
        {
            if (point == null)
                point = new Sprite("drawing\\resources\\point.png");
            spriteBatch.Draw(point, new Rectangle((int) location.X, (int) location.Y, (int) width, (int) height),null, color,0,Vector2.Zero, SpriteEffects.None,drawOrder);
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

        public static void DrawText(SpriteBatch spriteBatch,string text, Vector2 position,bool align, Font font, Color color)
        {
            position=(align?
                position-font.SpriteFont.MeasureString(text)/2:
                position);

            spriteBatch.DrawString(font,text,new Vector2((int)position.X,(int)position.Y),color);
        }
    }
}