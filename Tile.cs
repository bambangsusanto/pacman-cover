using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    /*
    // 21 column by 19 row
    // Depict the map from bottom to up, left to right
    // How to read: The first row element of the matrix is for the bottom row in Unity map,
    //              The second row element of the matrix is for the second bottom row in Unity map, etc
    //              The horizontal order of the column element in each row of the matrix is the same in Unity map, left to right
    */
    public bool[,] map = new bool[21, 19]
    {
        { false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false},
        { false, true , true , true , true , true , true , true , true , true , true , true , true , true , true , true , true , true , false},
        { false, true , false, false, false, false, false, false, true , false, true , false, false, false, false, false, false, true , false},
        { false, true , true , true , true , false, true , true , true , false, true , true , true , false, true , true , true , true , false},
        { false, false, true , false, true , false, true , false, false, false, false, false, true , false, true , false, true , false, false},
        { false, true , true , false, true , true , true , true , true , true , true , true , true , true , true , false, true , true , false},
        { false, true , false, false, true , false, false, false, true , false, true , false, false, false, true , false, false, true , false},
        { false, true , true , true , true , true , true , true , true , false, true , true , true , true , true , true , true , true , false},
        { false, false, false, false, true , false, true , false, false, false, false, false, true , false, true , false, false, false, false},
        { false, false, false, false, true , false, true , true , true , true , true , true , true , false, true , false, false, false, false},
        { false, false, false, false, true , false, true , false, false, false, false, false, true , false, true , false, false, false, false},
        { true , true , true , true , true , true , true , false, false, false, false, false, true , true , true , true , true , true , true },
        { false, false, false, false, true , false, true , false, false, false, false, false, true , false, true , false, false, false, false},
        { false, false, false, false, true , false, true , true , true , true , true , true , true , false, true , false, false, false, false},
        { false, false, false, false, true , false, false, false, true , false, true , false, false, false, true , false, false, false, false},
        { false, true , true , true , true , false, true , true , true , false, true , true , true , false, true , true , true , true , false},
        { false, true , false, false, true , false, true , false, false, false, false, false, true , false, true , false, false, true , false},
        { false, true , true , true , true , true , true , true , true , true , true , true , true , true , true , true , true , true , false},
        { false, true , false, false, true , false, false, false, true , false, true , false, false, false, true , false, false, true , false},
        { false, true , true , true , true , true , true , true , true , false, true , true , true , true , true , true , true , true , false},
        { false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false}
    };

    /* Map from Unity coordinate to tile coordinate in x-axis */
    public int MapToTileX(Vector3 position)
    {
        return Mathf.RoundToInt(position.x + 0.5f);
    }

    /* Map from Unity coordinate to tile coordinate in y-axis */
    public int MapToTileY(Vector3 position)
    {
        return Mathf.RoundToInt(position.z + 4.5f);
    }


}
