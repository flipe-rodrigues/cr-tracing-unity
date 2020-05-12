using System.Text;
using System.Collections;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkManager : MonoBehaviour
{
    // public fields
    public string uri;
    public string authKeyFileName;

    // private fields
    private string _authKey;
    private bool _isPosting;

    private void Start()
    {
        this.LoadAuthenticationKey();
    }

    private void LoadAuthenticationKey()
    {
        TextAsset authenticationKeyFile = Resources.Load<TextAsset>(authKeyFileName);

        _authKey = authenticationKeyFile.text;
    }

    public void OnSubmit()
    {
        if (!_isPosting)
        {
            StartCoroutine(this.PostUserData());
        }
    }

    private IEnumerator PostUserData()
    {
        SubmitButtonBhv.instance.DisableButton();

        SubmitButtonBhv.instance.SetToTransition();

        _isPosting = true;

        WWWForm form = new WWWForm();

        SHA256 sha256Hash = SHA256.Create();

        string userHash = GetSha256Hash(sha256Hash, UserBhv.instance.username);

        form.AddField("user", userHash);

        sha256Hash.Dispose();

        foreach (RoomBhv room in UserBhv.instance.rooms)
        {
            form.AddField(room.roomData.server_id, room.isToggled ? "1" : "0");
        }

        UnityWebRequest request = UnityWebRequest.Post(uri, form);

        request.SetRequestHeader("AUTHORIZATION", _authKey);

        yield return request.SendWebRequest();

        if (request.isNetworkError || request.isHttpError)
        {
            SubmitButtonBhv.instance.SetToError();

            Debug.Log(request.error);
        }
        else
        {
            SubmitButtonBhv.instance.SetToSuccess();

            Debug.Log("Form upload complete!\n" + request.downloadHandler.text);
        }

        _isPosting = false;
    }

    static string GetSha256Hash(SHA256 hash, string input)
    {
        byte[] data = hash.ComputeHash(Encoding.UTF8.GetBytes(input));

        StringBuilder stringBuilder = new StringBuilder();

        for (int i = 0; i < data.Length; i++)
        {
            stringBuilder.Append(data[i].ToString("x2"));
        }

        return stringBuilder.ToString();
    }
}
