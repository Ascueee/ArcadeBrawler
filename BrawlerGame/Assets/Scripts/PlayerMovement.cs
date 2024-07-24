using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerMovement : NetworkBehaviour
{
    [Header("Player Movement Componenets")]
    [SerializeField] float speed;
    [SerializeField] float maxSpeed;
    [Header("Player Movement Componenets")]
    [SerializeField] Transform playerObj;
    [SerializeField] Rigidbody rb;
    
    float horizontal;
    float vertical;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!IsOwner) return;
        PlayerInput();

    }

    private void FixedUpdate()
    {
        if (!IsOwner) return;
        MovePlayerServerRpc(horizontal, vertical);

    }

    void PlayerInput()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");



    }

    [ServerRpc]

    void MovePlayerServerRpc(float horizontal, float vertical)
    {
        if(Mathf.Abs(rb.velocity.magnitude) >= maxSpeed)
        {
            return;
        }
        var moveDirection = playerObj.right * horizontal + playerObj.forward * vertical;
        rb.AddForce(moveDirection * speed, ForceMode.Force);

    }
}
