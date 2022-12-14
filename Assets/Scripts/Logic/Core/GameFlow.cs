using System.Collections;
using Util.CoroutineRunner;
using View.GameViews;
using View.UI;

namespace Logic.Core
{
    public class GameFlow
    {
        private readonly GameUI gameUI;
        private readonly CoroutineRunner coroutineRunner;

        public GameState GameState { get; private set; }

        public GameFlow(GameUI gameUI, CoroutineRunner coroutineRunner)
        {
            this.gameUI = gameUI;
            this.coroutineRunner = coroutineRunner;
        }

        public void StartMainMenu()
        {
            GameState = GameState.PreGame;

            gameUI.MainMenuUI.gameObject.SetActive(true);
        }

        public void PrepareServe(PaddleView paddle, BallView ball)
        {
            GameState = GameState.PrepareServe;

            coroutineRunner.StartCoroutine(PrepareServeCoroutine(paddle, ball));
        }

        private IEnumerator PrepareServeCoroutine(PaddleView paddle, BallView ball)
        {
            paddle.AttachBall(ball);

            yield return gameUI.CalloutUI.Show(2.5f, $"{paddle.Owner.PlayerName} WITH THE SERVE");

            GameState = GameState.Playing;
        }

        public void GameOver(Player winner)
        {
            GameState = GameState.GameOver;

            coroutineRunner.StartCoroutine(GameOverCoroutine(winner));
        }

        private IEnumerator GameOverCoroutine(Player winner)
        {
            yield return gameUI.CalloutUI.Show(5f, $"GAME OVER! {winner.PlayerName} WINS!");

            StartMainMenu();
        }
    }
}