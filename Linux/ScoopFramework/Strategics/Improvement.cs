using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScoopFramework.Drawing;
using ScoopFramework.Strategics;

namespace ScoopFramework.Strategics
{
    public class Improvement
    {
        public Sprite Icon;
        public string Name;
        public string Description;
        public Resource CostPerSecond;
        public TimeSpan Duration;
        public bool IsAvailable
        {
            get { return Duration == TimeSpan.Zero; }
        }
    }
}