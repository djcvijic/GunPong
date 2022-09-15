using System.Collections.Generic;
using UnityEngine;

public enum PlayerEnum
{
    Undefined,
    Player1,
    Player2
}

public class GameView : GenericMonoSingleton<GameView>
{
    [SerializeField] private GameBoundsView gameBounds;

    [SerializeField] private BallView ball;

    private readonly List<PaddleView> paddles = new();

    public GameBoundsView GameBounds => gameBounds;

    public BallView Ball => ball;

    public void InitializeMe(PaddleView paddleView)
    {
        paddleView.Initialize(DetermineOwner(paddleView), new SimplePaddleBrain(this, paddleView));
        paddles.Add(paddleView);
    }

    private PlayerEnum DetermineOwner(PaddleView paddleView)
    {
        return paddleView.name == "BottomPaddle" ? PlayerEnum.Player1 : PlayerEnum.Player2;
    }

    public PaddleView GetEnemyPaddle(PaddleView paddleView)
    {
        var owner = paddleView.Owner;
        var enemyOwner = owner.GetEnemy();
        var enemyPaddle = paddles.Find(x => x.Owner == enemyOwner);
        return enemyPaddle;
    }
}
