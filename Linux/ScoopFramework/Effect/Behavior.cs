using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScoopFramework.Effect
{
    public class Behavior
    {
        protected Action<GameTime, Effect> CalcBehavior;
        public bool DeleteFlag;
        protected TimeSpan MaxAge;
        protected readonly DateTime CreationTime;

        public Behavior(Action<GameTime, Effect> calcBehavior, TimeSpan maxAge)
        {
            CalcBehavior = calcBehavior;
            MaxAge = maxAge;
            CreationTime = DateTime.Now;
        }

        protected internal Behavior(TimeSpan maxAge)
        {
            MaxAge = maxAge;
            CreationTime = DateTime.Now;
        }

        void Delete() { DeleteFlag = true; }

        public void Update(GameTime gameTime, Effect effect)
        {
            if (DeleteFlag) return;
            CalcBehavior(gameTime, effect);
            if (CreationTime + MaxAge <= DateTime.Now) Delete();
        }
    }
}