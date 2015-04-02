using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace ScoopFramework.Network
{
    public static class RawSerializer
    {
        public static T RawDeserialize<T>(byte[] rawData, int position, out int rawsize)
        {
            rawsize = Marshal.SizeOf(typeof(T));
            if (rawsize > rawData.Length)
                return default(T);

            IntPtr buffer = Marshal.AllocHGlobal(rawsize);
            Marshal.Copy(rawData, position, buffer, rawsize);
            T obj = (T)Marshal.PtrToStructure(buffer, typeof(T));
            Marshal.FreeHGlobal(buffer);
            return obj;
        }

        public static byte[] RawSerialize<T>(T item)
        {
            int rawSize = Marshal.SizeOf(typeof(T));
            IntPtr buffer = Marshal.AllocHGlobal(rawSize);
            Marshal.StructureToPtr(item, buffer, false);
            byte[] rawData = new byte[rawSize];
            Marshal.Copy(buffer, rawData, 0, rawSize);
            Marshal.FreeHGlobal(buffer);
            return rawData;
        }
    }
}
