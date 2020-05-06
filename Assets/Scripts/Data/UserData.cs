using System;
using System.Collections.Generic;

[Serializable]
public class UserData
{
    public string username;
    public string date;
    public List<string> roomIds;
    public List<bool> roomToggleStates;

    public UserData (UserBhv user)
    {
        this.username = user.username;

        this.date = DateTime.Now.ToString("dd/MM/yyyy");

        this.roomIds = new List<string>();

        this.roomToggleStates = new List<bool>();

        foreach (RoomBhv room in user.rooms)
        {
            this.roomIds.Add(room.roomData.id);

            this.roomToggleStates.Add(room.isToggled);
        }
    }
}
