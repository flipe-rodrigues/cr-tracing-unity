using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialBhv : MonoBehaviour, IMap
{
    // public fields
    public Color mapColor;

    // private fields
    private LogoBhv[] _logoParts;

    private void Awake()
    {
        _logoParts = this.GetComponentsInChildren<LogoBhv>();
    }

    public string GetName()
    {
        return this.name;
    }

    public Color GetColor()
    {
        return this.mapColor;
    }

    public void Select()
    {
        for (int i = 0; i < _logoParts.Length; i++)
        {
            _logoParts[i].Enable();
        }
    }

    public IEnumerator Deselect()
    {
        for (int i = 0; i < _logoParts.Length; i++)
        {
            _logoParts[i].Disable();

            yield return null;
        }
    }
}
