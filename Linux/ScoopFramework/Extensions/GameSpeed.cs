using Microsoft.Xna.Framework;

namespace ScoopFramework.Extensions
{
    public static class GameSpeed
    {
        /// <summary>
        /// Returns the speed of the game in percent. When the game runs slow, it returns a value less than 100%.
        /// </summary>
        /// <param name="gameTime"></param>
        /// <returns></returns>
        public static int GetSpeed(this GameTime gameTime,Game game)
        {
            return (int)(100*gameTime.ElapsedGameTime.TotalMilliseconds / game.TargetElapsedTime.TotalMilliseconds);
        }

        /// <summary>
        /// Time passed since last update in seconds.
        /// </summary>
        /// <param name="gameTime"></param>
        /// <returns></returns>
        public static float GetDelay(this GameTime gameTime)
        {
            return (float)(gameTime.ElapsedGameTime.TotalSeconds);
        }
    }
}
