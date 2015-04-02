//
//NUCLEX does not work well with monogame

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using Nuclex.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ScoopFramework;
using ScoopFramework.Drawing;
using ScoopFramework.Input;
using ScoopFramework.Extensions;

namespace ScoopFramework.Input
{
    public enum GamePadButtons { A, B, X, Y, Up, Down, Left, Right, LB, RB, LT, RT, Start, Back, Digi1Up, Digi1Down, Digi1Left, Digi1Right, Digi2Up, Digi2Down, Digi2Left, Digi2Right, Digi1Button, Digi2Button }

    public class GamePadProvider
    {
        //InputManager input;

        public List<GamePadButtons[]> Controllers = new List<GamePadButtons[]>();
        private Game Game;



        public GamePadProvider(Game Game)
        {
            this.Game = Game;
        }

        internal void Initialize()
        {
            //input = new InputManager(Game.Services, Game.Window.Handle);
        }

        internal void Update(GameTime gameTime)
        {
        //    if (input != null)
        //    {
        //        input.Update();

        //        Controllers.Clear();
        //        for (int i = 0; i < 8; i++)
        //        {
        //            if (input.GetGamePad((ExtendedPlayerIndex)i).IsAttached)
        //            {
        //                var state = input.GetGamePad((ExtendedPlayerIndex)i).GetState();

        //            }
        //        }
        //    }
        }
    }
}