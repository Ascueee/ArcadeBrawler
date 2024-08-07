using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class JumpPad : NetworkBehaviour
{
    [Header("Jump Pad Vars")]
    [SerializeField] float jumpForce;


    //COLLISION METHODS

    private void OnTriggerEnter(Collider other)
    {

    }

}
