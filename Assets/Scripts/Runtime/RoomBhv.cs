using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class RoomBhv : MonoBehaviour, IToggleable
{
    // public properties
    public bool IsEnabled { get { return _graphicsRaycaster.enabled; } }

    // public fields
    [Header("Room Data:")]
    public RoomData roomData;
    public bool isToggled;
    [Header("Image Settings:")]
    public RoomType roomType;
    public List<RoomTypeColors> roomTypeColors;
    [Range(0f, 1f)]
    public float disabledAlpha = .75f;
    [Header("Label Settings:")]
    public Color toggledFontColor;
    public Color untoggledFontColor;
    public float toggledFontSize = .5f;
    public float untoggledFontSize = .4f;
    [Header("Audio Settings:")]
    public AudioClip toggleClip;
    public AudioClip untoggleClip;

    // private fields
    private Canvas _canvas;
    private GraphicRaycaster _graphicsRaycaster;
    private Image _image;
    private TextMeshProUGUI _label;
    private Color _toggledImageColor;
    private Color _untoggledImageColor;
    private Color _targetImageColor;
    private Color _targetFontColor;
    private float _targetFontSize;

    private void Awake()
    {
        _canvas = this.GetComponent<Canvas>();

        _graphicsRaycaster = this.GetComponent<GraphicRaycaster>();

        _image = this.GetComponentInChildren<Image>();

        _label = this.GetComponentInChildren<TextMeshProUGUI>();
    }

    private void Start()
    {
        _graphicsRaycaster.enabled = false;

        this.AssignTypeImageColors();
    }

    private void LateUpdate()
    {
        _canvas.enabled = _image.color.a != 0;
    }

    private void AssignTypeImageColors()
    {
        string typeName = Enum.GetName(typeof(RoomType), roomType);

        _toggledImageColor = roomTypeColors.Find(r => r.typeName == typeName).toggledColor;

        _untoggledImageColor = roomTypeColors.Find(r => r.typeName == typeName).untoggledColor;

        _image.color = new Color(_untoggledImageColor.r, _untoggledImageColor.g, _untoggledImageColor.b, disabledAlpha);
    }

    public void Enable()
    {
        _canvas.enabled = true;

        _graphicsRaycaster.enabled = true;

        _targetImageColor = this.isToggled ? _toggledImageColor : _untoggledImageColor;

        _targetFontColor = this.isToggled ? toggledFontColor : untoggledFontColor;

        _targetFontSize = this.isToggled ? toggledFontSize : untoggledFontSize;

        this.TweenTowardsTarget(1f, 1f);
    }

    public void Disable()
    {
        _graphicsRaycaster.enabled = false;

        _targetImageColor = new Color(_image.color.r, _image.color.g, _image.color.b, disabledAlpha);

        _targetFontColor = Color.clear;

        _targetFontSize = untoggledFontSize;

        this.TweenTowardsTarget(1f, 1f);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _targetImageColor = this.isToggled ? _image.color : _toggledImageColor;

        _targetFontColor = this.isToggled ? _label.color : untoggledFontColor;

        _targetFontSize = this.isToggled ? _label.fontSize : toggledFontSize;

        this.TweenTowardsTarget(.5f, 1f);

        RoomLabelBhv.instance.SetText(roomData.label, 1f, 2.5f, toggledFontColor);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button.ToString() == "Left")
        {
            if (eventData.clickCount == 1)
            {
                StopAllCoroutines();

                StartCoroutine(this.Flash(10f));
            }

            else if (eventData.clickCount == 2)
            {
                this.isToggled = !this.isToggled;

                UserBhv.instance.HasChangedSinceLastSubmission = true;

                _targetImageColor = this.isToggled ? _toggledImageColor : _untoggledImageColor;

                _targetFontColor = this.isToggled ? toggledFontColor : untoggledFontColor;

                _targetFontSize = this.isToggled ? toggledFontSize : untoggledFontSize;

                AudioManager.instance.PlayClip(this.isToggled ? toggleClip : untoggleClip);

                this.TweenTowardsTarget(1f, 5f);
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _targetImageColor = this.isToggled ? _targetImageColor : _untoggledImageColor;

        _targetFontColor = this.isToggled ? _targetFontColor : untoggledFontColor;

        _targetFontSize = this.isToggled ? _targetFontSize : untoggledFontSize;

        this.TweenTowardsTarget(1f, 5f);

        RoomLabelBhv.instance.SetText(roomData.label, 0f, 5f, toggledFontColor);
    }

    private void TweenTowardsTarget(float maxLerp, float lerpSpeed)
    {
        StopAllCoroutines();

        StartCoroutine(this.ChangeImageColor(maxLerp, lerpSpeed));

        StartCoroutine(this.ChangeLabelColor(maxLerp, lerpSpeed));

        StartCoroutine(this.ChangeLabelFontSize(maxLerp, lerpSpeed));
    }

    private IEnumerator Flash(float speed)
    {
        Color imageColor = _image.color;

        Color labelColor = _label.color;

        float lerp = 0;

        while (lerp < 1)
        {
            _image.color = Color.Lerp(imageColor, _untoggledImageColor, lerp);

            _label.color = Color.Lerp(labelColor, untoggledFontColor, lerp);

            lerp = Mathf.Clamp01(lerp + Time.deltaTime * speed);

            yield return null;
        }

        lerp = 0;

        float maxLerp = this.isToggled ? 1f : .5f;

        imageColor = this.isToggled ? _toggledImageColor : _toggledImageColor;

        labelColor = this.isToggled ? toggledFontColor : untoggledFontColor;

        while (lerp < maxLerp)
        {
            _image.color = Color.Lerp(_untoggledImageColor, imageColor, lerp);

            _label.color = Color.Lerp(untoggledFontColor, labelColor, lerp);

            lerp = Mathf.Clamp01(lerp + Time.deltaTime * speed);

            yield return null;
        }

        _image.color = Color.Lerp(_untoggledImageColor, imageColor, maxLerp);

        _label.color = Color.Lerp(untoggledFontColor, labelColor, maxLerp);
    }

    private IEnumerator ChangeImageColor(float maxLerp, float lerpSpeed)
    {
        Color currentColor = _image.color;

        float lerp = 0;

        while (lerp < maxLerp)
        {
            _image.color = Color.Lerp(currentColor, _targetImageColor, lerp);

            lerp = Mathf.Clamp01(lerp + Time.deltaTime * lerpSpeed);

            yield return null;
        }

        if (maxLerp == 1f)
        {
            _image.color = _targetImageColor;
        }
    }

    private IEnumerator ChangeLabelColor(float maxLerp, float lerpSpeed)
    {
        Color currentColor = _label.color;

        float lerp = 0;

        while (lerp < maxLerp)
        {
            _label.color = Color.Lerp(currentColor, _targetFontColor, lerp);

            lerp = Mathf.Clamp01(lerp + Time.deltaTime * lerpSpeed);

            yield return null;
        }

        if (maxLerp == 1f)
        {
            _label.color = _targetFontColor;
        }
    }

    private IEnumerator ChangeLabelFontSize(float maxLerp, float lerpSpeed)
    {
        float currentSize = _label.fontSize;

        float lerp = 0;

        while (lerp < maxLerp)
        {
            _label.fontSize = Mathf.Lerp(currentSize, _targetFontSize, lerp);

            lerp = Mathf.Clamp01(lerp + Time.deltaTime * lerpSpeed);

            yield return null;
        }

        if (maxLerp == 1f)
        {
            _label.fontSize = _targetFontSize;
        }
    }
}
