using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
//using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Diagnostics;


namespace ScoopFramework.Input
{
    public enum KeyFunction
    {
        None,Backspace,Tab,Return
    }

    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class KeyProvider
    {
        KeyboardState currentKeyState;
        KeyboardState previousKeyState;

        string lastTextInput = "";
        string textInput = "";

        /// <summary>
        /// Keys that have been pressed and released since the last update call.
        /// </summary>
        public List<Keys> KeysStroked=new List<Keys>();
        /// <summary>
        /// Keys that are pressed right now.
        /// </summary>
        public List<Keys> KeysPressed=new List<Keys>();

        /// <summary>
        /// Contains all chars since the last update
        /// </summary>
        public string TextInput
        {
            get { return textInput; }
        }

        /// <summary>
        /// Invokes whenever a char is entered (thread safe code is necessary).
        /// </summary>
        public event Action<char, KeyFunction> TextEntered;

        /// <summary>
        /// Invokes whenever a char is entered, as soon as update is called.
        /// </summary>
        public event Action<char, KeyFunction> TextEnteredSync;

        /// <summary>
        /// Invokes whenever a key is pressed and released.
        /// </summary>
        public event Action<Keys> Keystroke;
        /// <summary>
        /// Invokes whenever a key is pressed down.
        /// </summary>
        public event Action<Keys> KeyWasPressed;


        internal KeyProvider(Game game)
        {
            EventInput.Initialize(game.Window.Handle);
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        internal void Initialize()
        {
            EventInput.CharEntered += EventInput_CharEntered;
        }

        void EventInput_CharEntered(object sender, CharacterEventArgs e)
        {
            lastTextInput += e.Character;

            if (TextEntered != null)
                TextEntered.Invoke(e.Character,FindKeyFunction(e.Character));

            lock (lastTextEntered)
            {
                lastTextEntered.Add(e);
            }
        }

        private KeyFunction FindKeyFunction(char p)
        {
            switch (p)
            {
                case '\b':
                    return KeyFunction.Backspace;
                case '\t':
                    return KeyFunction.Tab;
                case '\n':
                    return KeyFunction.Return;
                default:
                    return KeyFunction.None;
            }
        }

        private List<CharacterEventArgs> lastTextEntered = new List<CharacterEventArgs>();

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        internal void Update(GameTime gameTime)
        {
            previousKeyState = currentKeyState;
            currentKeyState = Keyboard.GetState();

            textInput = lastTextInput;
            lastTextInput = "";

            lock (lastTextEntered)
            {
                if (TextEnteredSync != null)
                    foreach (var e in lastTextEntered)
                    {
                        TextEnteredSync(e.Character, FindKeyFunction(e.Character));
                        textInput += e.Character;
                    }
                lastTextEntered.Clear();
            }

            KeysStroked.Clear();
            foreach (Keys Elem in previousKeyState.GetPressedKeys())
                if (currentKeyState.IsKeyUp(Elem))
                {
                    KeysStroked.Add(Elem);
                    KeysPressed.Remove(Elem);
                }
            if (Keystroke != null)
                KeysStroked.ForEach(Keystroke);
            
            foreach(Keys Elem in currentKeyState.GetPressedKeys())
                if (previousKeyState.IsKeyUp(Elem))
                {
                    KeysPressed.Add(Elem);
                    if (KeyWasPressed != null)
                        KeyWasPressed(Elem);
                }
        }
    }
}
