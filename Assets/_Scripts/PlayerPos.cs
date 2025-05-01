using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPos : MonoBehaviour
{
    void Update()
    {
        Vector3 playerPos = transform.position;

        // Get terrain height at player’s X, Z position
        float terrainHeight = Terrain.activeTerrain.SampleHeight(playerPos);

        // Adjust player Y position to match the terrain height
        playerPos.y = terrainHeight;

        // Update player position
        transform.position = playerPos;
    }

}
