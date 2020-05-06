using System;
using UnityEngine;

[Serializable]
public class RoomTypeColors
{
    public string typeName;
    public Color toggledColor;
    public Color untoggledColor;
}

public enum RoomType { Access, Room, Node }