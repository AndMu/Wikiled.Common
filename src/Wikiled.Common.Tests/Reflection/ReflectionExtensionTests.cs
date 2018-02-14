using NUnit.Framework;
using Wikiled.Common.Reflection;

namespace Wikiled.Common.Tests.Reflection
{
    [TestFixture]
    public class ReflectionExtensionTests
    {
        [Test]
        public void ResolveType()
        {
            var type = ReflectionExtension.ResolveType("Wikiled.Common.Tests.Reflection.ReflectionExtensionTests, Wikiled.Common.Tests");
            Assert.AreSame(GetType(), type);

            type = ReflectionExtension.ResolveType("Wikiled.Common.Tests.Reflection.ReflectionExtensionTests, Wikiled.Common.Tests, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null");
            Assert.AreSame(GetType(), type);
        }

        [Test]
        public void GetTypeName()
        {
            Assert.AreEqual("Wikiled.Common.Tests.Reflection.ReflectionExtensionTests,Wikiled.Common.Tests", ReflectionExtension.GetTypeName<ReflectionExtensionTests>());
        }

        [Test]
        public void IsPrimitive()
        {
            Assert.IsTrue(typeof(string).IsPrimitive());
            Assert.IsTrue(typeof(int).IsPrimitive());
            Assert.IsTrue(typeof(string).IsPrimitive());
            Assert.IsFalse(typeof(ReflectionExtensionTests).IsPrimitive());
        }
    }
}
