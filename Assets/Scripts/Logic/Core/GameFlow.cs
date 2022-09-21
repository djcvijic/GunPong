using System.Collections;

public class GameFlow
{
    public GameState GameState { get; private set; }

    public void PrepareServe(PaddleView paddle, BallView ball)
    {
        GameState = GameState.PrepareServe;

        CoroutineRunner.Instance.StartCoroutine(PrepareServeCoroutine(paddle, ball));
    }

    private IEnumerator PrepareServeCoroutine(PaddleView paddle, BallView ball)
    {
        paddle.AttachBall(ball);

        yield return GameUI.Instance.CalloutUI.Show(2.5f, $"{paddle.Owner.PlayerName} WITH THE SERVE");

        GameState = GameState.Playing;
    }

    public void GameOver(Player winner)
    {
        GameState = GameState.GameOver;

        CoroutineRunner.Instance.StartCoroutine(GameOverCoroutine(winner));
    }

    private IEnumerator GameOverCoroutine(Player winner)
    {
        yield return GameUI.Instance.CalloutUI.Show(5f, $"GAME OVER! {winner.PlayerName} WINS!");

        StartMainMenu();
    }

    public void StartMainMenu()
    {
        GameState = GameState.PreGame;

        GameUI.Instance.MainMenuUI.gameObject.SetActive(true);
    }
}