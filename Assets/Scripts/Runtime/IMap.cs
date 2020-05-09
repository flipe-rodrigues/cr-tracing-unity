using System.Collections;
using UnityEngine;

public interface IMap
{
    string GetName();
    Color GetColor();
    void Select();
    void Deselect();
}