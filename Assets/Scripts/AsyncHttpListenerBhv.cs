using System.IO;
using System.Net;
using System.Collections;
using System.Collections.Specialized;
using UnityEngine;
using UnityEngine.Networking;
using System.Security.Cryptography;
using System.Runtime.Serialization.Formatters.Binary;

public class AsyncHttpListenerBhv : MonoBehaviour
{
    public string uri = "https://tracinccu.xyz/test.php";
    private HttpListener listener;
    private string requestBody;
    private object callback;

    private void Start()
    {
        this.StartHttpListener();

        this.ListenForWebRequestAsync();

        StartCoroutine(this.Upload());
    }

    private IEnumerator Upload()
    {
        WWWForm form = new WWWForm();

        form.AddField("user", "Manel");
        foreach (RoomBhv room in UserBhv.instance.rooms)
        {
            Debug.Log(room.name + " " + room.isToggled.ToString());
            form.AddField(room.name, room.isToggled.ToString());
        }
        //UserData data = new UserData(UserBhv.instance);
        //data = SaveSystem.LoadUserData();
        //BinaryFormatter formatter = new BinaryFormatter();
        //MemoryStream stream = new MemoryStream();
        //formatter.Serialize(stream, data); 
        //byte[] dataArray = stream.ToArray();

        //byte[] arr = new byte[4];
        //arr[0] = 00000001;
        //arr[1] = 00000001;
        //arr[2] = 00000001;
        //arr[3] = 00000001;
        //Debug.Log(System.Convert.ToBase64String(arr));

        //form.AddField("raw_data","this should be a byte[]");
        //form.AddBinaryData("raw_data", arr);
        //stream.Close();

        UnityWebRequest request = UnityWebRequest.Post(uri, form);

        request.SetRequestHeader("AUTHORIZATION", "Basic dHJhY2luZzp0cmFjaW5nY2N1MTIz");

        yield return request.SendWebRequest();

        if (request.isNetworkError || request.isHttpError)
        {
            Debug.Log(request.error);
        }
        else
        {
            yield return new WaitForSeconds(1);

            Debug.Log("Form upload complete!" +
                request.downloadHandler.data + " " + request.downloadHandler.text + " " + request.GetRequestHeader(""));
        }
    }

    //----- POST type call
    //public void PostDataToURL(string url, WWWForm data, downloadData callback, bool showLoading = true)
    //{
    //    StartCoroutine(PostDataToURLCR(url, data, callback, showLoading));
    //}

    //public delegate void downloadData(byte[] data, string dataAsString, string error = "");

    //IEnumerator PostDataToURLCR(string url, WWWForm data, downloadData callback, bool showLoading = true)
    //{
    //    url = baseURL + url;
    //    Debug.Log("POST with: " + url);

    //    request.SetRequestHeader("SESSIONID", "0");

    //    yield return request.SendWebRequest();

    //    if (request.isNetworkError || request.isHttpError)
    //    {
    //        callback.Invoke(null, "", request.error);
    //    }
    //    else
    //    {
    //        callback.Invoke(request.downloadHandler.data, request.downloadHandler.text, "");
    //    }
    //}

    private void StartHttpListener()
    {
        listener = new HttpListener();

        listener.Prefixes.Add("http://localhost:4444/");

        listener.Prefixes.Add("http://127.0.0.1:4444/");

        listener.AuthenticationSchemes = AuthenticationSchemes.Anonymous;

        listener.Start();
    }

    private async void ListenForWebRequestAsync()
    {
        while (listener.IsListening)
        {
            HttpListenerContext context = await listener.GetContextAsync();

            HttpListenerRequest request = context.Request;

            if (request.HttpMethod == "POST")
            {
                this.LogAllKeyValuePairs(request.QueryString);

                StreamReader reader = new StreamReader(request.InputStream, request.ContentEncoding);

                requestBody = reader.ReadToEnd();
            }

            this.SendResponse(context);

            listener.Stop();
        }
    }

    private void LogAllKeyValuePairs(NameValueCollection query)
    {
        foreach (string key in query.AllKeys)
        {
            string value = query.GetValues(key)[0];

            Debug.Log(key + " : " + value);
        }
    }

    private void SendResponse(HttpListenerContext context)
    {
        HttpListenerResponse response = context.Response;

        string responseString = "Message received";

        byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);

        response.ContentLength64 = buffer.Length;

        Stream output = response.OutputStream;

        output.Write(buffer, 0, buffer.Length);

        output.Close();
    }
}