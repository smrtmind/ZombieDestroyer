using Zenject;

namespace CodeBase.Scripts.Installers
{
    public abstract class BaseInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindInstances();
        }

        protected abstract void BindInstances();

        protected void BindInstanceAsSingle<T>(T instance) where T : class
        {
            Container.Bind<T>().FromInstance(instance).AsSingle().NonLazy();
            Container.QueueForInject(instance);
            
            if (instance is IInitializable init)
                init.Initialize();
        }

        protected void BindInstanceAsTransient<T>(T instance) where T : class
        {
            Container.Bind<T>().FromInstance(instance).AsTransient().NonLazy();
            Container.QueueForInject(instance);

            if (instance is IInitializable init)
                init.Initialize();
        }

        protected void BindInstanceAsTransientWithId<T>(T instance, string id) where T : class
        {
            Container.Bind<T>().WithId(id).FromInstance(instance).AsTransient().NonLazy();
            Container.QueueForInject(instance);

            if (instance is IInitializable init)
                init.Initialize();
        }
    }
}
