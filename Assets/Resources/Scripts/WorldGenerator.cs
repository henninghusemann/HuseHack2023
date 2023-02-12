using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGenerator : MonoBehaviour
{
    [SerializeField] Texture2D mapData;
    object[] waterObjects;
    object[] jungleObjects;
    object[] grassObjects;
    object[] templeObjects;

    private void generateWater(int x, int y)
    {
        GameObject go = Instantiate((GameObject) waterObjects[0]);
        go.transform.position = new Vector3(x, 0f, y);
    }

    private void generateJungle(int x, int y)
    {
        GameObject go = Instantiate((GameObject) jungleObjects[0]);
        go.transform.position = new Vector3(x, 0f, y);
    }

    private void generateGrass(int x, int y)
    {
        GameObject go = Instantiate((GameObject) grassObjects[0]);
        go.transform.position = new Vector3(x, 0f, y);
    }

    private void generateTemple(int x, int y)
    {
        GameObject go = Instantiate((GameObject) templeObjects[0]);
        go.transform.position = new Vector3(x, 0f, y);
    }

    private string simplifyColor(Color32 color)
    {
        return color.r.ToString("x2").Substring(0, 1) + color.g.ToString("x2").Substring(0, 1) + color.b.ToString("x2").Substring(0, 1);
    }

    void Start()
    {
        LoadPrefabs();
        var hexColorToTileType = (
            water: "0af",
            jungle: "050",
            grass: "0a0",
            temple: "fa0"
        );
        var mapArray = mapData.GetPixels32();
        for (var i = 0; i < mapArray.Length; i++)
        {
            var x = i % mapData.width;
            var y = i / mapData.height;

            var tileHexColor = simplifyColor(mapArray[i]).ToLower();
            if (tileHexColor == hexColorToTileType.water)
            {
                generateWater(x, y);
            }
            else if (tileHexColor == hexColorToTileType.jungle)
            {
                generateJungle(x, y);
            }
            else if (tileHexColor == hexColorToTileType.grass)
            {
                generateGrass(x, y);
            }
            else if (tileHexColor == hexColorToTileType.temple)
            {
                generateTemple(x, y);
            }
            else
            {
                Debug.Log(tileHexColor);
                Debug.DrawRay(new Vector3(x, 0f, y), Vector3.up, Color.yellow, 120f);
            }
        }
    }

    private void LoadPrefabs()
    {
        waterObjects = Resources.LoadAll("Prefabs/WaterObjects");
        jungleObjects = Resources.LoadAll("Prefabs/JungleObjects");
        grassObjects = Resources.LoadAll("Prefabs/GrassObjects");
        templeObjects = Resources.LoadAll("Prefabs/TempleObjects");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
