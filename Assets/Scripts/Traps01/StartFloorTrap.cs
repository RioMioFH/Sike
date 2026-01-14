using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class StartFloorTrap : MonoBehaviour
{
    // Tilemap with the floor tiles that should disappear when the trap is triggered
    [SerializeField] private Tilemap trapStartFloorTilemap;
    
    // Ensures the trap is only triggered once
    private bool activated = false;

    // Method to activate the start trap
    public void Activate()
    {
        // Stop if the trap has already been triggered
        if (activated) return;
        activated = true;

        // Starts floor opening animation
        StartCoroutine(OpenFloor());
    }

    void Update()
    {   
        // First right-movement input triggers the trap
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            Activate();
        }
    }

    // Coroutine that removes both columns of the floor tiles row by row from top to bottom
    // to create fast opening effect/animation
    private IEnumerator OpenFloor()
    {   
        // Reduce bounds to the area that actually contains tiles to avoid issues with empty cells in the tilemap
        trapStartFloorTilemap.CompressBounds();
        BoundsInt bounds = trapStartFloorTilemap.cellBounds;

        // Identify the two columns of the trap area
        int leftX = bounds.xMin;
        int rightX = bounds.xMax - 1;

        // Iterate from the top row down to the bottom row
        for (int y = bounds.yMax - 1; y >= bounds.yMin; y--)
        {
            var leftCell = new Vector3Int(leftX, y, 0);
            var rightCell = new Vector3Int(rightX, y, 0);

            // Remove the tile in the left column if it exists
            if (trapStartFloorTilemap.GetTile(leftCell) != null)
            {
                trapStartFloorTilemap.SetTile(leftCell, null);
            }
                
            // Remove the tile in the right column if it exists
            if (trapStartFloorTilemap.GetTile(rightCell) != null)
            {
                trapStartFloorTilemap.SetTile(rightCell, null);
            }

            // Short delay to create the visual opening animation   
            yield return new WaitForSeconds(0.02f);
        }
    }
}


