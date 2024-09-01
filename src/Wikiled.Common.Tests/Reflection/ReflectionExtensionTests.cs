using NUnit.Framework;
using NUnit.Framework.Legacy;
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
            ClassicAssert.AreSame(GetType(), type);

            type = ReflectionExtension.ResolveType("Wikiled.Common.Tests.Reflection.ReflectionExtensionTests, Wikiled.Common.Tests, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null");
            ClassicAssert.AreSame(GetType(), type);
        }

        [Test]
        public void GetTypeName()
        {
            ClassicAssert.AreEqual("Wikiled.Common.Tests.Reflection.ReflectionExtensionTests,Wikiled.Common.Tests", ReflectionExtension.GetTypeName<ReflectionExtensionTests>());
        }

        [Test]
        public void IsPrimitive()
        {
            ClassicAssert.IsTrue(typeof(string).IsPrimitive());
            ClassicAssert.IsTrue(typeof(int).IsPrimitive());
            ClassicAssert.IsTrue(typeof(string).IsPrimitive());
            ClassicAssert.IsFalse(typeof(ReflectionExtensionTests).IsPrimitive());
        }
    }
}
