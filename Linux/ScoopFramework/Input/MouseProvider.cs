using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace ScoopFramework.Input
{
    public class MouseProvider
    {
        MouseState currentMouseState;
        MouseState previousMouseState;

        List<MouseEvent> events = new List<MouseEvent>();

        Vector2 position;
        Vector2 positionDelta;
        List<MouseButtons> pressedButtons = new List<MouseButtons>();
        List<MouseButtons> clicks = new List<MouseButtons>();
        List<MouseButtons> startDrags = new List<MouseButtons>();
        int scrollWheelDelta;

        public Vector2 Position
        {
            get
            {
                return position;
            }
        }

        public bool ContainsPosition(Vector2 leftTopCorner, Vector2 rightBottomCorner)
        {
            return Position.X >= leftTopCorner.X && Position.Y >= leftTopCorner.Y &&
                   Position.X <= rightBottomCorner.X && Position.Y <= rightBottomCorner.Y;
        }
        public bool ContainsPosition(float x, float y, float width, float height)
        {
            return ContainsPosition(new Vector2(x, y), new Vector2(x + width, y + height));
        }

        public Point IntPosition
        {
            get
            {
                return new Point((int)position.X,(int)position.Y);
            }
        }

        public List<MouseButtons> PressedButtons
        {
            get
            {
                return pressedButtons;
            }
        }

        public List<MouseButtons> StartDrags
        {
            get
            {
                return startDrags;
            }
        }

        public Vector2 PositionDelta
        {
            get
            {
                return positionDelta;
            }
        }

        public List<MouseButtons> Clicks
        {
            get
            {
                return clicks;
            }
        }

        public int ScrollWheelDelta
        {
            get
            {
                return scrollWheelDelta;
            }
        }

        /// <summary>
        /// Creates a new event.
        /// </summary>
        public MouseEvent Register(MouseEvent item)
        {
            events.Add(item);
            return item;
        }

        internal void Update()
        {
            previousMouseState = currentMouseState;
            currentMouseState = Mouse.GetState();

            pressedButtons.Clear();
            if (currentMouseState.LeftButton == ButtonState.Pressed)
                pressedButtons.Add(MouseButtons.Left);
            if (currentMouseState.RightButton == ButtonState.Pressed)
                pressedButtons.Add(MouseButtons.Right);
            if (currentMouseState.MiddleButton == ButtonState.Pressed)
                pressedButtons.Add(MouseButtons.Middle);

            startDrags.Clear();
            if (currentMouseState.LeftButton == ButtonState.Pressed && previousMouseState.LeftButton == ButtonState.Released)
                startDrags.Add(MouseButtons.Left);
            if (currentMouseState.RightButton == ButtonState.Pressed && previousMouseState.RightButton == ButtonState.Released)
                startDrags.Add(MouseButtons.Right);
            if (currentMouseState.MiddleButton == ButtonState.Pressed && previousMouseState.MiddleButton == ButtonState.Released)
                startDrags.Add(MouseButtons.Middle);

            scrollWheelDelta = currentMouseState.ScrollWheelValue - previousMouseState.ScrollWheelValue;

            clicks.Clear();
            if (currentMouseState.LeftButton == ButtonState.Released && previousMouseState.LeftButton == ButtonState.Pressed)
                clicks.Add(MouseButtons.Left);
            if (currentMouseState.RightButton == ButtonState.Released && previousMouseState.RightButton == ButtonState.Pressed)
                clicks.Add(MouseButtons.Right);
            if (currentMouseState.MiddleButton == ButtonState.Released && previousMouseState.MiddleButton == ButtonState.Pressed)
                clicks.Add(MouseButtons.Middle);

            position = new Vector2(currentMouseState.X, currentMouseState.Y);
            positionDelta = position - new Vector2(previousMouseState.X, previousMouseState.Y);

            UpdateEvents();
        }

        private void UpdateEvents()
        {
            events.RemoveAll(p => p.DeleteFlag);
            events.RemoveAll(p => !p.Persistent);
            for (int i = 0; i < events.Count; i++)
                if (events[i].Location.Contains(IntPosition))
                    if (ActionHappend(events[i].Action))
                    {
                        events[i].Trigger(events[i]);
                        if(!events[i].Permanent)
                        events[i].DeleteFlag = true;
                    }
        }

        private bool ActionHappend(MouseActionType action)
        {
            switch (action)
            {
                case MouseActionType.LeftButtonPressed:
                    if (PressedButtons.Contains(MouseButtons.Left))
                        return true;
                    return false;
                case MouseActionType.RightButtonPressed:
                    if (PressedButtons.Contains(MouseButtons.Right))
                        return true;
                    return false;
                case MouseActionType.MiddleButtonPressed:
                    if (PressedButtons.Contains(MouseButtons.Middle))
                        return true;
                    return false;

                case MouseActionType.LeftClick:
                    if (Clicks.Contains(MouseButtons.Left))
                        return true;
                    return false;
                case MouseActionType.RightClick:
                    if (Clicks.Contains(MouseButtons.Right))
                        return true;
                    return false;
                case MouseActionType.MiddleClick:
                    if (Clicks.Contains(MouseButtons.Middle))
                        return true;
                    return false;

                case MouseActionType.Located:
                        return true;
                case MouseActionType.Movement:
                        if (PositionDelta != Vector2.Zero)
                            return true;
                        return false;

                case MouseActionType.ScrollWheel:
                        if (ScrollWheelDelta != 0)
                            return true;
                        return false;

                default:
                        throw new NotImplementedException();
            }
        }
    }
}
