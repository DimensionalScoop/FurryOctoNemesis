using Lidgren.Network;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Linux.Code
{
    public class Player
    {
        public Guid ID = Guid.NewGuid();
        public Color Color = Color.Black;
        public byte NetID = 0;

        public static readonly Color[] Colors = {Color.Red,Color.Blue,Color.Green,Color.Orange,Color.Yellow,Color.LightBlue,Color.Pink };

        public void ReadFromStream(NetBuffer buffer)
        {
            ID = Guid.Parse(buffer.ReadString());
            Color = buffer.ReadColor();
            NetID = buffer.ReadByte();
        }

        public void WriteToStream(NetBuffer buffer)
        {
            buffer.Write(ID.ToString());//XXX: quite inefficient ID transfer
            buffer.Write(Color);
            buffer.Write(NetID);
        }
    }
}
