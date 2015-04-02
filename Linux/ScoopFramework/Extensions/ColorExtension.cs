using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ScoopFramework.Extensions
{
    public static class ColorHelper
    {
        public static Color Add(Color value1, Color value2)
        {
            return new Color(value1.R + value2.R, value1.G + value2.G, value1.B + value2.B, value1.A + value2.A);
        }
        public static Color Substract(Color value1, Color value2)
        {
            return new Color(value1.R - value2.R, value1.G - value2.G, value1.B - value2.B, value1.A - value2.A);
        }
        public static Color Multiply(Color value1, Color value2)
        {
            return new Color(value1.R * value2.R, value1.G * value2.G, value1.B * value2.B, value1.A * value2.A);
        }
        public static Color Divide(Color value1, Color value2)
        {
            return new Color(value1.R / value2.R, value1.G / value2.G, value1.B / value2.B, value1.A / value2.A);
        }
        public static Color SmoothStep(this Color value1, Color value2, float amount)
        {
            return new Color(MathHelper.SmoothStep(value1.R, value2.R, amount), MathHelper.SmoothStep(value1.G, value2.G, amount), MathHelper.SmoothStep(value1.B, value2.B, amount), MathHelper.SmoothStep(value1.A, value2.A, amount));
        }
        public static Color Inverse(this Color value)
        {
            return new Color(1f - value.R, 1f - value.A, 1f - value.B, value.A);
        }
    }
}
