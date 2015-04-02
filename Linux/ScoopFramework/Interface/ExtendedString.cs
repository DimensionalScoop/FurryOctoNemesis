using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScoopFramework.Drawing;
using ScoopFramework.Extensions;

namespace ScoopFramework.Interface
{
    public partial class ExtendedString
    {
        #region subtypes
        protected enum Types { Text, Bargraph, Symbol }
        protected class Base { public Types Type; }
        protected class DynText : Base { public DynText() { Type = Types.Text; } public Func<string> GetText; public Func<Color> GetColor;}
        protected class Bargraph : Base { public Bargraph() { Type = Types.Bargraph; } public Func<float> GetAmount; public Func<int> Lenght; public Func<Color> FilledColor; public Func<Color> EmptyColor;}
        protected class Symbol : Base { public Symbol() { Type = Types.Symbol; } public Func<Sprite> GetSprite; }
        #endregion

        static public Font DefaultFont = new Font("font");

        public Font FontOverwrite;

        protected List<Base> items = new List<Base>();


        public Color DefaultColor=Color.White;
        public Color DefaultBargraphFilledColor = Color.Green;
        public Color DefaultBargraphEmptyColor = Color.Gray;

        public void Write(Func<string> text, Func<Color> color = null)
        {
            items.Add(new DynText() { GetText = text, GetColor = color == null ? () => DefaultColor : color });
        }

        public void AddBarGraph(Func<int> charLenght, Func<float> value, Func<Color> filledColor=null, Func<Color> emptyColor=null)
        {
            items.Add(new Bargraph()
            {
                GetAmount = value,
                Lenght = charLenght,
                EmptyColor = emptyColor == null ? () => DefaultBargraphEmptyColor : emptyColor,
                FilledColor = filledColor == null ? () => DefaultBargraphFilledColor : filledColor
            });
        }

        public void AddSymbol(Func<Sprite> symbol)
        {
            items.Add(new Symbol() { GetSprite = symbol });
        }

        public void Clear() { items.Clear(); }

        public void Draw(SpriteBatch spriteBatch, Vector2 location)
        {
            Vector2 cursorPosition=Vector2.Zero;
            Font font = DefaultFont;
            if (FontOverwrite != null) font = DefaultFont;

            foreach (var item in items)
            {
                switch (item.Type)
                {
                    case Types.Text:
                        Color color=(item as DynText).GetColor();
                        string text=(item as DynText).GetText();
                        string[] lines=text.Split('\n');

                        if (text == "\n")
                        {
                            cursorPosition.Y += font.SpriteFont.LineSpacing;
                            cursorPosition.X = 0;
                            break;
                        }

                        for (int i = 0; i < lines.Length; i++)
                        {
                            spriteBatch.DrawString(font, lines[i], location + cursorPosition, color);
                            if (i != lines.Length - 1)
                            {
                                cursorPosition.Y += font.SpriteFont.LineSpacing;
                                cursorPosition.X = 0;
                            }
                            else
                            {
                                cursorPosition.X += font.SpriteFont.MeasureString(lines[i]).X;
                            }
                        }
                        break;

                    case Types.Bargraph:
                        int lenght=(item as Bargraph).Lenght();
                        float charLenght = font.SpriteFont.MeasureString("M").X;
                        spriteBatch.DrawProgressBar(location + cursorPosition + new Vector2(0, font.SpriteFont.LineSpacing / 3f), (int)(lenght * charLenght), (int)(charLenght * lenght * (item as Bargraph).GetAmount()),
                            font.SpriteFont.LineSpacing / 2, (item as Bargraph).EmptyColor(), (item as Bargraph).FilledColor(), false);
                        cursorPosition.X += lenght * charLenght;
                        break;

                    case Types.Symbol:
                        spriteBatch.Draw((item as Symbol).GetSprite(),location+ cursorPosition, Color.White);
                        cursorPosition.X += (item as Symbol).GetSprite().Texture.Width;
                        break;
                }
            }
        }
    }
}
