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

    public GameBoundsView GameBounds => gameBounds;

    private PlayerEnum GetOwner(PaddleView paddleView)
    {
        return paddleView.IsLocalPlayer ? PlayerEnum.Player1 : PlayerEnum.Player2;
    }

    public void InitializeMe(PaddleView paddleView)
    {
        paddleView.Initialize(GetOwner(paddleView), new SimplePaddleBrain(this));
    }
}
