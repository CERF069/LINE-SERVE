using UnityEngine;
using Zenject;
using Signals;
using Core.GameStates;

namespace Core
{
    public class GameManager : MonoBehaviour
    {
        private GameStateMachine _stateMachine;

        private PlayingState _playingState;
        private RestartGameState _restartState;
        private PauseState _pauseState;
        private WinState _winState;
        private LoseState _loseState;

        private void Awake()
        {
            Application.targetFrameRate = -1;
            QualitySettings.vSyncCount = 0;
        }

        [Inject]
        private void Construct(
            GameStateMachine stateMachine,
            PlayingState playing,
            RestartGameState restartGame,
            PauseState pause,
            WinState win,
            LoseState lose,
            SignalBus signalBus)
        {
            _stateMachine = stateMachine;
            _playingState = playing;
            _restartState = restartGame;
            _pauseState = pause;
            _winState = win;
            _loseState = lose;

            signalBus.Subscribe<BallReachedTargetSignal>(OnBallReachedTarget);
        }

        [ContextMenu("Start Game")]
        public void StartGame() => _stateMachine.ChangeState(_playingState);

        [ContextMenu("Playing Game")]
        public void PlayingState() => _stateMachine.ChangeState(_playingState);

        [ContextMenu("Restart Game")]
        public void RestartGame() => _stateMachine.ChangeState(_restartState);

        [ContextMenu("Pause Game")]
        public void PauseGame() => _stateMachine.ChangeState(_pauseState);

        [ContextMenu("Win Game")]
        public void WinGame() => _stateMachine.ChangeState(_winState);

        [ContextMenu("Lose Game")]
        public void LoseGame() => _stateMachine.ChangeState(_loseState);
        
        [ContextMenu("Quit Game")]
        public void QuitGame()
        {
            Debug.Log("Выход из игры...");
            Application.Quit();

#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }
        private void OnBallReachedTarget(BallReachedTargetSignal signal)
        {
            Debug.Log($"Мяч достиг цели: {signal.TargetType}");
            if (signal.TargetType == TargetType.Player) LoseGame(); else WinGame();
        }
        
        
        
        [ContextMenu("Delete save")]
        public void ResetAllSettings()
        {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();
            
            UnityEngine.SceneManagement.SceneManager.LoadScene(
                UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
        }
    }
}