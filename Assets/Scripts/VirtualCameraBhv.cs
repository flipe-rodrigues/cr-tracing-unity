using UnityEngine;
using Cinemachine;

public class VirtualCameraBhv : MonoBehaviour
{
    // public fields
    [Header("Pan Settings:")]
    [Range(.1f, 30f)]
    public float panSpeedModifier = 15f;
    public Vector2 maxPan = new Vector2(16f, 9f);
    [Header("Zoom Settings:")]
    [Range(.1f, 1e3f)]
    public float zoomSpeedModifier = 500f;
    public float minFOV = 25f;
    public float maxFOV = 95f;

    // private fields
    private Transform _followTarget;
    private Vector3 _targetHomePosition;
    private Vector3 _mouseDownPosition;
    private CinemachineVirtualCamera _virtualCamera;
    private bool _isSelected;

    private void Awake()
    {
        _virtualCamera = this.GetComponent<CinemachineVirtualCamera>();
    }
    private void Start()
    {
        _followTarget = _virtualCamera.Follow;

        _targetHomePosition = _followTarget.position;
    }

    private void Update()
    {
        if (_isSelected)
        {
            if (Input.GetMouseButtonDown(0))
            {
                _mouseDownPosition = this.GetWorldPosition(Input.mousePosition);
            }
            if (Input.GetMouseButton(0))
            {
                this.Pan();
            }
            if (Input.GetAxis("Mouse ScrollWheel") != 0)
            {
                this.Zoom(Input.GetAxis("Mouse ScrollWheel"));
            }
        }

        this.StayWithinPanLimits();

        this.StayWithinZoomLimits();
    }

    private void StayWithinPanLimits()
    {
        Vector3 homeDirection = _targetHomePosition - _followTarget.position;

        float multiplier;

        if (homeDirection.x > maxPan.x)
        {
            multiplier = (Mathf.Abs(homeDirection.x) - maxPan.x) * Time.deltaTime;

            _followTarget.position += Vector3.right * multiplier;
        }
        if (homeDirection.x < -maxPan.x)
        {
            multiplier = (Mathf.Abs(homeDirection.x) - maxPan.x) * Time.deltaTime;

            _followTarget.position += Vector3.left * multiplier;
        }
        if (homeDirection.y > maxPan.y)
        {
            multiplier = (Mathf.Abs(homeDirection.y) - maxPan.y) * Time.deltaTime;

            _followTarget.position += Vector3.up * multiplier;
        }
        if (homeDirection.y < -maxPan.y)
        {
            multiplier = (Mathf.Abs(homeDirection.y) - maxPan.y) * Time.deltaTime;

            _followTarget.position += Vector3.down * multiplier;
        }
    }

    private void StayWithinZoomLimits()
    {
        float currentFOV = _virtualCamera.m_Lens.FieldOfView;

        float increment;

        if (currentFOV > maxFOV)
        {
            increment = Mathf.Abs(currentFOV - maxFOV) * Time.deltaTime;

            _virtualCamera.m_Lens.FieldOfView -= increment;
        }
        if (currentFOV < minFOV)
        {
            increment = Mathf.Abs(currentFOV - minFOV) * Time.deltaTime;

            _virtualCamera.m_Lens.FieldOfView += increment;
        }
    }

    public void Select()
    {
        _isSelected = true;

        _virtualCamera.Priority = 10;

        _virtualCamera.m_Lens.FieldOfView = Mathf.Min(maxFOV + 50f, 180f);
    }

    public void Deselect()
    {
        _isSelected = false;

        _virtualCamera.Priority = -1;
    }

    private void Pan()
    {
        float multiplier = panSpeedModifier * Time.deltaTime;

        Vector3 direction = _mouseDownPosition - this.GetWorldPosition(Input.mousePosition);

        _followTarget.position += direction * multiplier;
    }

    private void Zoom(float increment)
    {
        float multiplier = zoomSpeedModifier * Time.deltaTime;

        Vector3 direction = increment > 0 ? 
            this.GetWorldPosition(Input.mousePosition) - _followTarget.position :
            _followTarget.position - _targetHomePosition;

        float focalDistance = Mathf.Cos(_virtualCamera.m_Lens.FieldOfView / 2f * Mathf.Deg2Rad) * 
            Mathf.Abs(_virtualCamera.transform.position.z - _followTarget.position.z);

        _followTarget.position += direction * 1f / focalDistance * increment * multiplier;

        _virtualCamera.m_Lens.FieldOfView -= increment * multiplier;
    }

    private Vector3 GetWorldPosition(Vector3 point)
    {
        Ray mouseRay = CameraManager.mainCamera.ScreenPointToRay(point);

        Plane groundPlane = new Plane(Vector3.forward, new Vector3(0, 0, _followTarget.position.z));

        groundPlane.Raycast(mouseRay, out float distance);

        return mouseRay.GetPoint(distance);
    }
}
