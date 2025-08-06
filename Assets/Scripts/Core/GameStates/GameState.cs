namespace Core.GameStates
{
    public interface IGameState
    {
        void Enter();
        void Exit();
    }
    public abstract class GameState : IGameState
    {
        protected readonly GameStateMachine StateMachine;

        protected GameState(GameStateMachine stateMachine)
        {
            StateMachine = stateMachine;
        }

        public virtual void Enter() { }
        public virtual void Exit() { }
    }
}


namespace Core.GameStates
{
    using UnityEngine;
    using UI;
    using Service;
    public class PlayingState : GameState
    {
        private readonly GameResetService _resetService;
        private readonly ITimerService _timerService;
        private readonly UIController _uiController;

        public PlayingState(GameStateMachine machine, GameResetService resetService, ITimerService timerService, UIController uiController)
            : base(machine)
        {
            _resetService = resetService;
            _timerService = timerService;
            _uiController = uiController;
        }

        public override void Enter()
        {
            Time.timeScale = 1f;
            _timerService.ResetTimer();
            _timerService.StartTimer();
            
            _uiController.SetActive(_uiController.PauseButton, true);
            
            Debug.Log("Playing State started");
        }

        public override void Exit()
        {
            _timerService.StopTimer();
        }
    }      
    public class RestartGameState : GameState
    {
        private readonly GameResetService _resetService;
        private readonly ITimerService _timerService;
        private readonly UIController _uiController;

        public RestartGameState(GameStateMachine machine, GameResetService resetService, ITimerService timerService, UIController uiController)
            : base(machine)
        {
            _resetService = resetService;
            _timerService = timerService;
            _uiController = uiController;
        }

        public override void Enter()
        {
            Time.timeScale = 1f;
            _timerService.ResetTimer();
            _timerService.StartTimer();
            
             _uiController.SetActive(_uiController.PauseButton, true);
             
            _resetService.ResetGame();
            Debug.Log("Playing State started");
        }
        public override void Exit()
        {
            _timerService.StopTimer();
        }
    }
    
    
        public class PauseState : GameState
        {
            private readonly UIController _uiController;

            public PauseState(GameStateMachine machine, UIController uiController) : base(machine)
            {
                _uiController = uiController;
            }

            public override void Enter()
            {
                Time.timeScale = 0f;
                Debug.Log("Pause State");
                _uiController.ShowPause(0f); // Пауза с задержкой 0.5 сек
            }

            public override void Exit()
            {
                _uiController.HidePause();
            }
        }

    public class WinState : GameState
    {
        private readonly UIController _uiController;
        private readonly ITimerService _timerService;

        public WinState(GameStateMachine machine, UIController uiController, ITimerService timerService)
            : base(machine)
        {
            _uiController = uiController;
            _timerService = timerService;
        }

        public override void Enter()
        {
            Time.timeScale = 0f;
            float timePlayed = _timerService.GetTime();
            int reward = Mathf.FloorToInt(timePlayed * 2); // 2 монеты за секунду

            PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins", 0) + reward);
            PlayerPrefs.Save();

            Debug.Log($"Win State — награда: {reward} монет");
            
            _uiController.UpdateText(_uiController.CoinRewardText, reward);
            
            _uiController.UpdateText(_uiController.ResultText, "You WON");
            
            _uiController.SetActive(_uiController.PauseButton, false);
            
            _uiController.HideAllUI();
            _uiController.ShowResult(0f);
        }

        public override void Exit()
        {
            _uiController.HidePause();
            _uiController.UpdateText(_uiController.CoinRewardText, 0);
        }
    }

    public class LoseState : GameState
    {
        private readonly UIController _uiController;

        public LoseState(GameStateMachine machine, UIController uiController) : base(machine)
        {
            _uiController = uiController;
        }

        public override void Enter()
        {
            Time.timeScale = 0f;
            Debug.Log("Lose State");
            _uiController.HideAllUI();
            
            _uiController.SetActive(_uiController.PauseButton, false);
            
            _uiController.UpdateText(_uiController.ResultText, "You LOSE");
                
            _uiController.ShowResult(0f);
        }

        public override void Exit()
        {
            _uiController.HidePause();
        }
    }
}

