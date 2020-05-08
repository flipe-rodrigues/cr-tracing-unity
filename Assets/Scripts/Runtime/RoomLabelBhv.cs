using System.Collections;
using UnityEngine;
using TMPro;

public class RoomLabelBhv : MonoBehaviour
{
    // singleton instance
    public static RoomLabelBhv instance = null;

    // private fields
    private TextMeshProUGUI _label;
    private Color _textColor;

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

        _label = this.GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        _textColor = _label.color;

        _label.color = Color.clear;
    }

    public void Clear()
    {
        _label.color = Color.clear;

        _label.text = "";
    }

    public void SetText(string text, float alpha, float lerpSpeed, Color color)
    {
        _label.text = text;

        StopAllCoroutines();

        StartCoroutine(this.ChangeLabelColor(alpha, lerpSpeed, color));
    }

    private IEnumerator ChangeLabelColor(float alpha, float lerpSpeed, Color color)
    {
        Color currentColor = _label.color;

        Color targetColor = new Color(color.r, color.g, color.b, alpha);

        float lerp = 0;

        while (lerp < 1)
        {
            _label.color = Color.Lerp(currentColor, targetColor, lerp);

            lerp = Mathf.Clamp01(lerp + Time.deltaTime * lerpSpeed);

            yield return null;
        }

        _label.color = targetColor;
    }
}
