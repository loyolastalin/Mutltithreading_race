using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MultheradingConcepts
{
    public class ConfigurationManager
    {
        private bool isLoaded = false;
        private object locker = new object();

        public int LoadedCounter { get; private set; }

        public void Singleton()
        {
            lock (locker)
            {
                if (!isLoaded)
                {
                    Console.WriteLine(string.Format("{0} updated at {1}.", Thread.CurrentThread.Name, System.DateTime.Now.ToLongTimeString()));
                    LoadedCounter++;
                    isLoaded = true;
                }
            }
        }

        public int UpdateDataWithLocker()
        {
            lock (locker)
            {
                Console.WriteLine(string.Format("{0} updated at {1}.", Thread.CurrentThread.Name, System.DateTime.Now.ToLongTimeString()));
                LoadedCounter++;
                return LoadedCounter;
            }
        }
        
        public void UpdateDataWithOutLocker()
        {
            Console.WriteLine(string.Format("{0} updated at {1}.", Thread.CurrentThread.Name, System.DateTime.Now.ToLongTimeString()));
            Thread.MemoryBarrier();
            LoadedCounter++;
        }

        public int GetDataWithOutLocker()
        {
            Console.WriteLine(string.Format("{0} updated at {1}.", Thread.CurrentThread.Name, System.DateTime.Now.ToLongTimeString()));
            Thread.MemoryBarrier();
            return LoadedCounter;
        }

        public void UpdateDataWithLocker(int a)
        {
            lock (locker)
            {
                Console.WriteLine(string.Format("{0} updated at {1}.", Thread.CurrentThread.Name, System.DateTime.Now.ToLongTimeString()));
                Thread.MemoryBarrier();
                LoadedCounter=a;
            }
        }

        public int GetDataWithLocker()
        {
            lock (locker)
            {
                Console.WriteLine(string.Format("{0} updated at {1}.", Thread.CurrentThread.Name, System.DateTime.Now.ToLongTimeString()));
                Thread.MemoryBarrier();
                return LoadedCounter;
            }
        }
    }
}
