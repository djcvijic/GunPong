using UnityEngine;

public struct InputParams
{
    public readonly float Horizontal;
    public readonly bool Fire;

    public InputParams(float horizontal, bool fire)
    {
        Horizontal = horizontal;
        Fire = fire;
    }
}

public class GameUI : GenericMonoSingleton<GameUI>
{
    [SerializeField] private MainMenuUI mainMenuUI;

    [SerializeField] private CalloutUI calloutUI;

    [SerializeField] private Joystick joystick;

    [SerializeField] private UIButton fireButton;

    public MainMenuUI MainMenuUI => mainMenuUI;

    public CalloutUI CalloutUI => calloutUI;

    public InputParams InputParams => new(joystick.Horizontal, fireButton.Input);
}