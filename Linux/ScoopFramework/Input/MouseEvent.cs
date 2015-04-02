using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ScoopFramework.Input
{
    public class MouseEvent
    {
        /// <summary>
        /// The rectangle the mouse must be in to trigger the event.
        /// </summary>
        public readonly Rectangle Location;
        /// <summary>
        /// The action that triggers the event.
        /// </summary>
        public readonly MouseActionType Action;
        /// <summary>
        /// Whether the event is deleted on being triggered.
        /// </summary>
        public readonly bool Permanent;
        /// <summary>
        /// Whether the Event should be deleted.
        /// </summary>
        public bool DeleteFlag;
        /// <summary>
        /// The function that is triggered.
        /// </summary>
        public readonly MouseEventDelegate Trigger;
        /// <summary>
        /// When false, the event will be deleted after one update.
        /// </summary>
        public bool Persistent;

        public MouseEvent(MouseActionType action, Microsoft.Xna.Framework.Rectangle location, MouseEventDelegate trigger, bool persistent = false, bool permanent = false)
        {
            Action = action;
            Location = location;
            Trigger = trigger;
            Persistent = persistent;
            Permanent = permanent;
            DeleteFlag = false;
        }

        public delegate void MouseEventDelegate(MouseEvent trigger);
    }
}
