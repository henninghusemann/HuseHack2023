using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralRootGrower : MonoBehaviour
{
    [SerializeField] int steps;
    [SerializeField] float stepSize;
    [SerializeField] float randomness;
    [SerializeField] float startRadius;
    [SerializeField] int overgrowableLayer;
    [SerializeField] int rootCylinderSteps;
    [SerializeField] int rootCount;
    [SerializeField] Material rootMaterial;

    private int overgrowableMask;

    void Start()
    {

        for(int i = 0; i < rootCount; i++)
        {
            List<Vector3> positions = Grow(transform.position, Vector3.up, steps, stepSize, randomness, true);

            BuildObject(positions, startRadius);

            for (int j = 0; j < 8; j++)
            {
                int startIndex = Random.Range(0, positions.Count);
                Vector3 randomOrig = positions[startIndex];

                List<Vector3> branchPositions = Grow(randomOrig, Random.insideUnitSphere, steps / 4, stepSize, randomness, true);
                
                float adaptRadius = StepToRadius(startIndex, steps, startRadius);

                BuildObject(branchPositions, adaptRadius);
            }
        }
    }

    float StepToRadius(int index, int count, float startRadius)
    {
        float alpha = (float)(index + 1f) / count;
        return Mathf.Lerp(startRadius, 0.01f, alpha);
    }

    private GameObject BuildObject(List<Vector3> positions, float startRadius)
    {
        Mesh m;
        m = BuildMeshHull(positions, rootCylinderSteps, startRadius);

        GameObject go = new GameObject();
        MeshFilter mf = go.AddComponent<MeshFilter>();
        mf.mesh = m;
        MeshRenderer mr = go.AddComponent<MeshRenderer>();
        mr.material = rootMaterial;
        go.AddComponent<MeshCollider>();
        return go;
    }

    List<Vector3> Grow(Vector3 origin, Vector3 direction, int steps, float stepSize, float randomness, bool ground)
    {
        List<Vector3> positions = new List<Vector3>();
        positions.Add(origin);
        overgrowableMask = 1 << overgrowableLayer;
        Vector3 position = origin;

        Vector3 GrowDirection = direction;

        for (int i = 0; i < steps; i++)
        {
            position += GrowDirection * stepSize;

            if (ground)
            {
                Vector3 groundedPos = FindGround(position + Vector3.up);
                //groundedPos += 0.5f * StepToRadius(i, steps, startRadius) * Vector3.up;
                position = groundedPos;
                //GrowDirection = Vector3.Slerp(GrowDirection,(groundedPos - position).normalized,0.25f);
            }

            GrowDirection = Vector3.Slerp(GrowDirection, Random.insideUnitSphere, randomness);
            positions.Add(position);
        }
        return positions;
    }

    Vector3 FindGround(Vector3 pos)
    {
        RaycastHit hit;
        if (Physics.Raycast(pos, Vector3.down, out hit, float.MaxValue, overgrowableMask))
        {
            return hit.point;
        }
        return pos;
    }

    private Mesh BuildMeshHull(List<Vector3> positions, int circleSteps, float radius)
    {
        int positionCount = positions.Count;
        Vector3[] vertices = new Vector3[positionCount * circleSteps];
        Vector3[] normals = new Vector3[positionCount * circleSteps];
        Vector2[] uvs = new Vector2[positionCount * circleSteps];

        for(int i = 0; i < positionCount; i++)
        {
            //float radiusFade = 1f - (float)(i+1f)/positionCount;
            float radiusFade = StepToRadius(i, positionCount, radius);

            Vector3 fwd;
            if (i < positionCount - 1)
            {
                fwd = (positions[i + 1] - positions[i]).normalized;
            }
            else
            {
                fwd = (positions[i] - positions[i - 1]).normalized;
            }

            Quaternion rotator = Quaternion.AngleAxis(360f / (circleSteps - 1), fwd);
            
            Vector3 offset = Vector3.Cross(fwd, Vector3.down) * radiusFade;

            for(int j = 0; j < circleSteps; j++)
            {
                offset = rotator * offset;
                vertices[i * circleSteps + j] = positions[i] + offset;

                float u = (float)(j) / (circleSteps + 1f);
                float v = (float)(i);
                uvs[i * circleSteps + j] = new Vector2(u,v);
                normals[i * circleSteps + j] = offset.normalized;
            }
        }

        int vertexCount = vertices.Length;
        int[] triangles = new int[vertexCount * 6];// + positionCount * 2 * 6];
        int triangleIdx = 0;

        int vi;
        for(int i = 0; i < positionCount - 1; i++)
        {
            for(int j = 0; j < circleSteps - 1; j++)
            {
                vi = i * circleSteps + j;

                triangles[triangleIdx]      = vi + 1;
                triangles[triangleIdx + 1]  = vi + circleSteps;
                triangles[triangleIdx + 2]  = vi;

                triangles[triangleIdx + 3]  = vi + circleSteps + 1;
                triangles[triangleIdx + 4]  = vi + circleSteps;
                triangles[triangleIdx + 5]  = vi + 1;

                triangleIdx += 6;
            }
        }

        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;
        mesh.normals = normals;
        return mesh;
    }


    void Update()
    {
        
    }
}
