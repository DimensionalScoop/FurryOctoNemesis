using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using X45Game.Input;
using X45Game.Strategics;

namespace X45Game.Drawing
{
    public class InputItem
    {
        public readonly string Caption;
        public readonly string Description;
        public readonly Resource ResourceInfo;
        public readonly Action<InputItem> Reaction;

        public InputItem(string caption, string description, Resource resourceInfo, Action<InputItem> reaction)
        {
            Caption = caption;
            Description = description;
            ResourceInfo = resourceInfo;
            Reaction = reaction;
        }
    }

    public class DynInput:DrawableGameComponent
    {
        Sprite _inputBar = new Sprite("resources\\input_bar.png");
        Sprite _selectionBar = new Sprite("resources\\sel_bar_small.png");
        Sprite _selectedSelectionBar = new Sprite("resources\\sel_sel_bar_small.png");
        Sprite _selectionArrow=new Sprite("resources\\arrow.png");
        Sprite _detailWindow=new Sprite("resources\\detail.png");

        Vector2 _position;
        InputProvider _input;
        private SpriteBatch _spriteBatch;
        Font _fontNormal=new Font("font1");
        Font _fontSmall = new Font("font2");

        bool _isActivated;
        string _userInput;
        private int _selectedItem;
        private Action<InputItem> _returnFunction;
        private int _listScroll;

        private const int MaxVisableItems = 15;


        List<InputItem> _items=new List<InputItem>();
         List<InputItem> _filteredItems=new List<InputItem>();


        public DynInput(Game game,Vector2 position) : base(game)
        {
            _position = position;
        }

        public override void Initialize()
        {
            _input = (InputProvider)Game.Services.GetService(typeof(InputProvider));
            _input.Key.TextEnteredSync += Key_TextEntered;
            _input.Key.Keystroke += Key_Keystroke;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch=new SpriteBatch(GraphicsDevice);
            base.LoadContent();
        }
        

        public void Call(List<InputItem> newCommands,Action<InputItem> returnFunction)
        {
            _items = newCommands;
            _filteredItems = _items;
            _selectedItem = 0;
            _isActivated = true;
            _returnFunction = returnFunction;
        }

        private void ResortList()
        {
            var formerSelected = _filteredItems[_selectedItem];

            _filteredItems.Clear();
            _filteredItems.AddRange(_items.FindAll(p=>p.Caption.ToLower().StartsWith(_userInput)));//Select items which start with _userInput
            _filteredItems.AddRange(_items.FindAll(p => p.Caption.ToLower().Contains(_userInput)));//Select items which contain _userInput
            _filteredItems.AddRange(_items.FindAll(p => p.Caption.Select(char.IsUpper).ToString().StartsWith(_userInput)));//Select items which upper letters are contained in _userInput
            _filteredItems.AddRange(_items.FindAll(p=>p.Caption.ToLower().Count(q=>_userInput.Contains(q))>=_userInput.Length));//Select items which contain all letters of _userInput

            _selectedItem = _filteredItems.Contains(formerSelected) ? _filteredItems.FindIndex(p => p==formerSelected) : 0;
        }
        
        private void TakeSelected()
        {
            _isActivated = false;
            _filteredItems.Clear();
            _items.Clear();
            _returnFunction(_filteredItems[_selectedItem]);
        }

        public override void Update(GameTime gameTime)
        {

            base.Update(gameTime);
        }


        void Key_Keystroke(Keys key)
        {
            switch (key)
            {
                case Keys.Up:
                    _selectedItem--;
                    if (_selectedItem < 0)
                        _selectedItem = _filteredItems.Count - 1;
                    break;
                case Keys.Down:
                    _selectedItem++;
                    if (_selectedItem > _filteredItems.Count - 1)
                        _selectedItem = 0;
                    break;
            }
        }

        void Key_TextEntered(char c,KeyFunction function)
        {
            if (!_isActivated) return;

            switch (function)
            {
                case KeyFunction.Backspace:
                    _userInput = _userInput.Length > 0 ? _userInput.Take(_userInput.Length - 1).ToString() : "";
                    ResortList();
                    break;
                        
                case KeyFunction.Tab:
                case KeyFunction.Return:
                    TakeSelected();
                    break;
                        
                default:
                    _userInput += char.ToLower(c);
                    ResortList();
                    break;        
            }
        }

        public override void Draw(GameTime gameTime)
        {
            if(!_isActivated) return;
            
            _spriteBatch.Begin();
            _spriteBatch.Draw(_inputBar,_position,Color.White);
            for (int i = 0; i < Math.Min(_filteredItems.Count,MaxVisableItems); i++)
            {
                var elem = _filteredItems[i];
                int yPos = (int) _position.Y + _inputBar.Texture.Height + _selectionBar.Texture.Height*i;
                Vector2 position = new Vector2(_position.X, yPos);

                _spriteBatch.Draw(_selectionBar,position,Color.White);
                if(_selectedItem==i)
                    _spriteBatch.Draw(_selectedSelectionBar,position,Color.White);

                _spriteBatch.DrawString(_fontNormal, elem.Caption, position, Color.White);
                _spriteBatch.DrawString(_fontSmall, elem.Description,
                                        position + new Vector2(0, _fontNormal.SpriteFont.LineSpacing), Color.White);

                if (_selectedItem == i)
                {
                    _spriteBatch.Draw(_selectionArrow,position-new Vector2(_selectionArrow.Texture.Width,0),Color.White);
                    _spriteBatch.Draw(_detailWindow,position-new Vector2(_detailWindow.Texture.Width,0),Color.White);
                }
            }

            base.Draw(gameTime);
        }
    }
}
