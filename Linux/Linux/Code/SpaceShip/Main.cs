﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Linux.Code.SpaceShipSystem
{
    public class Main
    {
        public List<Subsystem> Subsystems = new List<Subsystem>();
        
        public bool DeleteFlag;
        public Fraction Fraction;
        public float MaxAvailableEnergy { get { return FindAllSubsystems<IGenerator>().Sum(p => p.EnergyGeneration); } }
        public float AssignedEnergy;
        public float CurrentAvailableEnergy { get { return MaxAvailableEnergy - AssignedEnergy; } }

        public Vector2 Position;
        public float Heading;


        public void AddSubsystem(Subsystem item)
        {
            Subsystems.Add(item);
            Subsystems.Sort(new Comparison<Subsystem>((p, q) => q.Priority - p.Priority));//high priority first
        }


        public Interface FindSubsystem<Interface>(int maxPriority=int.MaxValue)
            where Interface: class
        {
            var returnValue=Subsystems.Find(p => p is Interface && p.Priority <= maxPriority);
            if (returnValue == null) throw new Exception("Ship interface data not handled!");
            return returnValue as Interface;
        }

        public List<Interface> FindAllSubsystems<Interface>()
            where Interface : class
        {
            var returnValue = Subsystems.FindAll(p => p is Interface);
            return returnValue.Select(p => p as Interface).ToList();
        }

        /// <summary>
        /// Returns list of all subsystems with specified interface, ordered by priority.
        /// </summary>
        /// <typeparam name="Interface"></typeparam>
        /// <returns></returns>
        public List<Subsystem> FindAllPrioritySubsystems<Interface>()
            where Interface : class
        {
            var returnValue = Subsystems.FindAll(p => p is Interface);
            return returnValue;
        }

        public void Destroy(string reason)
        {
            DeleteFlag = true;
        }


        public void Update(GameTime gameTime)
        {
            Subsystems.FindAll(p=>p.Online).ForEach(p =>p.Update(gameTime));
        }
    }
}