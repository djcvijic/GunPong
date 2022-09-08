using UnityEngine;

public class GameView : GenericMonoSingleton<GameView>
{
    [SerializeField] private Transform gameBounds;

    public Transform GameBounds => gameBounds;
}
