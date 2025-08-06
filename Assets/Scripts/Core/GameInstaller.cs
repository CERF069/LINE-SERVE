using Zenject;
using Signals;
using Core;
using Core.GameStates;
using UnityEngine;
using UI;
using Human_System;
using Service;
using BallСollection;

public class GameInstaller : MonoInstaller
{
    [SerializeField] private GameManager _gameManager;
    
    [SerializeField] private UIController _uiController;
    [SerializeField] private Ball _ball;
    [SerializeField] private Human _human;

    public override void InstallBindings()
    {
        Container.Bind<Ball>().FromInstance(_ball).AsSingle();
        Container.Bind<Human>().FromInstance(_human).AsSingle();
        Container.Bind<GameResetService>().AsSingle();

        Container.BindInterfacesAndSelfTo<TimerService>().AsSingle();

        Container.Bind<UIController>().FromInstance(_uiController).AsSingle();

        SignalBusInstaller.Install(Container);
        Container.DeclareSignal<BallReachedTargetSignal>();

        Container.BindInterfacesAndSelfTo<GameManager>().FromInstance(_gameManager).AsSingle();

        Container.Bind<GameStateMachine>().AsSingle();
        Container.Bind<PlayingState>().AsSingle();
        Container.Bind<RestartGameState>().AsSingle();
        Container.Bind<PauseState>().AsSingle();
        Container.Bind<WinState>().AsSingle();
        Container.Bind<LoseState>().AsSingle();
    }
}