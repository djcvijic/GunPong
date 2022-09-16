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

    private void Start()
    {
        foreach (var paddle in paddles)
        {
            paddle.Owner = DetermineOwner(paddle);
            paddle.Brain = DetermineBrain(paddle);
            if (paddle.Owner == PlayerEnum.Player1)
            {
                paddle.AttachBall(ball);
            }
        }
    }

    private PlayerEnum DetermineOwner(PaddleView paddle)
    {
        return paddle.IsABottom ? PlayerEnum.Player1 : PlayerEnum.Player2;
    }

    private PaddleBrain DetermineBrain(PaddleView paddle)
    {
        return paddle.IsABottom ? new LocalPlayerBrain(UIController.Instance) : new SimplePaddleBrain(this, paddle);
    }

    public PaddleView GetPaddle(PlayerEnum owner)
    {
        return paddles.Find(x => x.Owner == owner);
    }
}
