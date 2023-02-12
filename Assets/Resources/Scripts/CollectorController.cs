using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CollectorController : MonoBehaviour
{
    [SerializeField] Camera playerCamera;
    [SerializeField] float maxSchootDistance;
    [SerializeField] int collectablelayer;
    [SerializeField] InventoryUIController inventoryUIController;
    [SerializeField] AudioSource collectAudioSource;
    [SerializeField] List<AudioClip> collectAudioClips;

    private int collectableMask;

    void Start()
    {
        collectableMask = 1 << collectablelayer;
    }

    void Update()
    {
        
    }

    private void OnCollect()
    {
        Vector3 viewDirection = playerCamera.transform.forward;
        Vector3 cameraPosition = playerCamera.transform.position;
        RaycastHit hitInfo;
        if (Physics.Raycast(cameraPosition, viewDirection, out hitInfo, maxSchootDistance, collectableMask))
        {
            string collectedObject = hitInfo.collider.gameObject.name;

            Artifact artifact = hitInfo.collider.gameObject.GetComponent<ArtifactInstance>().artifact;
            inventoryUIController.AddCollectedItem(artifact);
            Destroy(hitInfo.collider.gameObject);
            Debug.DrawLine(cameraPosition, hitInfo.point, Color.blue, 4f);

            collectAudioSource.PlayOneShot(collectAudioClips[Random.Range(0, collectAudioClips.Count)]);
        }
    }
}
