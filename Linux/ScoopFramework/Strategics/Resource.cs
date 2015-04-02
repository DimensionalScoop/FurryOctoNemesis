using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.Xna.Framework;
using ScoopFramework.Drawing;

namespace ScoopFramework.Strategics
{
    public class Resource
    {
        public class MatterProperties
        {
            public readonly int Id;
            public readonly Sprite Icon;
            public readonly string Name;
            public readonly string Description;
            public readonly Color Color;

            public MatterProperties(int id, Sprite icon,Color color, string name, string description)
            {
                Id = id;
                Icon = icon;
                Color = color;
                Name = name;
                Description = description;
            }
        }

        private static readonly List<MatterProperties> _DefaultMatters;
        public static ReadOnlyCollection<MatterProperties> DefaultMatters;

        static Resource()
        {
            _DefaultMatters=new List<MatterProperties>();
            DefaultMatters=new ReadOnlyCollection<MatterProperties>(_DefaultMatters);
        }

        public static void AddDefaultMatter(MatterProperties item)
        {
            _DefaultMatters.Add(item);
        }



        private readonly float[] _matters;

        public float[] Matters { get { return _matters; } }

        private static float[] GenerateEmptyMattersArray()
        {
            float[] returnValue = new float[_DefaultMatters.Count];
            for (int i = 0; i < returnValue.Length; i++)
                returnValue[i] = 0;
            return returnValue;
        }

        public Resource(Dictionary<string, float> amount)
            : this()
        {
            foreach (var elem in amount)
                _matters[_DefaultMatters.Find(p => p.Name == elem.Key).Id] = elem.Value;
        }

        public Resource(params float[] amount)
            : this()
        {
            amount.CopyTo(_matters, 0);
        }

        public Resource()
        {
            _matters = GenerateEmptyMattersArray();
        }


        public override string ToString()
        {
            string returnValue = "";
            for (int i = 0; i < _matters.Length; i++)
            {
                if (_matters[i] != float.NaN)
                {
                    returnValue += Math.Round(_matters[i], 2) + "t " + _DefaultMatters[i].Name + "\n";
                }
            }

            if (returnValue == "")
                returnValue = "None";

            return returnValue;
        }

        public string ToComparison(Resource comparator)
        {
            string returnValue = "";
            for (int i = 0; i < _DefaultMatters.Count; i++)
            {
                if (_matters[i] == float.NaN || comparator._matters[i] == float.NaN) continue;
                
                var first = _matters[i];
                var second = comparator._matters[i];
                if (first == float.NaN)
                    first = 0;
                else if (second == float.NaN)
                    second = 0;

                returnValue += Math.Round(first, 2) + "/" + Math.Round(second, 2) + "t " +
                               _DefaultMatters[i].Name + "\n";
            }

            if (returnValue == "")
                returnValue = "None";

            return returnValue;
        }

        public Resource Negate()
        {
            var returnValue = new Resource(_matters);
            for (int i = 0; i < returnValue._matters.Length; i++)
            {
                returnValue._matters[i] = -returnValue._matters[i];
            }
            return returnValue;
        }

        public Resource PositiveValuesOnly()
        {
            var returnValue = new Resource(_matters);
            for (int i = 0; i < returnValue._matters.Length; i++)
            {
                if (returnValue._matters[i] < 0)
                    returnValue._matters[i] = 0;
            }
            return returnValue;
        }

        public bool AnyNegative()
        {
            return _matters.Any(t => t < 0);
        }

        public bool Contains(Resource amount)
        {
            for (int i = 0; i < amount._matters.Length; i++)
                if (this._matters[i] < amount._matters[i])
                    return false;
            return true;
        }

        public void Clamp(Resource maxResource)
        {
            for (int i = 0; i < maxResource._matters.Length; i++)
                if (this._matters[i] > maxResource._matters[i])
                    this._matters[i] = maxResource._matters[i];
        }

        public static Resource operator +(Resource a, Resource b)
        {
            var returnValue = new Resource(a._matters);

            for (int i = 0; i < returnValue._matters.Length; i++)
            {
                returnValue._matters[i] += b._matters[i];
            }

            return returnValue;
        }

        public static Resource operator -(Resource a, Resource b)
        {
            var returnValue = new Resource(a._matters);

            for (int i = 0; i < returnValue._matters.Length; i++)
            {
                returnValue._matters[i] -= b._matters[i];
            }

            return returnValue;
        }

        public static Resource operator *(Resource a, float b)
        {
            var returnValue = new Resource(a._matters);

            for (int i = 0; i < returnValue._matters.Length; i++)
            {
                returnValue._matters[i] *= b;
            }

            return returnValue;
        }

        public static Resource operator /(Resource a, float b)
        {
            return new Resource(a._matters) * (1 / b);
        }
    }
}
