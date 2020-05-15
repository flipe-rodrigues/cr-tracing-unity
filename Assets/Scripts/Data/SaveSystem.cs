using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveSystem
{
    // private fields
    private static readonly string fileName = "userdata";
    private static readonly string fileExtension = ".crbin";
    private static readonly string saveFilePath = Path.Combine(Application.dataPath, fileName + fileExtension);

    public static void SaveUserData(UserBhv user)
    {
        UserData data = new UserData(user);

        BinaryFormatter formatter = new BinaryFormatter();

        FileStream stream = new FileStream(saveFilePath, FileMode.Create);

        formatter.Serialize(stream, data);

        stream.Close();
    }

    public static UserData LoadUserData()
    {
        if (File.Exists(saveFilePath))
        {
            BinaryFormatter formatter = new BinaryFormatter();

            FileStream stream = new FileStream(saveFilePath, FileMode.Open);

            UserData data = formatter.Deserialize(stream) as UserData;

            stream.Close();

            return data;
        }
        else
        {
            Debug.LogError("Save file not found in " + saveFilePath);

            return null;
        }
    }

    public static void SavePlayerPrefs(UserBhv user)
    {
        UserData data = new UserData(user);

        string dataString = JsonUtility.ToJson(data);

        PlayerPrefs.SetString("userData", dataString);
    }

    public static UserData LoadPlayerPrefs()
    {
        string dataString = PlayerPrefs.GetString("userData");

        UserData data = JsonUtility.FromJson<UserData>(dataString);

        return data;
    }
}