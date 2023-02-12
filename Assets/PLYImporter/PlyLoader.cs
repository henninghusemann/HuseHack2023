using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlyLoader : MonoBehaviour
{
    [SerializeField] string folderPath;
    [SerializeField] Material vertexColorMaterial;

    void Start()
    {
        List<GameObject> allObjects = PLYImporter.LoadAllPLYObjectsAtPath(folderPath);
        foreach(GameObject go in allObjects)
        {
            go.GetComponent<MeshRenderer>().material = vertexColorMaterial;
            Vector2 offset = Random.insideUnitCircle;
            go.transform.position = transform.position + new Vector3(offset.x,0f,offset.y) * 4f;
        }
    }

    void Update()
    {
        
    }
}
