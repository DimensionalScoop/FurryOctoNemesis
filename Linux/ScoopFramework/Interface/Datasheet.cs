using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ScoopFramework.Drawing;

namespace ScoopFramework.Interface
{
    public class DatasheetItem
    {
        internal enum Types
        {
            Text,NewLine,Symbol,Variable,Color
        }

        public string Text;
        public Sprite Symbol;
        public Func<string> Variable;
        public int VariableLength;
        public Color? Color;
        
        internal readonly Types Type;

        internal Vector2 Position;


        internal DatasheetItem()
        {
            Type=Types.NewLine;
        }

        internal DatasheetItem(string text)
        {
            Text = text;
            Type = Types.Text;
        }

        internal DatasheetItem(Sprite symbol)
        {
            Symbol = symbol;
            Type=Types.Symbol;
        }

        internal DatasheetItem(Func<string> variable)
        {
            Variable = variable;
            Type=Types.Variable;
        }

        internal DatasheetItem(Func<string> variable,Color color)
        {
            Variable = variable;
            Color = color;
            Type=Types.Variable;
        }

        internal DatasheetItem(Color color)
        {
            Color=color;
            Type=Types.Color;
        }
    }

    public class Datasheet
    {
        private RenderTarget2D _sheet;
        private readonly Font _font;
        private readonly Color _backgroundColor;
        private readonly Color _fontColor;
        private readonly Color _boundsColor;

        private bool _redraw;
        private SpriteBatch _spriteBatch;

        public List<DatasheetItem> Content=new List<DatasheetItem>();
        private int _contentHash;

        public int Height { get; private set; }
        public int Width { get; private set; }

        static Dictionary<string, Sprite> symbols=new Dictionary<string, Sprite>();
        

        public static void AddSymbol(string identifier, Sprite symbol)
        {
            symbols.Add(identifier, symbol);
        }

        public static void RemoveSymbol(string identifier)
        {
            symbols.Remove(identifier);
        }


        public Datasheet(Font font, Color backgroundColor, Color fontColor, Color boundsColor)
        {
            _font = font;
            _backgroundColor = backgroundColor;
            _fontColor = fontColor;
            _boundsColor = boundsColor;
        }

        public void WriteText(string text)
        {
            text = text.Replace('\n', ' ');
            Content.Add(new DatasheetItem(text));
        }

        public void WriteLine(string line)
        {
            line = line.Replace('\n', ' ');
            Content.Add(new DatasheetItem(line));
            WriteNewLine();
        }

        public void WriteNewLine()
        {
            Content.Add(new DatasheetItem());
        }

        public void WriteSymbol(string identifier)
        {
           Content.Add(new DatasheetItem(symbols[identifier]));
        }

        public void WriteSymbol(Sprite symbol)
        {
            Content.Add(new DatasheetItem(symbol));
        }

        public void WriteVariable(Func<string> variable)
        {
            Content.Add(new DatasheetItem(variable));
        }

        public void WriteValueInfo(string explainText, Func<string> value, string suffix = "")
        {
            WriteText(explainText+": ");
            WriteVariable(value);
            WriteLine(" "+suffix);
        }

        public void WriteValueValueInfo(string explainText, Func<string> firstValue, Func<string> secondValue, string suffix = "")
        {
            WriteText(explainText + ": ");
            WriteVariable(firstValue);
            WriteText("/");
            WriteVariable(secondValue);
            WriteLine(" "+suffix);
        }

        public void ChangeFontColor(Color newColor)
        {
            Content.Add(new DatasheetItem(newColor));
        }

        public void Update(GraphicsDevice device)
        {
            if (_redraw) Redraw(device);
        }

