using System.Collections;
using UnityEngine;

public class MapBhv : MonoBehaviour
{
    // public fields
    public Color levelColor;

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
