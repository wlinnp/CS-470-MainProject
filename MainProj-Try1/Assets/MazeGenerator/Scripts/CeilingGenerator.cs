using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/**
 * file: CeilingGenerator.cs
 * author: Wai Phyo
 * class: CS 470 - Game Development
 * Main Project
 * last modified: 05/09/2017
 * 
 * purpose: Generating Prefab Ceiling at the spceific height of the maze. 
 */
public class CeilingGenerator : MonoBehaviour {
    public GameObject Ceiling = null;
    public float CeilingHeight = 5.3f;
    public float CeilingLength = 4f;
    public float CeilingWidth = 4f;
    
    /**
     * Generating Ceiling on start.
     * If Ceiling Height is not set by user, setting to default vaule
     * For Rows and Columns set by MazeSpawner, fill the ceilings. 
     */
    void Start () {
        if (Ceiling != null) {
            if (CeilingHeight == 0) {
                CeilingHeight = 5.3f;
            }
            float x = 0, z = 0;
            for (int row = 0; row < MazeSpawner.Rows; row++) {
                for (int col = 0; col < MazeSpawner.Columns; col++) {
                    GameObject tmp = Instantiate(Ceiling, new Vector3(x, CeilingHeight, z), Quaternion.Euler(0, 0, 0)) as GameObject;
                    tmp.transform.parent = transform;
                    x += CeilingWidth;
                }
                z += CeilingLength;
                x = 0;
            }
        }
    }
}
