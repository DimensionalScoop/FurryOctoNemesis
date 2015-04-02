using System;
using Microsoft.Xna.Framework;

namespace ScoopFramework.Extensions
{
    public static class VectorExt
    {
        public static Vector2 Transform(this Vector2 origin,float angle,float lenght)
        {
            return new Vector2((float)Math.Cos(angle) * lenght, (float)Math.Sin(angle) * lenght) + origin;
        }

        public static Vector2 Round(this Vector2 inp)
        {
            return new Vector2((int)inp.X, (int)inp.Y);
        }

        public static float Angle(this Vector2 origin, Vector2 item)
        {
            return (float)Math.Atan2(origin.Y - item.Y, origin.X - item.X);
        }

        public static Vector3 ToInt(this Vector3 item)
        {
            return new Vector3((int)item.X, (int)item.Y, (int)item.Z);
        }

        public static Vector2 Rotate(this Vector2 inp, float angle)
        {
            angle += inp.Angle(Vector2.Zero);
            var lenght=inp.Length();
            return new Vector2((float)Math.Cos(angle) * lenght, (float)Math.Sin(angle) * lenght);
        }

        public static Point3 ToPoint3(this Vector3 item) { return new Point3((int)item.X, (int)item.Y, (int)item.Z); }
    }
}
