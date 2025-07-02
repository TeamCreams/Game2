using UnityEngine;

[RequireComponent(typeof(Camera))]
public class FollowCamera : BaseObject
{
    public Vector3 _offset = new Vector3(0, 0, -10f);
    public float _followDelay = 0.1f;

    // Zoom
    public float _zoomOutSize = 8f;
    public float _zoomInSize = 5f;
    public float _zoomLerpSpeed = 2f;
    public float _idleTimeToZoomOut = 5f;

    private Camera _camera;
    private Vector3 _velocity = Vector3.zero;
    private Vector3 _lastPlayerPosition;
    private float _idleTimer = 0f;
    private PlayerController _player;

    public override bool Init()
    {
        if (false == base.Init())
        {
            return false;
        }
        _camera = GetComponent<Camera>();
        _player = GetComponentInParent<PlayerController>();
        if (_player == null)
        {
            Debug.LogError("Can't find PlayerController.");
        }
        _lastPlayerPosition = _player.transform.position;
        return true;
    }

    void LateUpdate()
    {
        if (Vector3.Distance(_player.transform.position, _lastPlayerPosition) < 0.01f)
        {
            _idleTimer += Time.deltaTime;
        }
        else
        {
            _idleTimer = 0f;
            _lastPlayerPosition = _player.transform.position;
        }

        float targetSize = (_idleTimer >= _idleTimeToZoomOut) ? _zoomOutSize : _zoomInSize;
        _camera.orthographicSize = Mathf.Lerp(_camera.orthographicSize, targetSize, _zoomLerpSpeed * Time.deltaTime);

        Vector3 desiredPos = _player.transform.position + _offset;
        Vector3 smoothPos = Vector3.SmoothDamp(transform.position, desiredPos, ref _velocity, _followDelay);

        transform.position = smoothPos;
    }
}
