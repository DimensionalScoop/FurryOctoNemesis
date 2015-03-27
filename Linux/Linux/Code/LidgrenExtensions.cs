using Lidgren.Network;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Linux.Code
{
    public static class LidgrenExtensions
    {
        public static void Write(this NetBuffer buffer, Color color)
        {
            buffer.Write(color.PackedValue);
        }

        public static Color ReadColor(this NetBuffer buffer)
        {
            var returnValue = new Color();
            returnValue.PackedValue = buffer.ReadUInt32();
            return returnValue;
        }
    }
}
