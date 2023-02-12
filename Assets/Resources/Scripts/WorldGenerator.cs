using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGenerator : MonoBehaviour
{
    [SerializeField] Texture2D mapData;
    [SerializeField] int tileSizeFactor;
    object[] artifactObjects;
    object[] jungleObjects;
    object[] grassObjects;
    object[] statueObjects;
    object[] templeLargeObjects;
    object[] templeSmallObjects;
    object[] treeObjects;
    object[] waterObjects;

    private Vector3 calcPosition(int xField, int yField, float posVariance)
    {
        return new Vector3((xField + Random.Range(-posVariance, posVariance)) * tileSizeFactor, 0f, (yField + Random.Range(-posVariance, posVariance)) * tileSizeFactor);
    }

    private void positionObject(GameObject go, int x, int y, float posVariance)
    {
        go.transform.position = calcPosition(x, y, posVariance);
        go.transform.Rotate(0, 0, Random.Range(0f, 360f));
    }

    private void applyRandomScale(GameObject go, float min, float max)
    {
        float randScale = Random.Range(min, max);
        Vector3 vec = go.transform.localScale;
        vec.x *= randScale;
        vec.y *= randScale;
        vec.z *= randScale;
        go.transform.localScale = vec;
    }

    private void generateGrass(int x, int y)
    {
        for (int i = 0; i < 5; i++)
        {
            GameObject go = Instantiate((GameObject) grassObjects[Random.Range(0, grassObjects.Length)]);
            positionObject(go, x, y, 0.4f);
            applyRandomScale(go, 0.5f, 0.8f);
        }
    }

    private void generateJungle(int x, int y)
    {
        float category = Random.Range(0f, 1f);
        if (category < 0.3f)
        {
            GameObject go = Instantiate((GameObject) treeObjects[Random.Range(0, treeObjects.Length)]);
            positionObject(go, x, y, 0.3f);
        }
        else if (category < 0.4f)
        {
            GameObject go = Instantiate((GameObject) jungleObjects[Random.Range(0, jungleObjects.Length)]);
            positionObject(go, x, y, 0.3f);
        }
        else {
            for (int i = 0; i < 6; i++)
            {
                GameObject go = Instantiate((GameObject) grassObjects[Random.Range(0, grassObjects.Length)]);
                positionObject(go, x, y, 0.4f);
                applyRandomScale(go, 0.9f, 1.3f);
            }
        }
    }

    private void generateTempleLarge(int x, int y)
    {
        GameObject go = Instantiate((GameObject) templeLargeObjects[Random.Range(0, templeLargeObjects.Length)]);
        go.transform.position = calcPosition(x, y, 0.0f);
    }
    
    private void generateTempleSmall(int x, int y)
    {
        GameObject go = Instantiate((GameObject) templeSmallObjects[Random.Range(0, templeSmallObjects.Length)]);
        go.transform.position = calcPosition(x, y, 0.0f);
    }

    private void generateWater(int x, int y)
    {
        GameObject go = Instantiate((GameObject) waterObjects[Random.Range(0, waterObjects.Length)]);
        go.transform.position = calcPosition(x, y, 0);
        go.transform.localScale = new Vector3(tileSizeFactor, 1, tileSizeFactor);
    }

    private string simplifyColor(Color32 color)
    {
        return color.r.ToString("x2").Substring(0, 1) + color.g.ToString("x2").Substring(0, 1) + color.b.ToString("x2").Substring(0, 1);
    }

    void Start()
    {
        LoadPrefabs();
        var hexColorToTileType = (
            empty: "fff",
            grass: "0a0",
            jungle: "050",
            templeLarge: "f50",
            templeSmall: "fa0",
            water: "0af"
        );
        var mapArray = mapData.GetPixels32();
        for (var i = 0; i < mapArray.Length; i++)
        {
            var x = i % mapData.width;
            var y = i / mapData.height;

            var tileHexColor = simplifyColor(mapArray[i]).ToLower();
            if (tileHexColor == hexColorToTileType.empty)
            {
                
            }
            else if (tileHexColor == hexColorToTileType.grass)
            {
                generateGrass(x, y);
            }
            else if (tileHexColor == hexColorToTileType.jungle)
            {
                generateJungle(x, y);
            }
            else if (tileHexColor == hexColorToTileType.templeLarge)
            {
                generateTempleLarge(x, y);
            }
            else if (tileHexColor == hexColorToTileType.templeSmall)
            {
                generateTempleSmall(x, y);
            }  
            else if (tileHexColor == hexColorToTileType.water)
            {
                generateWater(x, y);
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
        artifactObjects = Resources.LoadAll("Prefabs/ArtifactObjects");
        grassObjects = Resources.LoadAll("Prefabs/GrassObjects");
        jungleObjects = Resources.LoadAll("Prefabs/JungleObjects");
        statueObjects = Resources.LoadAll("Prefabs/StatueObjects");
        templeLargeObjects = Resources.LoadAll("Prefabs/TempleLargeObjects");
        templeSmallObjects = Resources.LoadAll("Prefabs/TempleSmallObjects");
        treeObjects = Resources.LoadAll("Prefabs/TreeObjects");
        waterObjects = Resources.LoadAll("Prefabs/WaterObjects");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
