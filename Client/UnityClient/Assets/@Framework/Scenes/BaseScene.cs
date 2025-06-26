using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UniRx;
using UniRx.Triggers;
using static Define;

public abstract class BaseScene : InitBase
{
    public EScene SceneType { get; protected set; } = EScene.Unknown;
    public Camera UICamera { get; protected set; }

    public int CameraTargetId { get; set; } = 0; // 현재 카메라가 바라보는 타겟의 ObjectId
    public Camera WorldCamera { get; protected set; }
    public IInputSystem InputSystem { get; protected set; }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        //{
        //    Object obj = Object.FindFirstObjectByType<EventSystem>();
        //    if (obj == null)
        //    {
        //        GameObject go = new GameObject() { name = "@EventSystem" };
        //        go.AddComponent<EventSystem>();
        //        go.AddComponent<StandaloneInputModule>();
        //    }
        //}

        {
            if (Camera.main == null)
            {
                GameObject go = new GameObject() { name = "@WorldCamera" };
                WorldCamera = go.AddComponent<Camera>();
                Debug.LogWarning("Main Camera가 없어 새로 생성했습니다.");
            }
            else
            {
                WorldCamera = Camera.main;
            }

            // Culling Mask에서 UI 체크 해제
            WorldCamera.cullingMask &= ~(1 << LayerMask.NameToLayer("UI"));

            Object obj = GameObject.Find("@WorldUICam");
            if (obj == null)
            {
                GameObject go = new GameObject() { name = "@WorldUICam" };
                UICamera = go.AddComponent<Camera>();
            }
            else
            {
                UICamera = obj.GetComponent<Camera>();
            }

            // Culling Mask에서 UI만 활성화하고 나머지 비활성화
            UICamera.cullingMask = 1 << LayerMask.NameToLayer("UI");


            // Clear Flags를 Depth only로 설정
            UICamera.clearFlags = CameraClearFlags.Nothing;
            UICamera.allowMSAA = false;

            // Depth 값을 Main Camera보다 크게 설정
            UICamera.depth = WorldCamera != null ? WorldCamera.depth + 1 : 1;

            // Main Camera의 나머지 설정을 UI Camera에 복사
            UICamera.fieldOfView = WorldCamera.fieldOfView;            // 시야각 설정
            UICamera.orthographic = WorldCamera.orthographic;          // 직교 카메라 설정
            UICamera.orthographicSize = WorldCamera.orthographicSize;  // 직교 카메라 크기 설정
            UICamera.nearClipPlane = WorldCamera.nearClipPlane;        // Near Clip Plane 설정
            UICamera.farClipPlane = WorldCamera.farClipPlane;          // Far Clip Plane 설정
            UICamera.aspect = WorldCamera.aspect;                      // 화면 비율 설정

            UICamera.transform.parent = WorldCamera.transform;
            UICamera.transform.localPosition = Vector3.zero;
            UICamera.transform.localRotation = Quaternion.identity;
        }


        this.UpdateAsObservable()
            .Subscribe(_ =>
            {
                if (InputSystem != null)
                    InputSystem.OnKeyAction();
            })
            .AddTo(this);

        return true;
    }

    public abstract void Clear();
}
