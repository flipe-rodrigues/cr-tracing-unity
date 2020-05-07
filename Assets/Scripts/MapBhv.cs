using System.Collections;
using UnityEngine;

public class MapBhv : MonoBehaviour, IMap
{
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
        for (int i = 0; i < _rooms.Length; i++)
        {
            _rooms[i].Enable();
        }
    }

    public IEnumerator Deselect()
    {
        for (int i = 0; i < _rooms.Length; i++)
        {
            _rooms[i].Disable();

            yield return null;
        }
    }
}
