using System.Runtime.InteropServices;
using UnityEngine;
using TMPro;

public class UsernameLabelBhv : MonoBehaviour
{
    // private fields
#if UNITY_WEBGL && !UNITY_EDITOR
    [DllImport("__Internal")]
    private static extern void SetUsernameOnLaunch();
#endif
    private TextMeshProUGUI _label;

    private void Awake()
    {
        _label = this.GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
#if UNITY_EDITOR
        _label.text = UserBhv.instance.username;
#elif UNITY_WEBGL
        SetUsernameOnLaunch();
#endif
    }

    public void SetUsername(string username)
    {
        _label.text = username;
    }
}
