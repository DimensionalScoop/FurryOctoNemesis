using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace ScoopFramework.Extensions
{
    public static class DeepCloneExtension
    {
        static BinaryFormatter formatter = new BinaryFormatter();

        public static T DeepClone<T>(this T obj)
        {
            using (var ms = new MemoryStream())
            {
                formatter.Serialize(ms, obj);
                ms.Position = 0;

                return (T)formatter.Deserialize(ms);
            }
        }
    }
}