using System;
using UnityEngine;
using UniRx;

public static class Event_UI_Joystick
{

    #region Action
    public static event Action<(Vector2, float)> OnMoveDirChanged;
	public static event Action<Define.EJoystickState> OnJoystickStateChanged;

    private static Subject<Vector2> _joystickAmountEvent = new Subject<Vector2>();
    public static IObservable<Vector2> JoystickAmountEvent => _joystickAmountEvent;
    public static void ChangeJoystickAmountEvent(Vector2 state)
    {
        _joystickAmountEvent.OnNext(state);
    }


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
            ChangeJoystickAmountEvent(value);
        }
    }
}