using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    // private fields
    private static readonly string fileName = "userdata";
    private static readonly string fileExtension = ".crbin";
    private static readonly string saveFilePath = Path.Combine(Application.dataPath, fileName + fileExtension);

    public static void SaveUserData(UserBhv user)
    {
        UserData data = new UserData(user);

        // MD5 cript

        BinaryFormatter formatter = new BinaryFormatter();

        FileStream stream = new FileStream(saveFilePath, FileMode.Create);

        formatter.Serialize(stream, data);

        //Debug.Log(stream.);

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
}