using System;
using UnityEngine;
using UnityEngine.UI;

namespace View.UI
{
    public class PlayerLivesUI : MonoBehaviour
    {
        private static readonly Color ColorHigh = new(0.1843137f, 0.4235294f, 0.3411765f);

        private static readonly Color ColorMedium = new(0.8941177f, 0.6156863f, 0.2666667f);

        private static readonly Color ColorLow = new(0.6509434f, 0.2359109f, 0.2118637f);

        [SerializeField] private Slider bar1;

        [SerializeField] private Image fill1;

        [SerializeField] private Slider bar2;

        [SerializeField] private Image fill2;

        [SerializeField] private Slider bar3;

        [SerializeField] private Image fill3;

        private int lives;

        private int health;

        public void UpdateValues(int newLives, int newHealth)
        {
            lives = newLives;
            health = newHealth;
            UpdateBar(bar1, fill1, 1);
            UpdateBar(bar2, fill2, 2);
            UpdateBar(bar3, fill3, 3);
        }

        private void UpdateBar(Slider bar, Image fill, int lifeIndex)
        {
            var healthAtLife = GetHealthAtLife(lifeIndex);
            bar.value = healthAtLife;
            fill.enabled = healthAtLife > 0;
            fill.color = HealthToColor(healthAtLife);
        }

        private int GetHealthAtLife(int lifeIndex)
        {
            return lives > lifeIndex ? 3 : lives < lifeIndex ? 0 : health;
        }

        private static Color HealthToColor(int healthValue)
        {
            switch (healthValue)
            {
                case 0:
                case 1:
                    return ColorLow;
                case 2:
                    return ColorMedium;
                case 3:
                    return ColorHigh;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}