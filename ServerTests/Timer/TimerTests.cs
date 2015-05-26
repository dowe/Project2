using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Server.Timer;
using System.Diagnostics;
using System.Collections.Generic;
using System.Threading;

namespace ServerTests.Timer
{
    [TestClass]
    public class TimerTests
    {
        private bool ok1;
        private bool ok2;
        private int i1;
        private int i2;
        private Stopwatch s1;
        private Stopwatch s2;

        [TestInitialize]
        public void SetUp()
        {
            ok2 = true;
            ok1 = true;
            s2 = null;
            s1 = null;
            i1 = 0;
            i2 = 0;
        }

        [TestMethod]
        public void TimerTest()
        {
            TimerFactory factory = new TimerFactory(() => TimeSpan.FromSeconds(2.0), false);
            ITimer t1 = factory.CreateInstance();
            ITimer t2 = factory.CreateInstance();
            t1.Start(T1Action);
            t2.Start(T2Action);

            Thread.Sleep(9000);

            t1.Cancel();
            t2.Cancel();

            Assert.IsTrue(ok1);
            Assert.IsTrue(ok2);

            Assert.IsTrue(i1 > 1);
            Assert.IsTrue(i2 > 1);

            Thread.Sleep(2000);

            i1 = 0;
            i2 = 0;

            factory = new TimerFactory(() => TimeSpan.FromSeconds(1.0), true);
            t1 = factory.CreateInstance();
            t2 = factory.CreateInstance();

            t1.Start(T1Action);
            t2.Start(T2Action);

            Thread.Sleep(2000);

            t1.Cancel();
            t2.Cancel();

            Assert.IsFalse(ok1);
            Assert.IsFalse(ok2);

            Assert.IsTrue(i1 == 1);
            Assert.IsTrue(i2 == 1);
        }

        private void T2Action()
        {
            if (s2 != null)
            {
                s2.Stop();
                ok2 &= Is2Seconds(s2.Elapsed);
                i2++;
            }

            s2 = new Stopwatch();
            s2.Start();
        }

        private bool Is2Seconds(TimeSpan span)
        {
            return span.TotalMilliseconds >= 1970 && span.TotalMilliseconds <= 2030;
        }

        private void T1Action()
        {
            if (s1 != null)
            {
                s1.Stop();
                ok1 &= Is2Seconds(s1.Elapsed);
                i1++;
            }

            s1 = new Stopwatch();
            s1.Start();
        }
    }
}
