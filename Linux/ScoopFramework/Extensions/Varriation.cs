using System;
using System.Collections.Generic;
using System.Linq;

namespace ScoopFramework.Extensions
{
    public class VarriationInt
    {
        static Random random = new Random();

        public int Base;
        public int Var;

        public VarriationInt(int baseValue, int var)
        {
            Base = baseValue;
            Var = var;
        }

        public int Random { get { return (int)(Base+random.Next(2*Var)-Var); } }
    }

    public class Varriation
    {
        static Random random = new Random();

        public float Base;
        public float Var;

        public Varriation(float baseValue, float var)
        {
            Base = baseValue;
            Var = var;
        }

        public float Random { get { return (float)(Base + random.NextFloat() * Var*2-Var); } }
    }
}