using Unity.Cinemachine;
using UnityEditor.SceneManagement;
using UnityEngine;

public class FreeLookCamera : BaseObject
{

    // Zoom
    public float _zoomOutSize = 8f;
    public float _zoomInSize = 5f;
    public float _zoomLerpSpeed = 2f;
    public float _idleTimeToZoomOut = 5f;

    [SerializeField]
    private Transform _player;
    private CinemachineCamera _camera;
    public override bool Init()
    {
        if (false == base.Init())
        {
            return false;
        }
        
        _camera = GetComponent<CinemachineCamera>();

        return true;
    }

    public void SetTarget()
    {
        int ownerObjectId = Contexts.BattleRush.PlayerObjectId.Value;
        _player = Managers.Object.ObjectDic[ownerObjectId].transform;
        if (_player == null)
        {
            Debug.LogError("Can't find Player.");
        }
        _camera.Target.TrackingTarget = _player;
        _camera.Follow = _player;
        _camera.UpdateTargetCache();
    }
    private void ScreenSize()
    {
        CinemachineBrain brain = CinemachineBrain.GetActiveBrain(0);
        if (brain != null)
        {
            Camera brainCamera = brain.OutputCamera;
            int brainWidth = brainCamera.pixelWidth;
            int brainHeight = brainCamera.pixelHeight;
        }
    }

    void LateUpdate()
    {
        
    }
}
