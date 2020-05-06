using System;
using UnityEngine;

[Serializable]
public class RoomTypeColors
{
    public string type;
    public Color toggledColor;
    public Color untoggledColor;
}

public enum RoomType { access, room, node }