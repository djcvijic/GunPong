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

    [SerializeField] private List<PaddleView> paddles;

    public GameBoundsView GameBounds => gameBounds;

    public BallView Ball => ball;

    protected override void Awake()
    {
        base.Awake();
        foreach (var paddle in paddles)
        {
            paddle.Initialize(DetermineOwner(paddle), new SimplePaddleBrain(this, paddle));
        }
    }

    private PlayerEnum DetermineOwner(PaddleView paddle)
    {
        return paddle.IsABottom ? PlayerEnum.Player1 : PlayerEnum.Player2;
    }

    public PaddleView GetEnemyPaddle(PaddleView paddle)
    {
        var owner = paddle.Owner;
        var enemyOwner = owner.GetEnemy();
        var enemyPaddle = paddles.Find(x => x.Owner == enemyOwner);
        return enemyPaddle;
    }

    public bool TryGetInputParams(PaddleView paddle, out InputParams inputParams)
    {
        if (paddle.IsABottom)
        {
            inputParams = UIController.Instance.InputParams;
            return true;
        }

        inputParams = default;
        return false;
    }
}
