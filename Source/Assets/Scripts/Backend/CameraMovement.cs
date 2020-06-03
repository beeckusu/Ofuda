/* Class: CameraMovement
 * Author: Gavin Lu
 * 
 * This class handles moving the camera with the player.
 */
 
 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{

    public GameObject player;   //Player that camera is following
    public Camera camera;       //Camera that the script controls
    public float minSize;       //Min size represents how close the zoom can get
    public float maxSize;       //Max size represents how far the zoom can get



    // Update is called once per frame
    void Update()
    {

        //Update camera to be above player position
        transform.position = player.transform.position - Vector3.forward * 10;

        //Check for mouse scrolling and zoom accordingly
        if (Input.mouseScrollDelta.y != 0)
            camera.orthographicSize = Game.Bound(camera.orthographicSize - Input.mouseScrollDelta.y, minSize, maxSize);

    }
}
