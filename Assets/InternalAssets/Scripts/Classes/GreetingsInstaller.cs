using UnityEngine;
using Zenject;

public class GreetingsInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<IGameController>().To<GameController>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<KeyboardSwipeControl>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<TileColorsManager>().AsSingle().NonLazy();

        // Signals
        {
            SignalBusInstaller.Install(Container);

            // PlayerControlSwapSignal
            {
                Container.DeclareSignal<PlayerControlSwapSignal>();
                Container.BindSignal<PlayerControlSwapSignal>()
                    .ToMethod<IGameController>(x => x.ReceivePlayerSwipeInput).FromResolve();
            }

            // StartButtonSignal
            {
                Container.DeclareSignal<StartButtonSignal>();
                Container.BindSignal<StartButtonSignal>()
                    .ToMethod<IGameController>(x => x.OnStartButtonClick).FromResolve();
            }

            // RestartButtonSignal
            {
                Container.DeclareSignal<RestartButtonSignal>();
                Container.BindSignal<RestartButtonSignal>()
                    .ToMethod<IGameController>(x => x.OnRestartButtonClick).FromResolve();
            }
        }
        

    }
}