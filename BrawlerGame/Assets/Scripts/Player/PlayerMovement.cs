using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerMovement : NetworkBehaviour
{
    [Header("Player Movement Vars")]
    [SerializeField] float moveSpeed;
    [SerializeField] float maxSpeed;
    float horizontalInput;
    float verticalInput;
    Vector3 moveDir;
    bool inAnim;

    [Header("Player Movement Componenets")]
    [SerializeField] Transform orientation;
    [SerializeField] Transform camPos;
    [SerializeField] GameObject camObj;
    [SerializeField] Animator playerAnim;
    GameObject playerCam;
    Rigidbody rb;
    

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        inAnim = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsOwner) return;

        PlayerInput();
        SetAnimations();
    }

    private void FixedUpdate()
    {
        if (!IsOwner) return;
        MovePlayerServerRpc(verticalInput, horizontalInput, inAnim);
    }


    //gets player input
    void PlayerInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
    }

    void SetAnimations()
    {
        //if both input vars != 0 then animate 
        if(horizontalInput != 0 || verticalInput != 0)
        {
            inAnim = true;
        }
        else
        {
            inAnim = false;
        }
    }

    //moves the player on the server side
    [ServerRpc]
    void MovePlayerServerRpc(float vertical, float horizontal, bool inRunAnim)
    {
        //checks if the player is above the maxSpeeds
        if(Mathf.Abs(rb.velocity.magnitude) > maxSpeed)
        {
            return;
        }
        moveDir = orientation.forward * vertical + orientation.right * horizontal;
        rb.AddForce(moveDir * moveSpeed, ForceMode.Force);
        playerAnim.SetBool("inMovement", inRunAnim);
    }
}
