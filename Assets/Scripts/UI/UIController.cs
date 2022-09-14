using UnityEngine;

public class UIController : GenericMonoSingleton<UIController>
{
    [SerializeField] private Joystick joystick;

    [SerializeField] private UIButton fireButton;

    public float InputHorizontal => joystick.Horizontal;

    public bool InputFire => fireButton.Input;
}