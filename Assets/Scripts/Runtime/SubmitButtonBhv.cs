using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SubmitButtonBhv : MonoBehaviour
{
    // singleton instance
    public static SubmitButtonBhv instance = null;

    // public fields
    [Header("Label settings:")]
    public string idleText = "submit";
    public string transitionText = "submitting..";
    public string successText = "success";
    public string errorText = "error";
    [Header("Audio settings:")]
    public AudioClip clickClip;

    // private fields
    private Button _button;
    private TextMeshProUGUI _label;

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

        _button = this.GetComponent<Button>();

        _label = this.GetComponentInChildren<TextMeshProUGUI>();
    }

    private void Start()
    {
        _button.onClick.AddListener(this.OnClick);

        this.DisableButton();
    }

    private void OnClick()
    {
        AudioManager.instance.PlayClip(clickClip);
    }

    public void EnableButton()
    {
        _button.interactable = true;
    }

    public void DisableButton()
    {
        _button.interactable = false;
    }

    public void SetToIdle()
    {
        _label.text = idleText;
    }

    public void SetToTransition()
    {
        _label.text = transitionText;
    }

    public void SetToSuccess()
    {
        _label.text = successText;
    }

    public void SetToError()
    {
        _label.text = errorText;
    }
}
