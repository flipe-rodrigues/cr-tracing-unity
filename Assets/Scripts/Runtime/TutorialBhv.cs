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
        foreach(LogoBhv logo in _logoParts)
        {
            logo.Enable();
        }
    }

    public IEnumerator Deselect()
    {
        foreach (LogoBhv logo in _logoParts)
        {
            logo.Disable();

            yield return null;
        }
    }

    public void CheckIfAllAreToggled()
    {
        bool allAreToggled = true;

        foreach (LogoBhv logo in _logoParts)
        {
            allAreToggled = allAreToggled && logo.isToggled;
        }

        float alpha = allAreToggled ? 1f : 0f;

        RoomLabelBhv.instance.SetText("Select a map to begin", alpha, 2.5f, Color.white);
    }
}
