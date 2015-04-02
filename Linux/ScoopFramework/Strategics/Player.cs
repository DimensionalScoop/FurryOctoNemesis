using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using ScoopFramework.Strategics;

namespace ScoopFramework.Strategics
{
    [Flags]
    public enum Players:byte
    {
        None = 0x00,
        Red = 0x01,
        Blue = 0x02,
        Green = 0x04,
        Yellow = 0x08,
        Purple = 0x10,
        White = 0x20,
        Gray = 0x40
    }

    /// <summary>
    /// Contains all player-specific values like fraction number, color, points, etc.
    /// </summary>
    public class Player
    {
        public readonly Color Color;

        /// <summary>
        /// All completed research
        /// </summary>
        public List<Improvement> Improvements;

        public readonly Players Id;
        public Players Allys;

        public string Name { get { return Enum.GetName(typeof(Players), Id); } }

        private static int idCount;
        public readonly static Players[] playersEnumCount = new[] { Players.Red, Players.Blue, Players.Green, Players.Yellow, Players.Purple, Players.White, Players.Gray };
        public static Color[] PlayerColors = new[] { Color.Red, Color.Blue, Color.Green, Color.Yellow, Color.Purple, Color.White, Color.Gray };

        
        public Player()
        {
            Id = playersEnumCount[idCount];
            Color = PlayerColors[idCount];
            Allys |= Id;

            idCount++;
        }

        public Player(Players id)
        {
            Id = id;
            Color = PlayerColors[(byte)Id-1];
            Allys |= id;
        }

        public virtual void Update(GameTime gameTime){}

        public bool IsAlley(Player fraction)
        {
            return (this.Allys & fraction.Id) == fraction.Id;//XXX
        }

        public bool IsEnemy(Player fraction)
        {
            return !IsAlley(fraction);
        }
    }
}