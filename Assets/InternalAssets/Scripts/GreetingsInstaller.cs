using UnityEngine;
using Zenject;

public class GreetingsInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        SignalBusInstaller.Install(Container);

        Container.Bind<IGameController>().To<GameController>().AsSingle().NonLazy();

        Container.DeclareSignal<PlayerControlSwapSignal>();

        Container.BindSignal<PlayerControlSwapSignal>()
            .ToMethod<IGameController>(x => x.ReceivePlayerSwipeInput).FromResolve();

        Container.BindInterfacesAndSelfTo<SwipeControll>().AsSingle().NonLazy();

        //Container.BindInterfacesTo<Field>();
    }
}