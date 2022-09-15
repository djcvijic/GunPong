using System;

public static class PlayerEnumExtensions
{
    public static PlayerEnum GetEnemy(this PlayerEnum player)
    {
        switch (player)
        {
            case PlayerEnum.Player1:
                return PlayerEnum.Player2;
            case PlayerEnum.Player2:
                return PlayerEnum.Player1;
            case PlayerEnum.Undefined:
            default:
                throw new ArgumentOutOfRangeException(nameof(player), player, null);
        }
    }
}