using TMPro;
using UnityEngine;

public class GameUI : GenericMonoSingleton<GameUI>
{
    [SerializeField] private MainMenuUI mainMenuUI;

    [SerializeField] private CalloutUI calloutUI;

    [SerializeField] private Joystick joystick;

    [SerializeField] private UIButton fireButton;

    [SerializeField] private TMP_Text topPlayerNameText;

    [SerializeField] private TMP_Text bottomPlayerNameText;

    [SerializeField] private PlayerLivesUI topPlayerLivesUI;

    [SerializeField] private PlayerLivesUI bottomPlayerLivesUI;

    public MainMenuUI MainMenuUI => mainMenuUI;

    public CalloutUI CalloutUI => calloutUI;

    public InputParams InputParams => new(joystick.Horizontal, fireButton.Input);

    public void Initialize(Player topPlayer, Player bottomPlayer)
    {
        topPlayerNameText.text = topPlayer.PlayerName;
        bottomPlayerNameText.text = bottomPlayer.PlayerName;
        topPlayerLivesUI.UpdateValues(topPlayer.CurrentLives, topPlayer.CurrentHealth);
        bottomPlayerLivesUI.UpdateValues(bottomPlayer.CurrentLives, bottomPlayer.CurrentHealth);
    }

    public void UpdateTopPlayerLife(Player player)
    {
        topPlayerLivesUI.UpdateValues(player.CurrentLives, player.CurrentHealth);
    }

    public void UpdateBottomPlayerLife(Player player)
    {
        bottomPlayerLivesUI.UpdateValues(player.CurrentLives, player.CurrentHealth);
    }
}