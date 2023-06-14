using UnityEngine;
using Zenject;

public class TestInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<string>().FromInstance("Hello World!");

        Container.Bind<IGreeter>().To<Greeter>().AsSingle().NonLazy();
    }
    interface IGreeter
    {
        string Str { get; set; }
    }
    public class Greeter : IGreeter
    {
        string str;
        string IGreeter.Str
        {
            get => str;
            set => str = value;
        }
        public Greeter(string message)
        {
            Debug.Log(message);
        }
    }
}