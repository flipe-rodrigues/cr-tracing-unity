using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public abstract class ToggleableBhv : MonoBehaviour, IToggleable
{
    // public properties
    public bool IsEnabled { get { return _graphicsRaycaster.enabled; } }

    // public fields
    public bool isToggled;
    [Range(0f, 1f)]
    public float disabledAlpha = 0f;
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
    }

    public void Enable()
    {
        _canvas.enabled = true;

        _graphicsRaycaster.enabled = true;

        _targetImageColor = this.isToggled ? _toggledImageColor : _untoggledImageColor;

        _targetFontColor = this.isToggled ? toggledFontColor : untoggledFontColor;

        _targetFontSize = this.isToggled ? toggledFontSize : untoggledFontSize;

        this.TweenTowardsTarget(1f, 1f, 5f);
    }

    public void Disable()
    {
        _graphicsRaycaster.enabled = false;

        _targetImageColor = new Color(_image.color.r, _image.color.g, _image.color.b, disabledAlpha);

        _targetFontColor = Color.clear;

        _targetFontSize = untoggledFontSize;

        this.TweenTowardsTarget(1f, 1f, 2.5f);
    }

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        _targetImageColor = this.isToggled ? _image.color : _toggledImageColor;

        _targetFontColor = this.isToggled ? _label.color : untoggledFontColor;

        _targetFontSize = this.isToggled ? _label.fontSize : toggledFontSize;

        this.TweenTowardsTarget(.5f, 1f, 0f);
    }

    public virtual void OnPointerClick(PointerEventData eventData)
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

                _targetImageColor = this.isToggled ? _toggledImageColor : _untoggledImageColor;

                _targetFontColor = this.isToggled ? toggledFontColor : untoggledFontColor;

                _targetFontSize = this.isToggled ? toggledFontSize : untoggledFontSize;

                AudioManager.instance.PlayClip(this.isToggled ? toggleClip : untoggleClip);

                this.TweenTowardsTarget(1f, 5f, 0f);
            }
        }
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        _targetImageColor = this.isToggled ? _targetImageColor : _untoggledImageColor;

        _targetFontColor = this.isToggled ? _targetFontColor : untoggledFontColor;

        _targetFontSize = this.isToggled ? _targetFontSize : untoggledFontSize;

        this.TweenTowardsTarget(1f, 5f, 0f);
    }

    private void TweenTowardsTarget(float lerpCeil, float lerpSpeed, float phaseOffset)
    {
        StopAllCoroutines();

        StartCoroutine(this.LerpImageColor(lerpCeil, lerpSpeed, phaseOffset));

        StartCoroutine(this.LerpLabelColor(lerpCeil, lerpSpeed, phaseOffset));

        StartCoroutine(this.LerpLabelFontSize(lerpCeil, lerpSpeed, phaseOffset));
    }

    private IEnumerator Flash(float lerpSpeed)
    {
        Color imageColor = _image.color;

        Color labelColor = _label.color;

        float interpolant = 0;

        while (interpolant < 1)
        {
            interpolant = Mathf.Clamp01(interpolant + Time.deltaTime * lerpSpeed);

            _image.color = Color.Lerp(imageColor, _untoggledImageColor, interpolant);

            _label.color = Color.Lerp(labelColor, untoggledFontColor, interpolant);

            yield return null;
        }

        interpolant = 0;

        float maxLerp = this.isToggled ? 1f : .5f;

        imageColor = this.isToggled ? _toggledImageColor : _toggledImageColor;

        labelColor = this.isToggled ? toggledFontColor : untoggledFontColor;

        while (interpolant < maxLerp)
        {
            interpolant = Mathf.Clamp01(interpolant + Time.deltaTime * lerpSpeed);

            _image.color = Color.Lerp(_untoggledImageColor, imageColor, interpolant);

            _label.color = Color.Lerp(untoggledFontColor, labelColor, interpolant);

            yield return null;
        }
    }

    private IEnumerator LerpImageColor(float lerpCeil, float lerpSpeed, float phaseOffset)
    {
        yield return new WaitForSeconds(UnityEngine.Random.value * phaseOffset);

        Color currentColor = _image.color;

        float interpolant = 0;

        while (interpolant < lerpCeil)
        {
            interpolant = Mathf.Clamp01(interpolant + Time.deltaTime * lerpSpeed);

            _image.color = Color.Lerp(currentColor, _targetImageColor, interpolant);

            yield return null;
        }
    }

    private IEnumerator LerpLabelColor(float lerpCeil, float lerpSpeed, float phaseOffset)
    {
        yield return new WaitForSeconds(UnityEngine.Random.value * phaseOffset);

        Color currentColor = _label.color;

        float interpolant = 0;

        while (interpolant < lerpCeil)
        {
            interpolant = Mathf.Clamp01(interpolant + Time.deltaTime * lerpSpeed);

            _label.color = Color.Lerp(currentColor, _targetFontColor, interpolant);

            yield return null;
        }
    }

    private IEnumerator LerpLabelFontSize(float lerpCeil, float lerpSpeed, float phaseOffset)
    {
        yield return new WaitForSeconds(UnityEngine.Random.value * phaseOffset);

        float currentSize = _label.fontSize;

        float interpolant = 0;

        while (interpolant < lerpCeil)
        {
            interpolant = Mathf.Clamp01(interpolant + Time.deltaTime * lerpSpeed);

            _label.fontSize = Mathf.Lerp(currentSize, _targetFontSize, interpolant);

            yield return null;
        }
    }
}