        public void Draw(SpriteBatch spriteBatch, GraphicsDevice device, Vector2 position, Color color)
        {
            if (Content.GetHashCode() != _contentHash || _sheet == null)
            { _redraw = true; return; }

            spriteBatch.Draw(_sheet, position, color);

            foreach (var datasheetItem in Content)
                if (datasheetItem.Type == DatasheetItem.Types.Variable)
                {
                    if (datasheetItem.VariableLength != datasheetItem.Variable().Length)
                    { _redraw = true; }

                    spriteBatch.DrawString(_font,
                                           datasheetItem.Variable(),
                                           datasheetItem.Position + position,
                                           (datasheetItem.Color == null ? _fontColor : (Color)datasheetItem.Color));
                }
        }

        public void Redraw(GraphicsDevice device)
        {
            CompileSheet(device);
        }

        private void CompileSheet(GraphicsDevice device)
        {
            if(_spriteBatch==null)
                _spriteBatch=new SpriteBatch(device);

            CalculatePositions();

            _sheet=new RenderTarget2D(device,Width,Height);
            device.SetRenderTarget(_sheet);
            device.Clear(new Color(0, 0, 0, 0));
            _spriteBatch.Begin();
            Basic.DrawBox(_spriteBatch,Vector2.Zero,Width,Height,_backgroundColor,_boundsColor);

            foreach (var datasheetItem in Content)
            {
                switch (datasheetItem.Type)
                {
                    case DatasheetItem.Types.Text:
                        _spriteBatch.DrawString(_font,datasheetItem.Text,datasheetItem.Position,(Color)datasheetItem.Color);
                        break;
                    case DatasheetItem.Types.Symbol:
                        _spriteBatch.Draw(datasheetItem.Symbol,datasheetItem.Position,Color.White);
                        break;
                    case DatasheetItem.Types.Variable:
                        //are drawn everytime Draw is called
                        break;
                    
                    case DatasheetItem.Types.Color:
                        break;
                    case DatasheetItem.Types.NewLine:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            _spriteBatch.End();
            device.SetRenderTarget(null);
        }

        //TODO: Add table/accurat spacing
        private void CalculatePositions()
        {
            int additionalHeight = 3;
            int additionalWidth = 3;
            Vector2 defaultOffset = new Vector2(2,0);
            int maxLineWidth=0;
            int thisLineHeight = 0;
            Color currentColor = _fontColor;
            Vector2 cursorPosition = defaultOffset;

            foreach (var datasheetItem in Content)
            {
                datasheetItem.Position = cursorPosition;

                switch (datasheetItem.Type)
                {
                    case DatasheetItem.Types.Text:
                        cursorPosition += new Vector2(_font.SpriteFont.MeasureString(datasheetItem.Text).X,0);
                        thisLineHeight = Math.Max(_font.SpriteFont.LineSpacing, thisLineHeight);
                        if (datasheetItem.Color == null)
                            datasheetItem.Color = currentColor;
                        break;

                    case DatasheetItem.Types.Symbol:
                        cursorPosition+=new Vector2(datasheetItem.Symbol.Texture.Width,0);
                        thisLineHeight = Math.Max(datasheetItem.Symbol.Texture.Height, thisLineHeight);
                        break;
                    case DatasheetItem.Types.Variable:
                        cursorPosition += new Vector2(_font.SpriteFont.MeasureString(datasheetItem.Variable()).X, 0);
                        thisLineHeight = Math.Max(_font.SpriteFont.LineSpacing, thisLineHeight);
                        datasheetItem.VariableLength = datasheetItem.Variable().Length;
                        if (datasheetItem.Color == null)
                            datasheetItem.Color = currentColor;
                        break;
                    case DatasheetItem.Types.Color:
                        currentColor = (Color)datasheetItem.Color;
                        break;
                    case DatasheetItem.Types.NewLine:
                        maxLineWidth = Math.Max((int)cursorPosition.X, maxLineWidth);
                        cursorPosition = new Vector2(defaultOffset.X, cursorPosition.Y + thisLineHeight);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                Height = (int)cursorPosition.Y+additionalHeight;
                Width = maxLineWidth+additionalWidth;
                _contentHash = Content.GetHashCode();
            }
        }
    }
}
