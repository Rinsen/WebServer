using System;
using Microsoft.SPOT;

namespace NetduinoSDCard
{
    public class ConsoleWrite  // todo this should be moved to a general library.
    {
        public static Object _PrintLock = new Object();
        public static Object _GCLock = new Object();

        public static void Print(string message)
        {
            lock (_PrintLock)
            {
                Debug.Print(message);
            }
        }

        public static uint MemoryCollect(bool force)
        {
            uint freeMemory = 0;
            lock (_GCLock)
            {
                freeMemory = Microsoft.SPOT.Debug.GC(force);
            }
            return freeMemory;
        }

        public static void CollectMemoryAndPrint(bool force, int id)
        {
            uint freeMemory = MemoryCollect(force);
            Print("Memory: " + freeMemory.ToString() + ", release workerid: " + id.ToString());

        }
    }
}
