using System;
using System.Text;
using System.Collections;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkManager : MonoBehaviour
{
    // public fields
    public string uri;
    public TextAsset authKeyFile;

    // private fields
    private bool _isPosting;
    private bool _isGetting;

    public void OnSubmit()
    {
        if (!_isPosting)
        {
            StartCoroutine(this.PostUserData());
        }
    }

    public void OnLoad()
    {
        if (!_isGetting)
        {
            StartCoroutine(this.GetUserData());
        }
    }

    private IEnumerator PostUserData()
    {
        _isPosting = true;

        WWWForm form = new WWWForm();

        MD5 md5Hash = MD5.Create();

        string userHash = GetMd5Hash(md5Hash, UserBhv.instance.username);

        form.AddField("user", userHash);

        md5Hash.Dispose();

        foreach (RoomBhv room in UserBhv.instance.rooms)
        {
            Debug.Log(room.roomData.server_id + "   " + room.isToggled.ToString());
            form.AddField(room.roomData.server_id, room.isToggled ? "1" : "0");
        }

        UnityWebRequest request = UnityWebRequest.Post(uri, form);

        request.SetRequestHeader("AUTHORIZATION", "Basic " + authKeyFile.text);

        yield return request.SendWebRequest();

        if (request.isNetworkError || request.isHttpError)
        {
            Debug.Log(request.error);
        }
        else
        {
            Debug.Log("Form upload complete! Here's the response:\n" + request.downloadHandler.text);
        }

        _isPosting = false;
    }

    private IEnumerator GetUserData()
    {
        _isGetting = true;

        UnityWebRequest request = UnityWebRequest.Get(uri);

        request.SetRequestHeader("AUTHORIZATION", authKeyFile.text);

        yield return request.SendWebRequest();

        if (request.isNetworkError || request.isHttpError)
        {
            Debug.Log(request.error);
        }
        else
        {
            yield return new WaitForSeconds(1);

            Debug.Log("Form download complete! Here's the response:\n" + request.downloadHandler.text);
        }

        _isGetting = false;
    }

    IEnumerator GetRequest(string uri)
    {
        UnityWebRequest webRequest = UnityWebRequest.Get(uri);

        // Request and wait for the desired page.
        yield return webRequest.SendWebRequest();

        string[] pages = uri.Split('/');
        int page = pages.Length - 1;

        if (webRequest.isNetworkError)
        {
            Debug.Log(pages[page] + ": Error: " + webRequest.error);
        }
        else
        {
            Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
        }
    }

    static string GetMd5Hash(MD5 md5Hash, string input)
    {

        // Convert the input string to a byte array and compute the hash.
        byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

        // Create a new Stringbuilder to collect the bytes
        // and create a string.
        StringBuilder sBuilder = new StringBuilder();

        // Loop through each byte of the hashed data
        // and format each one as a hexadecimal string.
        for (int i = 0; i < data.Length; i++)
        {
            sBuilder.Append(data[i].ToString("x2"));
        }

        // Return the hexadecimal string.
        return sBuilder.ToString();
    }

    // Verify a hash against a string.
    static bool VerifyMd5Hash(MD5 md5Hash, string input, string hash)
    {
        // Hash the input.
        string hashOfInput = GetMd5Hash(md5Hash, input);

        // Create a StringComparer an compare the hashes.
        StringComparer comparer = StringComparer.OrdinalIgnoreCase;

        if (0 == comparer.Compare(hashOfInput, hash))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
