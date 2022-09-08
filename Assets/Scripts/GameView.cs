using UnityEngine;

public class GameView : GenericMonoSingleton<GameView>
{
    [SerializeField] private GameBounds gameBounds;

    public GameBounds GameBounds => gameBounds;
}
