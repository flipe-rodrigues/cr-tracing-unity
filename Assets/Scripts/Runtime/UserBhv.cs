using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UserBhv : MonoBehaviour
{
    // singleton instance
    public static UserBhv instance = null;

    // public fields
    public string username;
    public bool loadPlayerPrefsAtStart;
    public TMP_Dropdown mapDropdown;
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

    private void Start()
    {
        if (PlayerPrefs.HasKey("userData") && this.loadPlayerPrefsAtStart)
        {
            this.LoadUserData();
        }
    }

    public void SaveUserData()
    {
        SaveSystem.SavePlayerPrefs(this);
    }

    public void LoadUserData()
    {
        UserData data = SaveSystem.LoadPlayerPrefs();

        string todayString = DateTime.Now.ToString("dd/MM/yyyy");

        if (data.date == todayString)
        {
            for (int i = 0; i < data.roomIds.Count; i++)
            {
                this.rooms.Find(r => r.roomData.room_id == data.roomIds[i]).isToggled = data.roomToggleStates[i];
            }
        }

        this.mapDropdown.value = data.mapIndex;
    }
}
