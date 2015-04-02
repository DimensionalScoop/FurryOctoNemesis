using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ScoopFramework;
using ScoopFramework.Drawing;
using ScoopFramework.Input;
using ScoopFramework.Extensions;
using System.Diagnostics;

namespace ScoopFramework.Drawing
{
    public class ParticleProvider
    {
        public static ParticleProvider Default;
        public static Random Random = new Random();

        List<Particle> particles = new List<Particle>();
        public TimeSpan LastUpdate { get; private set; }
        public bool ParticlesToDelete;


        public ParticleProvider()
        {
            Debug.Assert(Default == null);
            Default = this;
        }

        public void AddParticle(Particle item)
        {
                particles.Add(item);
        }

        public void Update(GameTime gameTime)
        {
            LastUpdate = gameTime.TotalGameTime;
            if (ParticlesToDelete)
            {
                particles.RemoveAll(p => p.DeleteFlag);
                ParticlesToDelete = false;
            }
        }

        public void Draw(SpriteBatch spriteBatch,Vector2 camera, GameTime gameTime)
        {
            particles.ForEach(p => p.Draw(spriteBatch, camera, gameTime));
        }
    }
}
