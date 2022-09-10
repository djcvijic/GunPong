using UnityEngine;

public class GameView : GenericMonoSingleton<GameView>
{
    [SerializeField] private GameBoundsView gameBounds;

    public GameBoundsView GameBounds => gameBounds;

    public PlayerEnum LocalPlayer => PlayerEnum.Player1;
}
