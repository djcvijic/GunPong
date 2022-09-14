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

public class UIController : GenericMonoSingleton<UIController>
{
    [SerializeField] private Joystick joystick;

    [SerializeField] private UIButton fireButton;

    public InputParams InputParams => new(joystick.Horizontal, fireButton.Input);
}