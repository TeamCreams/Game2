using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UniRx;

public class UI_Joystick : UI_Base
{
    enum Images
    {
        BlockingImage,
        Ground,
        Lever
    }

    //[SerializeField, Range(10f, 150f)]
    private float leverRange = 65f;

    private RectTransform _rectTransform = null;
    Vector2 _touchPosition = Vector2.zero;

    public override bool Init()
    {
        if (base.Init() == false)
        {
            return false;
        }

        BindImages(typeof(Images));
        //_rectTransform = GetImage((int)Images.Ground).rectTransform;

        //Debug.Log(this.GetImage((int)Images.BlockingImage));

        this.GetImage((int)Images.BlockingImage).gameObject.BindEvent(OnBeginDrag, Define.EUIEvent.PointerDown);
        this.Get<Image>((int)Images.BlockingImage).gameObject.BindEvent(OnDrag, Define.EUIEvent.Drag);
        this.Get<Image>((int)Images.BlockingImage).gameObject.BindEvent(OnEndDrag, Define.EUIEvent.EndDrag);
        return true;
    }

    void OnBeginDrag(PointerEventData eventData)
	{
        _touchPosition = eventData.position;
        GetImage((int)Images.Ground).gameObject.transform.position = _touchPosition;
        Event_UI_Joystick.JoystickState = Define.EJoystickState.PointerDown;
    }
    void OnDrag(PointerEventData eventData)
    {
        Vector2 targetPosition = eventData.position;

        Vector2 moveDir = targetPosition - _touchPosition;

        if(leverRange <= moveDir.magnitude)
		{
            moveDir = moveDir.normalized * leverRange;
        }
        GetImage((int)Images.Lever).transform.position = _touchPosition + moveDir;

        Event_UI_Joystick.JoystickAmount = moveDir.normalized * (moveDir.magnitude / leverRange);
        Event_UI_Joystick.JoystickState = Define.EJoystickState.Drag;
    }
    void OnEndDrag(PointerEventData eventData)
    {
        GetImage((int)Images.Lever).gameObject.transform.position = _touchPosition;
        Event_UI_Joystick.JoystickAmount = Vector2.zero;
        Event_UI_Joystick.JoystickState = Define.EJoystickState.PointerUp;
    }

    public void JoystickMove()
    {
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(_rectTransform, Input.mousePosition, null, out Vector2 localPoint))
        {
            var clampedDir = localPoint.magnitude < leverRange ? localPoint : localPoint.normalized * leverRange;

            GetImage((int)Images.Lever).rectTransform.anchoredPosition = clampedDir;

            //Managers.Game.JoystickAmount = localPoint.normalized;
        }
    }

}
