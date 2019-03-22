using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voxel;
using Voxel.Blocks;

public class PlayerInfo : MonoBehaviour
{
    public Vector3 position;
    [HideInInspector]
    public Block standingOn;

    private void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 1f))
        {
            standingOn = EditTerrain.GetBlock(hit);
        }

        position = new Vector3(Mathf.Round(transform.position.x * 100) / 100, Mathf.Round(transform.position.y * 100) / 100, Mathf.Round(transform.position.z * 100) / 100);
    }
}
