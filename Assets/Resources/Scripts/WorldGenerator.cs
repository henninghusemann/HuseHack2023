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
        GameObject go = Instantiate((GameObject) waterObjects[Random.Range(0, waterObjects.Length)]);
        go.transform.position = new Vector3(x, 0f, y);
        go.transform.Rotate(-90, 0, 0);
    }

    private void generateJungle(int x, int y)
    {
        GameObject go = Instantiate((GameObject) jungleObjects[Random.Range(0, jungleObjects.Length)]);
        go.transform.position = new Vector3(x + Random.Range(-0.3f, 0.3f), 0f, y + Random.Range(-0.3f, 0.3f));
        go.transform.Rotate(-90, 0, 0);
        go.transform.localScale = new Vector3(6, 6, 10);
    }

    private void generateGrass(int x, int y)
    {
        for (int i = 0; i < 4; i++)
        {
            GameObject go = Instantiate((GameObject) grassObjects[Random.Range(0, grassObjects.Length)]);
            go.transform.position = new Vector3(x + Random.Range(-0.4f, 0.4f), 0f, y + Random.Range(-0.4f, 0.4f));
            go.transform.Rotate(-90, 0, 0);
            go.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        }
    }

    private void generateTemple(int x, int y)
    {
        GameObject go = Instantiate((GameObject) templeObjects[Random.Range(0, templeObjects.Length)]);
        go.transform.position = new Vector3(x + Random.Range(-0.2f, 0.2f), 0f, y + Random.Range(-0.2f, 0.2f));
        go.transform.Rotate(-90, 0, 0);
        go.transform.localScale = new Vector3(1, 1, Random.Range(1, 4));
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
