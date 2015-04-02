using System;

namespace ScoopFramework.Extensions
{
    public static class RandomExt
    {
        public static float NextFloat(this Random random)
        {
            return (float) random.NextDouble();
        }

        /// <summary>
        /// Returns flase in 50% of all cases, otherwise true.
        /// </summary>
        /// <param name="random"></param>
        /// <returns></returns>
        public static bool Coin(this Random random)
        {
            return random.Next(2) == 0;
        }

        /// <summary>
        /// Returns -1 in 50% of all cases, otherwise 1.
        /// </summary>
        /// <param name="random"></param>
        /// <returns></returns>
        public static int CoinMux(this Random random)
        {
            return random.Next(2) == 0 ? -1 : 1;
        }

        public static float Next(this Random random,float maxValue)
        {
            return Next(random, 0, maxValue);
        }

        public static float Next(this Random random,float minValue,float maxValue)
        {
            return (float)(random.NextDouble()*(maxValue-minValue)+minValue);
        }

        public static T NextElement<T>(this Random random, T[] array)
        {
            return array[random.Next(array.Length)];
        }
    }
}
