﻿using UnityEngine;
using TMPro;

public class MapSelectionBhv : MonoBehaviour
{
    // private fields
    private TMP_Dropdown _mapDropdown;

    private void Awake()
    {
        _mapDropdown = this.GetComponent<TMP_Dropdown>();
    }

    private void Start()
    {
        this.PopulateOptions();

        _mapDropdown.value = _mapDropdown.options.Count - 1;
    }

    private void PopulateOptions()
    {
        _mapDropdown.options.Clear();

        foreach (IMap map in MapManager.instance.Maps)
        {
            TMP_Dropdown.OptionData option = new TMP_Dropdown.OptionData(map.GetName());

            _mapDropdown.options.Add(option);
        }
    }

    public void OnMapSelection()
    {
        int mapIndex = _mapDropdown.value;

        MapManager.instance.SelectMap(mapIndex);

        VirtualCameraManager.instance.SelectVirtualCamera(mapIndex);

        RoomLabelBhv.instance.Clear();
    }
}
