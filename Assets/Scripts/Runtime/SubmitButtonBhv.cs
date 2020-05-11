using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SubmitButtonBhv : MonoBehaviour
{
    // singleton instance
    public static SubmitButtonBhv instance = null;

    // public fields
    public string enabledText = "submit";
    public string transitionText = "submitting..";
    public string disabledText = "submitted";
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

    public void SetTextToEnabled()
    {
        _label.text = enabledText;
    }

    public void SetTextToTransition()
    {
        _label.text = transitionText;
    }

    public void SetTextToDisabled()
    {
        _label.text = disabledText;
    }
}
