using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScoopFramework.Drawing;
using ScoopFramework.Extensions;

namespace ScoopFramework.Interface
{
    public partial class ExtendedString
    {
        public void Write(string text, Color color) { Write(() => text, () => color); }
        public void Write(string text) { Write(() => text, () => DefaultColor); }
        public void AddBarGraph(int charLenght, Func<float> value, Func<Color> filledColor = null, Func<Color> emptyColor = null) { AddBarGraph(() => charLenght, value, filledColor, emptyColor); }
    }
}
