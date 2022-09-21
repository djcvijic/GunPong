using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    PreGame,
    PrepareServe,
    Playing,
    GameOver,
}

public enum Owner
{
    Undefined,
    Player1,
    Player2
}

public class Player
{
    private const int MaxLives = 3;

    private const int MaxHealthPerLife = 3;

    public int CurrentLives { get; private set; }

    public int CurrentHealth { get; private set; }

    public Player(Owner owner, string playerName, Type brain)
    {
        Owner = owner;
        PlayerName = playerName;
        Brain = brain;
        CurrentLives = MaxLives;
        CurrentHealth = MaxHealthPerLife;
    }

    public Owner Owner { get; }

    public string PlayerName { get; }

    public Type Brain { get; }

    public bool IsDead => CurrentLives <= 0;

    public void LoseLife()
    {
        CurrentLives--;
        CurrentHealth = MaxHealthPerLife;
    }

    public void LoseHealth()
    {
        CurrentHealth--;
        if (CurrentHealth <= 0)
        {
            LoseLife();
        }
    }
}

public class GameView : GenericMonoSingleton<GameView>
{
    [SerializeField] private GameBoundsView gameBounds;

    [SerializeField] private BallView ball;

    [SerializeField] private List<PaddleView> paddles;

    [SerializeField] private Transform topPaddleStartingPoint;

    [SerializeField] private Transform bottomPaddleStartingPoint;

    private readonly List<Player> players = new();

    public GameState GameState { get; private set; }

    public GameBoundsView GameBounds => gameBounds;

    public BallView Ball => ball;

    private PaddleView BottomPaddle => paddles.Find(x => x.IsABottom);

    private PaddleView TopPaddle => paddles.Find(x => !x.IsABottom);

    private void Start()
    {
        StartMainMenu();
    }

    private Owner DetermineOwner(PaddleView paddle)
    {
        return paddle.IsABottom ? Owner.Player1 : Owner.Player2;
    }

    private PaddleBrain DetermineBrain(PaddleView paddle)
    {
        var player = GetPlayer(paddle.Owner);
        var brain = (PaddleBrain)Activator.CreateInstance(player.Brain);
        brain.Initialize(GameUI.Instance, this, paddle);
        return brain;
    }

    public PaddleView GetPaddle(Owner owner)
    {
        return paddles.Find(x => x.Owner == owner);
    }

    public void BulletHitPaddle(PaddleView paddle)
    {
        var player = GetPlayer(paddle.Owner);
        var previousLives = player.CurrentLives;
        player.LoseHealth();
        ReactToPlayerStatus(player, previousLives);
    }

    public void BallHitBottom()
    {
        var player = GetPlayer(BottomPaddle.Owner);
        var previousLives = player.CurrentLives;
        player.LoseLife();
        ReactToPlayerStatus(player, previousLives);
    }

    public void BallHitTop()
    {
        var player = GetPlayer(TopPaddle.Owner);
        var previousLives = player.CurrentLives;
        player.LoseLife();
        ReactToPlayerStatus(player, previousLives);
    }

    private void ReactToPlayerStatus(Player player, int previousLives)
    {
        if (GetPaddle(player.Owner).IsABottom)
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
            PrepareServe(player.Owner);
        }
    }

    private void PrepareServe(Owner owner)
    {
        GameState = GameState.PrepareServe;

        StartCoroutine(PrepareServeCoroutine(owner));
    }

    private IEnumerator PrepareServeCoroutine(Owner owner)
    {
        ClearBullets();

        var paddle = GetPaddle(owner);
        paddle.AttachBall(ball);

        var player = GetPlayer(owner);
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

    private Player GetPlayer(Owner owner)
    {
        return players.Find(x => x.Owner == owner);
    }

    private void GameOver()
    {
        GameState = GameState.GameOver;

        StartCoroutine(GameOverCoroutine());
    }

    private IEnumerator GameOverCoroutine()
    {
        var winner = players.Find(x => !x.IsDead);
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
        players.Clear();
        players.Add(new Player(Owner.Player1, player1Name, player1Brain));
        players.Add(new Player(Owner.Player2, player2Name, player2Brain));

        foreach (var paddle in paddles)
        {
            paddle.Owner = DetermineOwner(paddle);
            paddle.Brain = DetermineBrain(paddle);
        }

        BottomPaddle.transform.position = bottomPaddleStartingPoint.position;
        TopPaddle.transform.position = topPaddleStartingPoint.position;

        var topPlayer = GetPlayer(TopPaddle.Owner);
        var bottomPlayer = GetPlayer(BottomPaddle.Owner);
        GameUI.Instance.Initialize(topPlayer, bottomPlayer);

        PrepareServe(Owner.Player1);
    }
}
