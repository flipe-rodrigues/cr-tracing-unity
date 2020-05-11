using System.Collections;
using UnityEngine;

public class VirtualCameraManager : MonoBehaviour
{
    // singleton instance
    public static VirtualCameraManager instance = null;

    // static fields
    public static Camera mainCamera;

    // public fields
    [Header("Pan settings:")]
    [Range(.1f, 30f)]
    public float panMultiplier = 10f;
    [Range(1f, 10f)]
    public float panIncrementCap = 7.5f;
    [Header("Zoom settings:")]
    [Range(.1f, 30f)]
    public float zoomMultiplier = 15f;
    [Range(1f, 10f)]
    public float zoomIncrementCap = 7.5f;

    // private fields
    private VirtualCameraBhv[] _virtualCameras;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        mainCamera = Camera.main;

        _virtualCameras = this.GetComponentsInChildren<VirtualCameraBhv>();
    }

    public void SelectVirtualCamera(int mapIndex)
    {
        _virtualCameras[mapIndex].Select();

        for (int i = 0; i < _virtualCameras.Length; i++)
        {
            if (i != mapIndex)
            {
                _virtualCameras[i].Deselect();
            }
        }

        StopAllCoroutines();

        StartCoroutine(this.ChangeCameraColor(MapManager.instance.Maps[mapIndex].GetColor()));
    }

    public IEnumerator ChangeCameraColor(Color targetColor)
    {
        Color currentColor = mainCamera.backgroundColor;

        float lerp = 0;

        while (lerp < 1)
        {
            mainCamera.backgroundColor = Color.Lerp(currentColor, targetColor, lerp);

            lerp = Mathf.Clamp01(lerp + Time.deltaTime * .5f);

            yield return null;
        }

        mainCamera.backgroundColor = targetColor;
    }
}
