using System;
using System.Collections.Generic;
using UnityEngine;

public class GameView : GenericMonoSingleton<GameView>
{
    [SerializeField] private GameBoundsView gameBounds;

    [SerializeField] private BallView ball;

    [SerializeField] private List<PaddleView> paddles;

    [SerializeField] private Transform topPaddleStartingPoint;

    [SerializeField] private Transform bottomPaddleStartingPoint;

    [SerializeField] private AudioClipSettings musicSettings;

    [SerializeField] private AudioClipSettings dieSoundSettings;

    private Player player1;

    private Player player2;

    private GameFlow gameFlow;

    public GameState GameState => gameFlow.GameState;

    public AudioManager AudioManager => AudioManager.Instance;

    public GameBoundsView GameBounds => gameBounds;

    public BallView Ball => ball;

    private PaddleView BottomPaddle => paddles.Find(x => x.IsABottom);

    private PaddleView TopPaddle => paddles.Find(x => !x.IsABottom);

    private void Start()
    {
        gameFlow = new GameFlow(GameUI.Instance, CoroutineRunner.Instance);
        gameFlow.StartMainMenu();
        AudioManager.PlayAudio(musicSettings);
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
            ClearBullets();
            gameFlow.GameOver(GetEnemyPlayer(player));
            AudioManager.PlayAudio(dieSoundSettings);
        }
        else if (player.CurrentLives != previousLives)
        {
            ClearBullets();
            gameFlow.PrepareServe(GetPaddle(player), ball);
            AudioManager.PlayAudio(dieSoundSettings);
        }
    }

    private Player GetEnemyPlayer(Player player)
    {
        return player == player1 ? player2 : player1;
    }

    private void ClearBullets()
    {
        var bullets = GetComponentsInChildren<BulletView>();
        foreach (var bullet in bullets)
        {
            bullet.Kill();
        }
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

        gameFlow.PrepareServe(GetPaddle(player1), ball);
    }
}
