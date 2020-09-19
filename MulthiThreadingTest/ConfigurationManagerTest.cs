using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;
using MultheradingConcepts;

namespace MulthiThreadingTest
{
    [TestClass]
    public class ConfigurationManagerTest
    {
        AutoResetEvent autoReset1 = new AutoResetEvent(false);
        AutoResetEvent autoReset2 = new AutoResetEvent(false);

        [TestMethod]
        public void Singlton_WhenCalled_ShouldIncrmentOnlyOnce()
        {
            // Arrange
            ConfigurationManager configuration = new ConfigurationManager();
            Thread t1 = new Thread(() => { configuration.Singleton(); autoReset1.WaitOne();});
            Thread t2 = new Thread(() => { configuration.Singleton(); autoReset2.WaitOne(); });

            // Act
            t1.Start();
            t2.Start();
            autoReset1.Set();
            autoReset2.Set();

            System.Threading.Thread.Sleep(1 * 1000);

            // Assert
            Assert.AreEqual(1, configuration.LoadedCounter, string.Format("Expected value is 1, but got it {0}", configuration.LoadedCounter));
        }

        [TestMethod]
        public void UpdateDataWithLocker_WhenCalled_ShouldIncrmentOnlybyOne()
        {
            // Arrange
            int firstThread=0;
            int secondThread=0;
            ConfigurationManager configuration = new ConfigurationManager();
            Thread t1 = new Thread(() => { 
               firstThread= configuration.UpdateDataWithLocker();
                autoReset1.WaitOne();
               
            });
            Thread t2 = new Thread(() => { 
                secondThread = configuration.UpdateDataWithLocker(); 
                autoReset2.WaitOne();
            
            });

            // Act
            t1.Start();
            t2.Start();
            autoReset1.Set();
            autoReset2.Set();

            Thread.Sleep(1 * 1000);

            // Assert
            Assert.AreNotEqual(firstThread, secondThread, string.Format("Thread 1 value {0} and Thread 2 value {1}", firstThread, secondThread));
            Assert.AreNotEqual(0, firstThread);
          
        }

        [TestMethod]
        public void UpdateGetDataWithLocker_WhenCalled_ShouldLockData()
        {
            // Arrange
            int secondThread = 0;
            ConfigurationManager configuration = new ConfigurationManager();
            Thread t1 = new Thread(() =>
            {
                configuration.UpdateDataWithLocker(4);
                autoReset1.WaitOne();

            });
            Thread t2 = new Thread(z =>
            {
                secondThread = configuration.GetDataWithLocker();
                autoReset2.WaitOne();

            });

            // Act
            t1.Start();
            t2.Start();
            autoReset1.Set();
            autoReset2.Set();

            Thread.Sleep(1 * 1000);

            // Assert
            Assert.AreEqual(4, secondThread, string.Format("Actual value 4 but expected {0}", secondThread));

        }
    }
}
