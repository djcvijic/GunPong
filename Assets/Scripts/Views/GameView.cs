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

    public PlayerEnum GetOwner(PaddleView paddleView)
    {
        return paddleView.IsLocalPlayer ? PlayerEnum.Player1 : PlayerEnum.Player2;
    }
}
