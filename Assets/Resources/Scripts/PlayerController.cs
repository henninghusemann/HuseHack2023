using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Camera playerCamera;
    [SerializeField] float WalkingSpeed;
    [SerializeField] float SprintSpeed;
    [SerializeField] float PanSensitivity;
    [SerializeField] int walkablelayer;
    [SerializeField] float JumpStrength;
    [SerializeField] InventoryUIController inventoryUIController;
    
    private Vector2 currentMovement;
    private float currentSpeed;
    private int walkableMask;
    private Rigidbody playerRigidbody;


    void Start()
    {
        currentSpeed = WalkingSpeed;
        walkableMask = 1 << walkablelayer;
        playerRigidbody = GetComponent<Rigidbody>();
        playerCamera.transform.forward = Vector3.forward;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.forward * currentMovement.y * currentSpeed * Time.deltaTime;
        transform.position += transform.right * currentMovement.x * currentSpeed * Time.deltaTime;
    }

    private void OnMove(InputValue value)
    {
        var v = value.Get<Vector2>();
        currentMovement = new Vector2(v.x, v.y);
    }

    private void OnLook(InputValue value)
    {
        var v = value.Get<Vector2>();
        v *= PanSensitivity;

        playerCamera.transform.Rotate(-v.y, 0f, 0f, Space.Self);
        float pitchAngle = playerCamera.transform.localEulerAngles.x;
        playerCamera.transform.localEulerAngles = new Vector3(pitchAngle, 0f, 0f);

        transform.Rotate(0f, v.x, 0f,Space.Self);
    }

    private void OnSprint(InputValue value)
    {
        var v = value.Get<float>();
        if (v > 0.5f)
        {
            currentSpeed = SprintSpeed;
        }
        else
        {
            currentSpeed = WalkingSpeed;
        }
    }

    private void OnJump(InputValue value)
    {
        var v = value.Get<float>();
        if (v > 0.5f && !IsJumping())
        {
            playerRigidbody.AddForce(Vector3.up * JumpStrength, ForceMode.Impulse);
        }
    }

    private bool IsJumping()
    {
        RaycastHit hitInfo;
        if (Physics.Raycast(playerCamera.transform.position, Vector3.down, out hitInfo, float.MaxValue, walkableMask))
        {
            if (hitInfo.distance > 2.1f)
            {
                return true;
            }
        }
        return false;
    }

    private void OnToggleInventory()
    {
        inventoryUIController.ToggleVisibility();
    }
}
