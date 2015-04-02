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
using System.Diagnostics;

namespace ScoopFramework.Extensions
{
    public static class SpriteBatchExtension
    {
        static Sprite point;
        static int frame;

        public static void Draw(this SpriteBatch spriteBatch, SpriteSheet sheet, Vector2 position, GameTime gameTime, Color color, Vector2 origin, float rotation = 0, float scale = 1, float layerDepth = 0, float percentualFrame = -1)
        {
            if (sheet.FrameLength == -1)
            {
                spriteBatch.Draw(sheet, position, null, color, rotation, origin, scale, SpriteEffects.None, layerDepth);
                return;
            }

            if (percentualFrame == -1)
            {
                var m = (gameTime.TotalGameTime.Ticks) % ((sheet.FrameDuration.Ticks * sheet.CountFrames));
                frame = (int)(m / ((sheet.FrameDuration.Ticks)));
                if (frame >= sheet.CountFrames) frame = sheet.CountFrames - 1;//FIX: sometimes spritesheet frames are calculated wrong
            }
            else
                frame = (int)(sheet.Texture.Width * percentualFrame / sheet.FrameLength);//XXX

            spriteBatch.Draw(sheet, position, new Rectangle(sheet.FrameLength * frame, 0, sheet.FrameLength, sheet.Texture.Height), color, rotation, origin, scale, SpriteEffects.None, layerDepth);
        }

        public static void DrawProgressBar(this SpriteBatch spriteBatch, Vector2 position, int backgroundLength,
                                           int foregroundLength, int height, Color backgroundColor,
                                           Color foregroundColor,bool aligned=true)
        {
            if (point == null)
                point = new Sprite("drawing\\resources\\point.png");

            var barPosition = new Point((int)(position.X - (aligned?backgroundLength / 2f:0)), (int)position.Y - (aligned?height:0));

            spriteBatch.Draw(point, new Rectangle(barPosition.X, barPosition.Y, backgroundLength, height),
                             backgroundColor);
            spriteBatch.Draw(point, new Rectangle(barPosition.X, barPosition.Y, foregroundLength, height),
                             foregroundColor);
        }

        public static void DrawLine(this SpriteBatch spriteBatch, Vector2 startPosition, Vector2 endPosition, Color color, float layerDepth = 0)
        {
            if (point == null)
                point = new Sprite("drawing\\resources\\point.png");

            spriteBatch.Draw(point,
                             new Rectangle((int)Math.Round(startPosition.X), (int)Math.Round(startPosition.Y),
                                           (int)(endPosition - startPosition).Length(), 1), null, color,
                             endPosition.Angle(startPosition), Vector2.Zero, SpriteEffects.None, layerDepth);
            //spriteBatch.Draw(point,startPosition, new Rectangle(0, 0, (int)Vector2.Distance(startPosition,endPosition),1), color, (float)endPosition.Angle(startPosition), Vector2.Zero,1, SpriteEffects.None, 0);
        }

        public static void DrawRectangle(this SpriteBatch spriteBatch, Vector2 location, float width, float height,
                                         Color color, float drawOrder = 0)
        {
            if (point == null)
                point = new Sprite("drawing\\resources\\point.png");
            spriteBatch.Draw(point, new Rectangle((int)location.X, (int)location.Y, (int)width, (int)height), null, color, 0, Vector2.Zero, SpriteEffects.None, drawOrder);
        }

        public static void DrawPoint(this SpriteBatch spriteBatch, Vector2 location,
                                         Color color, float drawOrder = 0)
        {
            if (point == null)
                point = new Sprite("drawing\\resources\\point.png");
            spriteBatch.Draw(point, location,null, color, 0, Vector2.Zero,1, SpriteEffects.None, drawOrder);
        }

        public static void DrawBox(this SpriteBatch spriteBatch, Vector2 position, int width, int height)
        {
            DrawBox(spriteBatch, position, width, height, new Color(245, 245, 180), new Color(10, 10, 75));
        }

        public static void DrawBox(this SpriteBatch spriteBatch, Vector2 position, int width, int height,
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

        public static void DrawText(this SpriteBatch spriteBatch, string text, Vector2 position, bool align, Font font, Color color, float layerDepth=0,bool allowInBetweenPixels=false)
        {
            position = (align ?
                position - font.SpriteFont.MeasureString(text) / 2 :
                position);

            if (!allowInBetweenPixels) position = new Vector2((int)position.X, (int)position.Y);
            spriteBatch.DrawString(font, text, position, color,0,Vector2.Zero,1,SpriteEffects.None,layerDepth);
        }
    }
}
