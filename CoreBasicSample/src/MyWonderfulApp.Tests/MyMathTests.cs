using MyWonderfulApp.Data;
using NUnit.Framework;
using System;

namespace MyWonderfulApp.Tests
{
    [TestFixture]
    public class MyMathTests
    {
        private MyMath sut;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            sut = new MyMath();
        }

        [Test]
        public void Basic_add()
        {
            Assert.That(sut.Add(1, 3), Is.EqualTo(4));
        }
    }
}
