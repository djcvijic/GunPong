using UnityEngine;

public class UIController : GenericMonoSingleton<UIController>
{
    [SerializeField] private Joystick joystick;

    public float InputHorizontal => joystick.Horizontal;
}