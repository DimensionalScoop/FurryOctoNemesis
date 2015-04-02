using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ScoopFramework;
using ScoopFramework.Drawing;
using ScoopFramework.Effect;
using ScoopFramework.Input;
using ScoopFramework.Extensions;

namespace ScoopFramework.Network
{
    public static class NetExtension
    {
        public static void WriteT<T>(this NetOutgoingMessage msg, T data)
        {
            var type = typeof(T);

            if (type == typeof(string))
            {
                msg.Write(data as string);
            }
            else if (type == typeof(Vector2))
            {
                msg.Write((data as Vector2?).Value.X);
                msg.Write((data as Vector2?).Value.Y);
            }
            else if (type == typeof(Guid))
            {
                msg.Write((data as Guid?).Value.ToByteArray());
            }
            else
            {
                msg.Write(RawSerializer.RawSerialize(data));
            }
        }

        public static T ReadT<T>(this NetIncomingMessage msg)
        {
            var type = typeof(T);

            if (type == typeof(string))
            { 
                return (T)(object)msg.ReadString(); 
            }
            else if (type == typeof(Vector2))
            {
                return (T)(object)new Vector2(msg.ReadFloat(), msg.ReadFloat());
            }
            else if (type == typeof(Guid))
            {
                return (T)(object)new Guid(msg.ReadBytes(16));
            }
            else
            {
                int readBytes;
                T returnValue = RawSerializer.RawDeserialize<T>(msg.Data, msg.PositionInBytes, out readBytes);
                msg.Position += readBytes * 8;
                return returnValue;
            }
        }
    }
}
