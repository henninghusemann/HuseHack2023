using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtifactInstance : MonoBehaviour
{
    public Artifact artifact;

    private void Start()
    {
        Vector2 randPos2D = Random.insideUnitCircle * 10f;
        transform.position = new Vector3(randPos2D.x, 0.5f, randPos2D.y);
    }
}
