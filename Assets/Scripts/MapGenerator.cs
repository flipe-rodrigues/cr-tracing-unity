using System.IO;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using TMPro;

public class MapGenerator : MonoBehaviour
{
    // public fields
    public string mapResourcesFolder = "JSON";
    public GameObject roomPrefab;

    public void GenerateMaps()
    {
        List<string> mapPaths = this.GetMapPaths();

        List<string> childNames = this.GetChildNames();

        foreach (string mapPath in mapPaths)
        {
            string mapName = Path.GetFileName(mapPath);

            if (childNames.Contains(mapName))
            {
                Debug.Log("Map '" + mapName + "' already exists.");
            }
            else
            {
                this.GenerateMap(mapName);
            }
        }
    }

    private List<string> GetMapPaths()
    {
        string mapResourcesPath = Path.Combine(Application.dataPath, "Resources", mapResourcesFolder);

        List<string> mapPaths = new List<string>(Directory.GetDirectories(mapResourcesPath));

        return mapPaths;
    }

    private List<string> GetChildNames()
    {
        List<string> childNames = new List<string>(this.transform.childCount);

        foreach (Transform child in this.transform)
        {
            childNames.Add(child.name);
        }

        return childNames;
    }

    private void GenerateMap(string mapName)
    {
        List<RoomData> mapRoomData = this.LoadMapRoomData(mapName);

        this.InstantiateMap(mapName, mapRoomData);
    }

    private List<RoomData> LoadMapRoomData(string mapName)
    {
        List<RoomData> mapRoomData = new List<RoomData>();

        string mapPath = Path.Combine(mapResourcesFolder, mapName);

        TextAsset[] jsonFiles = Resources.LoadAll<TextAsset>(mapPath);

        for (int i = 0; i < jsonFiles.Length; i++)
        {
            RoomData roomData = JsonUtility.FromJson<RoomData>(jsonFiles[i].text);

            mapRoomData.Add(roomData);
        }

        return mapRoomData;
    }

    private void InstantiateMap(string mapName, List<RoomData> mapRoomData)
    {
        GameObject mapObj = new GameObject(mapName);

        mapObj.AddComponent<MapBhv>();

        mapObj.transform.parent = this.transform;

        this.InstantiateRooms(mapRoomData, mapObj.transform);
        
        this.InstantiateVirtualCamera(mapObj.transform);

        mapObj.transform.localScale = new Vector3(1f, 1f, -100f);
    }

    private void InstantiateRooms(List<RoomData> mapRoomData, Transform mapTransform)
    {
        GameObject roomParentObj = new GameObject("Rooms");

        roomParentObj.transform.parent = mapTransform;

        foreach(RoomData roomData in mapRoomData)
        {
            this.InstantiateRoom(roomData, roomParentObj.transform);
        }
    }

    private void InstantiateRoom(RoomData roomData, Transform roomParentTransform)
    {
        GameObject roomObj = Instantiate(roomPrefab, roomParentTransform);

        roomObj.name = roomData.label;

        Vector3 offset = new Vector3(roomData.dimensions.x, -roomData.dimensions.y, 0) / 2f;

        roomObj.transform.position = roomData.position + offset;

        RectTransform[] rectTransforms = roomObj.GetComponentsInChildren<RectTransform>();

        foreach (RectTransform rectTransform in rectTransforms)
        {
            rectTransform.sizeDelta = roomData.dimensions - (Vector2.one - rectTransform.sizeDelta);
        }

        Canvas canvas = roomObj.GetComponent<Canvas>();

        canvas.worldCamera = Camera.main;

        RoomBhv roomBhv = roomObj.GetComponent<RoomBhv>();

        roomBhv.roomData = roomData;

        roomBhv.roomType = (RoomType)System.Enum.Parse(typeof(RoomType), roomData.type);

        TextMeshProUGUI roomLabel = roomBhv.GetComponentInChildren<TextMeshProUGUI>();

        roomLabel.text = roomData.label;

        if (roomData.dimensions.x <= 1)
        {
            roomLabel.rectTransform.Rotate(Vector3.forward, 90f);

            Vector2 dimensions = roomLabel.rectTransform.sizeDelta;

            roomLabel.rectTransform.sizeDelta = new Vector2(dimensions.y, dimensions.x);
        }
    }

    private void InstantiateVirtualCamera(Transform mapTransform)
    {
        GameObject virtuaCameraObj = new GameObject("CM Virtual Camera");

        virtuaCameraObj.transform.parent = mapTransform;

        CinemachineVirtualCamera virtualCamera = virtuaCameraObj.AddComponent<CinemachineVirtualCamera>();

        virtualCamera.Priority = -1;

        virtualCamera.m_Lens.FieldOfView = 90f;

        CinemachineTransposer transposer = virtualCamera.AddCinemachineComponent<CinemachineTransposer>();

        transposer.m_FollowOffset = new Vector3(0, 0, -15f);

        GameObject focusPointObj = new GameObject("Focus Point");

        focusPointObj.transform.parent = mapTransform;

        focusPointObj.transform.position = this.GetMapCenterOfMass(mapTransform);

        virtualCamera.Follow = focusPointObj.transform;

        virtuaCameraObj.AddComponent<VirtualCameraBhv>();
    }

    private Vector3 GetMapCenterOfMass(Transform mapTransform)
    {
        Vector3 centerOfMass = Vector3.zero;

        RoomBhv[] rooms = mapTransform.GetComponentsInChildren<RoomBhv>();
        
        foreach (RoomBhv room in rooms)
        {
            centerOfMass += room.transform.position / rooms.Length;
        }

        return centerOfMass;
    }
}
