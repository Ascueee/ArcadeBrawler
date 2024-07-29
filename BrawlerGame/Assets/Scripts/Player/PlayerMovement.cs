using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerMovement : NetworkBehaviour
{
    [Header("Player Movement Vars")]
    [SerializeField] float speed;
    [SerializeField] float maxSpeed;


    [Header("Player Movement Componenets")]
    [SerializeField] Transform playerObj;
    [SerializeField] Transform playerModel;
    [SerializeField] Rigidbody rb;
    [SerializeField] Animator playerAnim;

    [Header("Jump Collision Componets")]
    [SerializeField] LayerMask ground;

    float horizontal;
    float vertical;
    float rotation;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!IsOwner) return;
        PlayerInput();
        RotatePlayer();

    }

    private void FixedUpdate()
    {
        if (!IsOwner) return;
        MovePlayerServerRpc(horizontal, vertical);
        PlayerRotationServerRpc(rotation);


    }

    //gets player input
    void PlayerInput()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");


    }

    //rotates player depending on input
    void RotatePlayer()
    {
        if (horizontal == 1)
        {
            playerModel.rotation = Quaternion.Euler(0, 90, 0);
            rotation = 90;
        }
        else if (horizontal == -1)
        {
            playerModel.rotation = Quaternion.Euler(0, -90, 0);
            rotation = -90;
        }

        if (vertical == 1)
        {
            playerModel.rotation = Quaternion.Euler(0, 0, 0);
            rotation = 0;
        }
        else if (vertical == -1)
        {
            playerModel.rotation = Quaternion.Euler(0, 180, 0);
            rotation = 180;
        }
    }

    //tells the server to move the player
    [ServerRpc]
    void MovePlayerServerRpc(float horizontal, float vertical)
    {
        if (Mathf.Abs(rb.velocity.magnitude) >= maxSpeed)
        {
            return;
        }

        var moveDirection = playerObj.forward * vertical + playerObj.right * horizontal;
        rb.AddForce(moveDirection * speed, ForceMode.Force);
        playerAnim.SetBool("inMovement", true);

        //checks if the players has stopped moving to set animations to false
        if (Mathf.Abs(rb.velocity.magnitude) < 0.5f)
        {
            playerAnim.SetBool("inMovement", false);
        }
    }

    //tells server what direction the player is facing
    [ServerRpc]
    void PlayerRotationServerRpc(float y)
    {
        playerModel.rotation = Quaternion.Euler(0f, y, 0f);
    }

}
