using System;

namespace Logic.Core
{
    public class Player
    {
        private const int MaxLives = 3;

        private const int MaxHealthPerLife = 3;

        public int CurrentLives { get; private set; }

        public int CurrentHealth { get; private set; }

        public Player(string playerName, Type brain)
        {
            PlayerName = playerName;
            Brain = brain;
            CurrentLives = MaxLives;
            CurrentHealth = MaxHealthPerLife;
        }

        public string PlayerName { get; }

        public Type Brain { get; }

        public bool IsDead => CurrentLives <= 0;

        public void LoseLife()
        {
            CurrentLives--;
            CurrentHealth = MaxHealthPerLife;
        }

        public void LoseHealth()
        {
            CurrentHealth--;
            if (CurrentHealth <= 0)
            {
                LoseLife();
            }
        }
    }
}