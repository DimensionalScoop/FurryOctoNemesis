using System;
using Microsoft.Xna.Framework;

namespace ScoopFramework.Extensions
{
    public static class TimeExt
    {
        public static Vector2 Transform(this Vector2 origin,float angle,float lenght)
        {
            return new Vector2((float)Math.Sin(angle) * lenght, (float)Math.Cos(angle) * lenght) + origin;
        }
    }
}
