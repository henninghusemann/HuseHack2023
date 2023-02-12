using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ThreeDeeBear.Models.Ply;

public static class PLYImporter
{
    public static List<GameObject> LoadAllPLYObjectsAtPath(string folderPath)
    {
        List<GameObject> loadedObjects = new List<GameObject>();
        string[] files = System.IO.Directory.GetFiles(folderPath);
        foreach (string file in files)
        {
            Debug.Log("Try load: " + file);
            if (!file.EndsWith(".meta"))
            {
                loadedObjects.Add(PlyToGameObject(file));
            }
        }
        return loadedObjects;
    }

    private static GameObject PlyToGameObject(string modelPath)
    {
        PlyResult result = PlyHandler.GetVerticesAndTriangles(modelPath);

        Mesh mesh = new Mesh();

        mesh.vertices = result.Vertices.ToArray();
        mesh.triangles = result.Triangles.ToArray();
        mesh.colors = result.Colors.ToArray();
        mesh.RecalculateNormals();

        GameObject go = new GameObject();

        MeshFilter mf = go.AddComponent<MeshFilter>();
        mf.mesh = mesh;

        go.AddComponent<MeshRenderer>();
        return go;
    }
}
