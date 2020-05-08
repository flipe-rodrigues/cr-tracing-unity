using System.Collections;
using UnityEngine;

public class MapBhv : MonoBehaviour, IMap
{
    // public properties
    public bool IsSelected { get; private set; }

    // public fields
    public Color mapColor;

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
        return this.mapColor;
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
                //room.gameObject.SetActive(true);

                room.Enable();
            }
        }
    }

    public IEnumerator Deselect()
    {
        foreach (RoomBhv room in _rooms)
        {
            if (room.IsEnabled)
            {
                room.Disable();
            }

            yield return null;
        }
    }
}
