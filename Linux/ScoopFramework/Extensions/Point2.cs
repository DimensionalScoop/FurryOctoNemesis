using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ScoopFramework
{
    public struct Point2
    {
        public int X;
        public int Y;
        public static readonly Point2 Zero = new Point2();
        public static readonly Point2 UnitX = new Point2(1, 0);
        public static readonly Point2 UnitY = new Point2(0, 1);

        public Point2(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        public static bool operator ==(Point2 value1, Point2 value2)
        {
            return ((value1.X == value2.X) && (value1.Y == value2.Y));
        }

        public static Point2 operator +(Point2 value1, Point2 value2)
        {
            return new Point2(value1.X + value2.X, value1.Y + value2.Y);
        }

        public static Point2 operator -(Point2 value1, Point2 value2)
        {
            return new Point2(value1.X - value2.X, value1.Y - value2.Y);
        }

        public static Point2 operator -(Point2 value1)
        {
            return new Point2(-value1.X, -value1.Y);
        }

        public static Point2 operator *(Point2 value1, Point2 value2)
        {
            return new Point2(value1.X * value2.X, value1.Y * value2.Y);
        }

        public static Point2 operator /(Point2 value1, Point2 value2)
        {
            return new Point2(value1.X / value2.X, value1.Y / value2.Y);
        }

        public static Point2 operator *(Point2 value1, int value2)
        {
            return new Point2(value1.X * value2, value1.Y * value2);
        }

        public static Point2 operator /(Point2 value1, int value2)
        {
            return new Point2(value1.X / value2, value1.Y / value2);
        }

        public static bool operator !=(Point2 value1, Point2 value2)
        {
            return !(value1 == value2);
        }

        public override int GetHashCode()
        {
            return (this.X.GetHashCode() + this.Y.GetHashCode());
        }

        public override string ToString()
        {
            return string.Format("{{X:{0} Y:{1}}}", this.X, this.Y);
        }

        public static implicit operator Vector2(Point2 n) { return new Vector2(n.X, n.Y); }
    }
}