﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linux.Code.SpaceShipSystem
{
    public class LandingSlot:Subsystem,ILandingSlot
    {
        public LandingSlot(string name)
            : base(3, 8, name)
        {

        }
    }
}
