using UnityEngine;
using Zenject;

public class GreetingsInstaller : MonoInstaller
{
    [SerializeField]
    GameObject visualTilePrefab;
    public override void InstallBindings()
    {
        Container.Bind<IGameController>().To<GameController>().AsSingle().NonLazy();

        // Also can use KeyboardSwipeControl and KeyboardHoldSwipeControl here
        Container.BindInterfacesAndSelfTo<SwipeControl>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<TileColorsManager>().AsSingle().NonLazy();
        Container.Bind<IField>().To<FieldHolder>().AsSingle().NonLazy();
        Container.Bind<IFieldController>().To<FieldController>().AsSingle().NonLazy();
        Container.BindFactory<VisualTile, VisualTile.Factory>().FromComponentInNewPrefab(visualTilePrefab);

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