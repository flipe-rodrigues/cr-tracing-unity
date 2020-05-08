using UnityEngine;
using TMPro;

public class FPSLabelBhv : MonoBehaviour
{
    // private fields
    private TextMeshProUGUI _label;

    private void Awake()
    {
        _label = this.GetComponent<TextMeshProUGUI>();
    }

    private void LateUpdate()
    {
        _label.text = Mathf.RoundToInt(1f / Time.deltaTime).ToString() + " FPS";
    }
}
