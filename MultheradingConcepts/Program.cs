using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MultheradingConcepts
{
    class Program
    {
        static AutoResetEvent autoReset1 = new AutoResetEvent(false);
        static AutoResetEvent autoReset2 = new AutoResetEvent(false);
        static ConfigurationManager configManager = new ConfigurationManager();

        static void Main(string[] args)
        {
            Thread t1 = new Thread(new ThreadStart(SingletonFirst));
            t1.Name = "First Thread";
            Thread t2 = new Thread(new ThreadStart(SingletonSecond));
            t2.Name = "Second Thread";

            t1.Start();
            t2.Start();

            autoReset1.Set();
            autoReset2.Set();
            
        }

        static void SingletonFirst()
        {
            Console.WriteLine(string.Format("{0} called at {1}.", Thread.CurrentThread.Name, System.DateTime.Now.ToLongTimeString()));
            configManager.Singleton();
            Console.WriteLine(configManager.LoadedCounter.ToString());
            autoReset1.WaitOne();
        }


        static void SingletonSecond()
        {
            Console.WriteLine(string.Format("{0} called at {1}.", Thread.CurrentThread.Name, System.DateTime.Now.ToLongTimeString()));
            configManager.Singleton();
            Console.WriteLine(configManager.LoadedCounter.ToString());
            autoReset2.WaitOne();
        }

        static void UpdatWithoutLock()
        {
            Console.WriteLine(string.Format("{0} called at {1}.", Thread.CurrentThread.Name, System.DateTime.Now.ToLongTimeString()));
            configManager.UpdateDataWithOutLocker();
            autoReset1.WaitOne();
        }


        static void GetWithoutLock()
        {
            Console.WriteLine(string.Format("{0} called at {1}.", Thread.CurrentThread.Name, System.DateTime.Now.ToLongTimeString()));
            int count = configManager.GetDataWithOutLocker();
            Console.WriteLine(count.ToString());
            autoReset2.WaitOne();
        }

        static void UpdateWithLock()
        {
            Console.WriteLine(string.Format("{0} called at {1}.", Thread.CurrentThread.Name, System.DateTime.Now.ToLongTimeString()));
            configManager.UpdateDataWithLocker(4);
            autoReset1.WaitOne();
        }


        static void GetWithLock()
        {
            Console.WriteLine(string.Format("{0} called at {1}.", Thread.CurrentThread.Name, System.DateTime.Now.ToLongTimeString()));
            int count = configManager.GetDataWithLocker();
            Console.WriteLine(count.ToString());
            autoReset2.WaitOne();
        }
    }
}
