namespace Core.GameStates
{
    public class GameStateMachine
    {
        private IGameState _currentState;

        public void ChangeState(IGameState newState)
        {
            _currentState?.Exit();
            _currentState = newState;
            _currentState.Enter();
        }
    }
}