using System;
using UnityEngine;

public static class Event_UI_Joystick
{

    #region Action
    public static event Action<(Vector2, float)> OnMoveDirChanged;
	public static event Action<Define.EJoystickState> OnJoystickStateChanged;
    public static event Action<Vector2> Joystickstate;
    #endregion

    private static Define.EJoystickState _joystickState;
	public static Define.EJoystickState JoystickState
	{
		get { return _joystickState; }
		set
		{
			_joystickState = value;
			OnJoystickStateChanged?.Invoke(_joystickState);
		}
	}
    
    private static Vector2 _joystickAmount;
    public static Vector2 JoystickAmount 
    {
        get { return _joystickAmount; }
        set
        {
            _joystickAmount = value;
            Joystickstate?.Invoke(value);
        }
    }
}