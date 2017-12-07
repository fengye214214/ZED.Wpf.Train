using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using Nito.AsyncEx;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
        }

        [TestMethod]
        public async Task MyMethodAsync()
        {
            Task tas = null;
            await tas;
        }

        [TestMethod]
        public void MyMethodAsync_ReturnsFalse()
        {
            AsyncContext.Run(async () =>
            {

            });
        }

        public static async Task ThrowsExceptionAsync<TException>(Func<Task> action, bool allowDerivedTypes = true)
        {
            try
            {
                await action();
                Assert.Fail("Delegate did not throw expected exception" + typeof(TException).Name + ".");
            }
            catch (Exception ex)
            {
                if (allowDerivedTypes && !(ex is TException))
                {

                }
            }
        }
    }
}
