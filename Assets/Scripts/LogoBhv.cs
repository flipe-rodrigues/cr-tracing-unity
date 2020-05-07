using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class LogoBhv : MonoBehaviour, IToggleable
{
    // public fields
    public bool isToggled;
    [Header("Image Settings:")]
    public Color toggledImageColor;
    public Color untoggledImageColor;
    [Header("Label Settings:")]
    public Color toggledFontColor;
    public Color untoggledFontColor;
    public float toggledFontSize = .5f;
    public float untoggledFontSize = .4f;
    [Header("Audio Settings:")]
    public AudioClip toggleClip;
    public AudioClip untoggleClip;

    // private fields
    private GraphicRaycaster _graphicsRaycaster;
    private Image _image;
    private TextMeshProUGUI[] _labels;
    private Color _targetImageColor;
    private Color _targetFontColor;
    private float _targetFontSize;

    private void Awake()
    {
        _graphicsRaycaster = this.GetComponent<GraphicRaycaster>();

        _image = this.GetComponentInChildren<Image>();

        _labels = this.GetComponentsInChildren<TextMeshProUGUI>();
    }

    private void Start()
    {
        _image.alphaHitTestMinimumThreshold = 1f;
    }


    public void Enable()
    {
        _graphicsRaycaster.enabled = true;

        _targetImageColor = this.isToggled ? toggledImageColor : untoggledImageColor;

        _targetFontColor = this.isToggled ? toggledFontColor : untoggledFontColor;

        _targetFontSize = this.isToggled ? toggledFontSize : untoggledFontSize;

        this.TweenTowardsTarget(1f, 1f);
    }

    public void Disable()
    {
        _graphicsRaycaster.enabled = false;

        _targetImageColor = new Color(_image.color.r, _image.color.g, _image.color.b, 1f);

        _targetFontColor = Color.clear;

        _targetFontSize = untoggledFontSize;

        this.TweenTowardsTarget(1f, 1f);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _targetImageColor = this.isToggled ? _image.color : toggledImageColor;

        _targetFontColor = this.isToggled ? _labels[0].color : untoggledFontColor;

        _targetFontSize = this.isToggled ? _labels[0].fontSize : toggledFontSize;

        this.TweenTowardsTarget(.5f, 1f);
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

                _targetImageColor = this.isToggled ? toggledImageColor : untoggledImageColor;

                _targetFontColor = this.isToggled ? toggledFontColor : untoggledFontColor;

                _targetFontSize = this.isToggled ? toggledFontSize : untoggledFontSize;

                AudioManager.instance.PlayClip(this.isToggled ? toggleClip : untoggleClip);

                this.TweenTowardsTarget(1f, 5f);
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _targetImageColor = this.isToggled ? _targetImageColor : untoggledImageColor;

        _targetFontColor = this.isToggled ? _targetFontColor : untoggledFontColor;

        _targetFontSize = this.isToggled ? _targetFontSize : untoggledFontSize;

        this.TweenTowardsTarget(1f, 5f);
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

        Color labelColor = _labels[0].color;

        float lerp = 0;

        while (lerp < 1)
        {
            _image.color = Color.Lerp(imageColor, untoggledImageColor, lerp);

            foreach (TextMeshProUGUI label in _labels)
            {
                label.color = Color.Lerp(labelColor, untoggledFontColor, lerp);
            }

            lerp = Mathf.Clamp01(lerp + Time.deltaTime * speed);

            yield return null;
        }

        lerp = 0;

        float maxLerp = this.isToggled ? 1f : .5f;

        imageColor = this.isToggled ? toggledImageColor : toggledImageColor;

        labelColor = this.isToggled ? toggledFontColor : untoggledFontColor;

        while (lerp < maxLerp)
        {
            _image.color = Color.Lerp(untoggledImageColor, imageColor, lerp);

            foreach (TextMeshProUGUI label in _labels)
            {
                label.color = Color.Lerp(untoggledFontColor, labelColor, lerp);
            }

            lerp = Mathf.Clamp01(lerp + Time.deltaTime * speed);

            yield return null;
        }

        _image.color = Color.Lerp(untoggledImageColor, imageColor, maxLerp);

        foreach (TextMeshProUGUI label in _labels)
        {
            label.color = Color.Lerp(untoggledFontColor, labelColor, maxLerp);
        }
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
        Color currentColor = _labels[0].color;

        float lerp = 0;

        while (lerp < maxLerp)
        {
            foreach (TextMeshProUGUI label in _labels)
            {
                label.color = Color.Lerp(currentColor, _targetFontColor, lerp);
            }

            lerp = Mathf.Clamp01(lerp + Time.deltaTime * lerpSpeed);

            yield return null;
        }

        if (maxLerp == 1f)
        {
            foreach (TextMeshProUGUI label in _labels)
            {
                label.color = _targetFontColor;
            }
        }
    }

    private IEnumerator ChangeLabelFontSize(float maxLerp, float lerpSpeed)
    {
        float currentSize = _labels[0].fontSize;

        float lerp = 0;

        while (lerp < maxLerp)
        {
            foreach (TextMeshProUGUI label in _labels)
            {
                label.fontSize = Mathf.Lerp(currentSize, _targetFontSize, lerp);
            }

            lerp = Mathf.Clamp01(lerp + Time.deltaTime * lerpSpeed);

            yield return null;
        }

        if (maxLerp == 1f)
        {
            foreach (TextMeshProUGUI label in _labels)
            {
                label.fontSize = _targetFontSize;
            }
        }
    }
}
