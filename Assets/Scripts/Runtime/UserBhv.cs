using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UserBhv : MonoBehaviour
{
    // singleton instance
    public static UserBhv instance = null;

    // public fields
    public string username;
    public List<RoomBhv> rooms;

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
    }

    public void SaveUserData()
    {
        SaveSystem.SaveUserData(this);
    }

    public void LoadUserData()
    {
        UserData data = SaveSystem.LoadUserData();

        this.username = data.username;

        for (int i = 0; i < data.roomIds.Count; i++)
        {
            this.rooms.Find(r => r.roomData.room_id == data.roomIds[i]).isToggled = data.roomToggleStates[i];

            this.rooms.Find(r => r.roomData.room_id == data.roomIds[i]).Enable();
        }
    }
}
