using System;
using NUnit.Framework;
using Plugins.Zenject.Source.Binding.Binders.Factory.FactoryFromBinder;
using Plugins.Zenject.Source.Factories;
using Plugins.Zenject.Source.Factories.Pooling;
using Plugins.Zenject.Source.Main;
using Zenject;
using Assert = Plugins.Zenject.Source.Internal.Assert;

namespace Plugins.Zenject.OptionalExtras.UnitTests.Editor.MemoryPool
{
    [TestFixture]
    public class TestFromPoolableMemoryPoolValidation
    {
        public class Bar
        {
        }

        public class Foo : IPoolable<IMemoryPool>, IDisposable
        {
            IMemoryPool _pool;

            public Foo(Bar bar)
            {
            }

            public IMemoryPool Pool
            {
                get { return _pool; }
            }

            void SetDefaults()
            {
                _pool = null;
            }

            public void Dispose()
            {
                _pool.Despawn(this);
            }

            public void OnDespawned()
            {
                _pool = null;
                SetDefaults();
            }

            public void OnSpawned(IMemoryPool pool)
            {
                _pool = pool;
            }

            public class Factory : PlaceholderFactory<Foo>
            {
            }
        }

        [Test]
        public void TestFailure()
        {
            var container = new DiContainer(true);
            container.Settings = new ZenjectSettings(
                ValidationErrorResponses.Throw, RootResolveMethods.All);

            container.BindFactory<Foo, Foo.Factory>().FromPoolableMemoryPool(x => x.WithInitialSize(2));

            Assert.Throws(() => container.ResolveRoots());
        }


        [Test]
        public void TestSuccess()
        {
            var container = new DiContainer(true);
            container.Settings = new ZenjectSettings(
                ValidationErrorResponses.Throw, RootResolveMethods.All);

            container.Bind<Bar>().AsSingle();
            container.BindFactory<Foo, Foo.Factory>().FromPoolableMemoryPool(x => x.WithInitialSize(2));

            container.ResolveRoots();
        }
    }
}

