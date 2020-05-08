using UnityEngine;
using Cinemachine;

public class VirtualCameraBhv : MonoBehaviour
{
    // public fields
    [Range(.1f, 30f)]
    public float panSpeedModifier = 10f;
    [Range(.1f, 30f)]
    public float zoomSpeedModifier = 10f;
    public Vector3 boundingBoxDimensions = new Vector3(40, 20, 10);

    // private fields
    private Transform _transform;
    private Transform _followTarget;
    private Bounds _boundingBox;
    private Vector3 _targetHomePosition;
    private Vector3 _mouseDownPosition;
    private CinemachineVirtualCamera _virtualCamera;
    private CinemachineTransposer _transposer;
    private bool _isSelected;

    private void Awake()
    {
        _transform = this.GetComponent<Transform>();

        _virtualCamera = this.GetComponent<CinemachineVirtualCamera>();

        _transposer = _virtualCamera.GetCinemachineComponent<CinemachineTransposer>();
    }
    private void Start()
    {
        _followTarget = _virtualCamera.Follow;

        _targetHomePosition = _followTarget.position;

        this.InitializeBoundingBox();
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

            this.StayWithinBoundingBox();
        }
    }

    private void InitializeBoundingBox()
    {
        Vector3 boxCenter = _targetHomePosition + Vector3.forward * boundingBoxDimensions.z / 2f;

        _boundingBox = new Bounds(boxCenter, boundingBoxDimensions);
    }

    public void Select()
    {
        _isSelected = true;

        _virtualCamera.Priority = 1;

        _followTarget.position += Vector3.back * 50;
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
        float multiplier = increment * zoomSpeedModifier * Time.deltaTime;

        Vector3 direction = this.GetWorldPosition(Input.mousePosition) - _transform.position;

        _followTarget.position += direction * multiplier;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(_boundingBox.center, boundingBoxDimensions);
    }

    private void StayWithinBoundingBox()
    {
        Vector3 closestPoint = _boundingBox.ClosestPoint(_followTarget.position);

        Vector3 transgression = _followTarget.position - closestPoint;

        _followTarget.position -= transgression * Time.deltaTime;
    }

    private Vector3 GetWorldPosition(Vector3 point)
    {
        Ray mouseRay = CameraManager.mainCamera.ScreenPointToRay(point);

        Plane groundPlane = new Plane(Vector3.forward, new Vector3(0, 0, _followTarget.position.z));

        groundPlane.Raycast(mouseRay, out float distance);

        return mouseRay.GetPoint(distance);
    }
}
