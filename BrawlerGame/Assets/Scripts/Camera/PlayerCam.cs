using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerCam : NetworkBehaviour
{
    [Header("Player Cam Vars")]
    [SerializeField] float sensX;
    [SerializeField] float sensY;
    float xRotation;
    float yRotation;
    float mouseX;
    float mouseY;

    [Header("Player Cam Components")]
    [SerializeField] Transform orientation;
    [SerializeField] Transform playerModel;
    [SerializeField] Transform playerArm;


    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsOwner)
        {
            gameObject.SetActive(false);
            return;
        }
        GetPlayerInput();
    }

    private void FixedUpdate()
    {
        if (!IsOwner) return;
        RotatePlayerCamServerRpc(xRotation, yRotation);
    }

    //Gets player mouse input and sets rotation values
    void GetPlayerInput()
    {
        mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
        mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;

        yRotation += mouseX;
        xRotation -= mouseY;
        //clamps xRotation to 
        xRotation = Mathf.Clamp(xRotation, -90, 90);
    }

    //tells the server the players are rotating
    [ServerRpc]
    void RotatePlayerCamServerRpc(float xRotation, float yRotation)
    {
        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);
        playerModel.rotation = Quaternion.Euler(0, yRotation, 0);
    }

    //SETTER METHODS
    public void SetPlayerOrientation(Transform playerOrientation)
    {
        orientation = playerOrientation;
    }

    public void SetPlayerModel(Transform currentPlayerModel)
    {
        playerModel = currentPlayerModel;
    }
}
