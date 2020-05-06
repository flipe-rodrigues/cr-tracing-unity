using UnityEngine;

public class MapManager : MonoBehaviour
{
    // singleton instance
    public static MapManager instance = null;

    // public properties
    public MapBhv[] Maps { get; private set; }

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

        this.Maps = this.GetComponentsInChildren<MapBhv>();
    }

    public void SelectMap(int mapIndex)
    {
        this.Maps[mapIndex].Select();

        Debug.Log(mapIndex);

        for (int i = 0; i < this.Maps.Length; i++)
        {
            if (i != mapIndex)
            {
                StartCoroutine(this.Maps[i].Deselect());
            }
        }
    }
}
