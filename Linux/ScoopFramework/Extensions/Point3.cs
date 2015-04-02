using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ScoopFramework
{
    public struct Point3
    {
        public int X;
        public int Y;
        public int Z;
        public static readonly Point3 Zero = new Point3();
        public static readonly Point3 UnitXYZ = new Point3(1, 1, 1);

        public Point3(int x, int y, int z=0)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        public Point3(Point2 point, int z = 0) : this(point.X, point.Y, z) { }

        public static bool operator ==(Point3 value1, Point3 value2)
        {
            return ((value1.X == value2.X) && (value1.Y == value2.Y) && (value1.Z == value2.Z));
        }

        public static Point3 operator +(Point3 value1, Point3 value2)
        {
            return new Point3(value1.X + value2.X, value1.Y + value2.Y, value1.Z + value2.Z);
        }

        public static Point3 operator -(Point3 value1, Point3 value2)
        {
            return new Point3(value1.X - value2.X, value1.Y - value2.Y, value1.Z - value2.Z);
        }

        public static Point3 operator *(Point3 value1, Point3 value2)
        {
            return new Point3(value1.X * value2.X, value1.Y * value2.Y, value1.Z * value2.Z);
        }

        public static Point3 operator %(Point3 value1, Point3 value2)
        {
            return new Point3(value1.X % value2.X, value1.Y % value2.Y, value1.Z % value2.Z);
        }

        public static Point3 operator /(Point3 value1, Point3 value2)
        {
            return new Point3(value1.X / value2.X, value1.Y / value2.Y, value1.Z / value2.Z);
        }

        public static Point3 operator *(Point3 value1, int value2)
        {
            return new Point3(value1.X * value2, value1.Y * value2, value1.Z * value2);
        }

        public static Point3 operator /(Point3 value1, int value2)
        {
            return new Point3(value1.X / value2, value1.Y / value2, value1.Z / value2);
        }

        public static bool operator !=(Point3 value1, Point3 value2)
        {
            return !(value1 == value2);
        }

        public override int GetHashCode()
        {
            return (this.X.GetHashCode() + this.Y.GetHashCode() + this.Z.GetHashCode());
        }

        public override string ToString()
        {
            return string.Format("{{X:{0} Y:{1} Z:{2}}}", this.X, this.Y, this.Z);
        }

        public static implicit operator Vector3(Point3 n) { return new Vector3(n.X, n.Y, n.Z); }
        public static explicit operator Point3(Vector3 n) { return new Point3((int)n.X, (int)n.Y, (int)n.Z); }

        public static Point3 Upcast(Vector3 n) { return new Point3((int)Math.Round(n.X, MidpointRounding.AwayFromZero), (int)Math.Round(n.Y, MidpointRounding.AwayFromZero), (int)Math.Round(n.Z, MidpointRounding.AwayFromZero)); }
    }

    public struct SPoint3
    {
        public short X;
        public short Y;
        public short Z;
        public static readonly SPoint3 Zero = new SPoint3();

        public SPoint3(int x, int y, int z = 0)
        {
            if (x > short.MaxValue || y > short.MaxValue || z > short.MaxValue ||
                x < short.MinValue || y < short.MinValue || z < short.MinValue)
                throw new ArgumentOutOfRangeException();

            this.X = (short)x;
            this.Y = (short)y;
            this.Z = (short)z;
        }

        public static bool operator ==(SPoint3 value1, SPoint3 value2)
        {
            return ((value1.X == value2.X) && (value1.Y == value2.Y) && (value1.Z == value2.Z));
        }

        public static SPoint3 operator +(SPoint3 value1, SPoint3 value2)
        {
            return new SPoint3(value1.X + value2.X, value1.Y + value2.Y, value1.Z + value2.Z);
        }

        public static SPoint3 operator -(SPoint3 value1, SPoint3 value2)
        {
            return new SPoint3(value1.X - value2.X, value1.Y - value2.Y, value1.Z - value2.Z);
        }

        public static SPoint3 operator *(SPoint3 value1, SPoint3 value2)
        {
            return new SPoint3(value1.X * value2.X, value1.Y * value2.Y, value1.Z * value2.Z);
        }

        public static SPoint3 operator %(SPoint3 value1, SPoint3 value2)
        {
            return new SPoint3(value1.X % value2.X, value1.Y % value2.Y, value1.Z % value2.Z);
        }

        public static SPoint3 operator /(SPoint3 value1, SPoint3 value2)
        {
            return new SPoint3(value1.X / value2.X, value1.Y / value2.Y, value1.Z / value2.Z);
        }

        public static SPoint3 operator *(SPoint3 value1, int value2)
        {
            return new SPoint3(value1.X * value2, value1.Y * value2, value1.Z * value2);
        }

        public static SPoint3 operator /(SPoint3 value1, int value2)
        {
            return new SPoint3(value1.X / value2, value1.Y / value2, value1.Z / value2);
        }

        public static bool operator !=(SPoint3 value1, SPoint3 value2)
        {
            return !(value1 == value2);
        }

        public override int GetHashCode()
        {
            return (this.X.GetHashCode() + this.Y.GetHashCode() + this.Z.GetHashCode());
        }

        public override string ToString()
        {
            return string.Format("{{X:{0} Y:{1} Z:{2}}}", this.X, this.Y, this.Z);
        }

        public static implicit operator Vector3(SPoint3 n) { return new Vector3(n.X, n.Y, n.Z); }
    }
}