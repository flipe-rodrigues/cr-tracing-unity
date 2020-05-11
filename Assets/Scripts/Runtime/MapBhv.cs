using System.Collections;
using UnityEngine;

public class MapBhv : MonoBehaviour, IMap
{
    // public properties
    public bool IsSelected { get; private set; }

    // public fields
    public Color mapBackgroundColor;

    // private fields
    private RoomBhv[] _rooms;

    private void Awake()
    {
        _rooms = this.GetComponentsInChildren<RoomBhv>();
    }

    private void Start()
    {
        this.LogRooms();
    }

    public string GetName()
    {
        return this.name;
    }

    public Color GetColor()
    {
        return this.mapBackgroundColor;
    }

    private void LogRooms()
    {
        foreach (RoomBhv room in _rooms)
        {
            UserBhv.instance.rooms.Add(room);
        }
    }

    public void Select()
    {
        foreach (RoomBhv room in _rooms)
        {
            if (!room.IsEnabled)
            {
                room.Enable();
            }
        }
    }

    public void Deselect()
    {
        foreach (RoomBhv room in _rooms)
        {
            if (room.IsEnabled)
            {
                room.Disable();
            }
        }
    }

    private IEnumerator LerpTowardsAlpha(float targetAlpha, float lerpSpeed)
    {
        //float currentAlpha = _canvasGroup.alpha;

        //float interpolant = 0;

        //while (interpolant < 1)
        //{
        //    interpolant = Mathf.Clamp01(interpolant + Time.deltaTime * lerpSpeed);

        //    _canvasGroup.alpha = Mathf.Lerp(currentAlpha, targetAlpha, interpolant);

        //    yield return null;
        //}

        yield return null;
    }
}
