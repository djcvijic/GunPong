using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameView : GenericMonoSingleton<GameView>
{
    [SerializeField] private GameBoundsView gameBounds;

    [SerializeField] private BallView ball;

    [SerializeField] private List<PaddleView> paddles;

    [SerializeField] private Transform topPaddleStartingPoint;

    [SerializeField] private Transform bottomPaddleStartingPoint;

    private Player player1;

    private Player player2;

    public GameState GameState { get; private set; }

    public GameBoundsView GameBounds => gameBounds;

    public BallView Ball => ball;

    private PaddleView BottomPaddle => paddles.Find(x => x.IsABottom);

    private PaddleView TopPaddle => paddles.Find(x => !x.IsABottom);

    private void Start()
    {
        StartMainMenu();
    }

    private Player DetermineOwner(PaddleView paddle)
    {
        return paddle.IsABottom ? player1 : player2;
    }

    private PaddleBrain DetermineBrain(PaddleView paddle)
    {
        var player = paddle.Owner;
        var brain = (PaddleBrain)Activator.CreateInstance(player.Brain);
        brain.Initialize(GameUI.Instance, this, paddle);
        return brain;
    }

    public PaddleView GetPaddle(Player owner)
    {
        return paddles.Find(x => x.Owner == owner);
    }

    public PaddleView GetEnemyPaddle(Player owner)
    {
        return paddles.Find(x => x.Owner != owner);
    }

    public void BulletHitPaddle(PaddleView paddle)
    {
        var player = paddle.Owner;
        var previousLives = player.CurrentLives;
        player.LoseHealth();
        ReactToPlayerStatus(player, previousLives);
    }

    public void BallHitBottom()
    {
        var player = BottomPaddle.Owner;
        var previousLives = player.CurrentLives;
        player.LoseLife();
        ReactToPlayerStatus(player, previousLives);
    }

    public void BallHitTop()
    {
        var player = TopPaddle.Owner;
        var previousLives = player.CurrentLives;
        player.LoseLife();
        ReactToPlayerStatus(player, previousLives);
    }

    private void ReactToPlayerStatus(Player player, int previousLives)
    {
        if (GetPaddle(player).IsABottom)
        {
            GameUI.Instance.UpdateBottomPlayerLife(player);
        }
        else
        {
            GameUI.Instance.UpdateTopPlayerLife(player);
        }

        if (player.IsDead)
        {
            GameOver();
        }
        else if (player.CurrentLives != previousLives)
        {
            PrepareServe(player);
        }
    }

    private void PrepareServe(Player player)
    {
        GameState = GameState.PrepareServe;

        StartCoroutine(PrepareServeCoroutine(player));
    }

    private IEnumerator PrepareServeCoroutine(Player player)
    {
        ClearBullets();

        var paddle = GetPaddle(player);
        paddle.AttachBall(ball);

        yield return GameUI.Instance.CalloutUI.Show(2.5f, $"{player.PlayerName} WITH THE SERVE");

        GameState = GameState.Playing;
    }

    private void ClearBullets()
    {
        var bullets = GetComponentsInChildren<BulletView>();
        foreach (var bullet in bullets)
        {
            bullet.Kill();
        }
    }

    private void GameOver()
    {
        GameState = GameState.GameOver;

        StartCoroutine(GameOverCoroutine());
    }

    private IEnumerator GameOverCoroutine()
    {
        var winner = player1.IsDead ? player2 : player1;
        yield return GameUI.Instance.CalloutUI.Show(5f, $"GAME OVER! {winner.PlayerName} WINS!");

        StartMainMenu();
    }

    private void StartMainMenu()
    {
        GameState = GameState.PreGame;

        GameUI.Instance.MainMenuUI.gameObject.SetActive(true);
    }

    public void StartGame(string player1Name, Type player1Brain, string player2Name, Type player2Brain)
    {
        player1 = new Player(player1Name, player1Brain);
        player2 = new Player(player2Name, player2Brain);

        foreach (var paddle in paddles)
        {
            paddle.Owner = DetermineOwner(paddle);
            paddle.Brain = DetermineBrain(paddle);
        }

        BottomPaddle.transform.position = bottomPaddleStartingPoint.position;
        TopPaddle.transform.position = topPaddleStartingPoint.position;

        var topPlayer = TopPaddle.Owner;
        var bottomPlayer = BottomPaddle.Owner;
        GameUI.Instance.Initialize(topPlayer, bottomPlayer);

        PrepareServe(player1);
    }
}
