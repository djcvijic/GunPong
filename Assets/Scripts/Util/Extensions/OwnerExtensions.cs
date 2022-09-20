using System;

public static class OwnerExtensions
{
    public static Owner GetEnemy(this Owner player)
    {
        switch (player)
        {
            case Owner.Player1:
                return Owner.Player2;
            case Owner.Player2:
                return Owner.Player1;
            case Owner.Undefined:
            default:
                throw new ArgumentOutOfRangeException(nameof(player), player, null);
        }
    }
}