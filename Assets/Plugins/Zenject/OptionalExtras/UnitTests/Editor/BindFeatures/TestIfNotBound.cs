using NUnit.Framework;
using Plugins.Zenject.OptionalExtras.TestFramework;
using Assert = Plugins.Zenject.Source.Internal.Assert;

namespace Plugins.Zenject.OptionalExtras.UnitTests.Editor.BindFeatures
{
    [TestFixture]
    public class TestIfNotBound : ZenjectUnitTestFixture
    {
        interface IFoo
        {
        }

        public class Foo1 : IFoo
        {
        }

        public class Foo2 : IFoo
        {
        }

        [Test]
        public void Test1()
        {
            Container.Bind<IFoo>().To<Foo1>().AsSingle();
            Container.Bind<IFoo>().To<Foo2>().AsSingle();

            Assert.IsEqual(Container.ResolveAll<IFoo>().Count, 2);
        }

        [Test]
        public void Test2()
        {
            Container.Bind<IFoo>().To<Foo1>().AsSingle();
            Container.Bind<IFoo>().To<Foo2>().AsSingle().IfNotBound();

            Assert.IsEqual(Container.ResolveAll<IFoo>().Count, 1);
            Assert.IsType<Foo1>(Container.Resolve<IFoo>());
        }
    }
}

