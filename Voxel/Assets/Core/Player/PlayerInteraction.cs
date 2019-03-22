using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voxel.Blocks;

public class PlayerInteraction : MonoBehaviour
{
    public Camera playerCamera;
    private static float interactionDistance = 5;

    private void Start()
    {
        if(playerCamera == null)
        {
            playerCamera = Camera.main;
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, interactionDistance))
            {
                EditTerrain.SetBlock(hit, new Block_Air());
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            RaycastHit hit;
            if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, interactionDistance))
            {
                EditTerrain.SetBlock(hit, new Block_Stone(), true);
            }
        }
    }
}
