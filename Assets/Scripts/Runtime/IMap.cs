using System.Collections;
using UnityEngine;

public interface IMap
{
    string GetName();
    Color GetColor();
    IEnumerator Deselect();
    void Select();
}