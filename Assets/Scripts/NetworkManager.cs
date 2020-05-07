using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkManager : MonoBehaviour
{
    // public fields
    public string uri = "https://tracinccu.xyz/test.php";
    public string authKey = "Basic dHJhY2luZzp0cmFjaW5nY2N1MTIz";

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

        form.AddField("user", UserBhv.instance.username);

        foreach (RoomBhv room in UserBhv.instance.rooms)
        {
            form.AddField(room.name, room.isToggled.ToString());
        }

        UnityWebRequest request = UnityWebRequest.Post(uri, form);

        request.SetRequestHeader("AUTHORIZATION", authKey);

        yield return request.SendWebRequest();

        if (request.isNetworkError || request.isHttpError)
        {
            Debug.Log(request.error);
        }
        else
        {
            yield return new WaitForSeconds(1);

            Debug.Log("Form upload complete! Here's the response:\n" + request.downloadHandler.text);
        }

        _isPosting = false;
    }

    private IEnumerator GetUserData()
    {
        _isGetting = true;

        UnityWebRequest request = UnityWebRequest.Get(uri);

        request.SetRequestHeader("AUTHORIZATION", authKey);

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
}
