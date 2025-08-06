namespace Service
{
    using BallСollection;
    using Human_System;

    public class GameResetService
    {
        private readonly Ball _ball;
        private readonly Human _human;

        public GameResetService(Ball ball, Human human)
        {
            _ball = ball;
            _human = human;
        }

        public void ResetGame()
        {
            _ball.ResetBall();
            _human.ResetHuman();
        }
    }
}