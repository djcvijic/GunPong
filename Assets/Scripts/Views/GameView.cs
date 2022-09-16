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
            var owner = DetermineOwner(paddle);
            var brain = DetermineBrain(paddle);
            paddle.Initialize(owner, brain);
        }
    }

    private PlayerEnum DetermineOwner(PaddleView paddle)
    {
        return paddle.IsABottom ? PlayerEnum.Player1 : PlayerEnum.Player2;
    }

    private SimplePaddleBrain DetermineBrain(PaddleView paddle)
    {
        return paddle.IsABottom ? null : new SimplePaddleBrain(this, paddle);
    }

    public PaddleView GetPaddle(PlayerEnum owner)
    {
        return paddles.Find(x => x.Owner == owner);
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
