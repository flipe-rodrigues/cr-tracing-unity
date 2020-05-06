using System.Collections;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    // singleton instance
    public static CameraManager instance = null;

    // static fields
    public static Camera mainCamera;

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

        StartCoroutine(this.ChangeCameraColor(MapManager.instance.Maps[mapIndex].levelColor));
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
