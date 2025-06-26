using System;
using UnityEngine;

public partial class BaseObject : InitBase
{
    public int DataTemplateID { get; set; }

    public int ObjectId { get; set; }

    protected Collider _collider;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        _collider = GetComponent<Collider>();
        if (_collider == null)
        {
            _collider = gameObject.GetComponentInChildren<Collider>();
        }

        EventBinding();
        return true;
    }

    public virtual void EventBinding()
    {
        //this.gameObject.BindEvent(TriggerEnter, EGOEvent.TriggerEnter);
    }

    public virtual void SetInfo(int dataTemplate)
    {
        DataTemplateID = dataTemplate;
    }

    public virtual void Clear()
    {

    }

    #region Static Functions
    public static void BindEvent(GameObject go, Action action, Define.EGOEvent type)
    {
        GO_EventHandler evt = Util.GetOrAddComponent<GO_EventHandler>(go);

        switch (type)
        {
            case Define.EGOEvent.MouseEnter:
                evt.OnMouseEnterHandler -= action;
                evt.OnMouseEnterHandler += action;
                break;
            case Define.EGOEvent.MouseExit:
                evt.OnMouseExitHandler -= action;
                evt.OnMouseExitHandler += action;
                break;
            case Define.EGOEvent.MouseDown:
                evt.OnMouseDownHandler -= action;
                evt.OnMouseDownHandler += action;
                break;
            case Define.EGOEvent.MouseDrag:
                evt.OnMouseDragHandler -= action;
                evt.OnMouseDragHandler += action;
                break;
            case Define.EGOEvent.MouseOver:
                evt.OnMouseOverHandler -= action;
                evt.OnMouseOverHandler += action;
                break;
            case Define.EGOEvent.MouseUp:
                evt.OnMouseUpHandler -= action;
                evt.OnMouseUpHandler += action;
                break;
            case Define.EGOEvent.MouseUpAsButton:
                evt.OnMouseUpAsButtonHandler -= action;
                evt.OnMouseUpAsButtonHandler += action;
                break;
        }
    }

    public static void BindEvent(GameObject go, Action<Collider> action, Define.EGOEvent type)
    {
        GO_EventHandler evt = Util.GetOrAddComponent<GO_EventHandler>(go);

        switch (type)
        {
            case Define.EGOEvent.TriggerEnter:
                evt.OnTriggerEnterHandler -= action;
                evt.OnTriggerEnterHandler += action;
                break;
            case Define.EGOEvent.TriggerStay:
                evt.OnTriggerStayHandler -= action;
                evt.OnTriggerStayHandler += action;
                break;
            case Define.EGOEvent.TriggerExit:
                evt.OnTriggerExitHandler -= action;
                evt.OnTriggerExitHandler += action;
                break;
        }
    }
    #endregion
}
