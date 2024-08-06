using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class MoveCam : NetworkBehaviour
{
    [Header("Move Camera Vars")]
    [SerializeField] Transform playerPos;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (!IsOwner) return;
        MovePlayerCamServerRpc();
    }

    [ServerRpc]
    void MovePlayerCamServerRpc()
    {
        transform.position = playerPos.position;
    }

    public void SetPlayerPos(Transform pos)
    {
        playerPos = pos;
    }
}
