/* Class: Game
 * Author: Gavin Lu
 * 
 * This class (poorly named) represents the indicator that previews where a structure can be placed.
 * The Blueprint is only displayed when the player selected the 'Structure' secondary and locks onto
 * a 'grid-like' position on the field. If nothing is in the area, then the structure can be placed.
 * Otherwise, the blueprint will be tinted red and indicates the structure cannot be built.
 */
 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blueprint : MonoBehaviour
{
    public static Bounds bounds;

    public Camera camera;           //Reference to the viewing camera which is needed to compute the world position the mouse is over
    public SpriteRenderer sr;       //SpriteRenderer that holds the preview of what the structure to be placed looks like
    public BoxCollider hitbox;      //Hitbox of the blueprint that checks to see if the position is cleared and can be placed
    private float width;            //Width of the structure
    private float height;           //Height of the structure
    private bool isClear;           //bool that notates whether any collider is within the specified area
    private List<GameObject> overlappedObjects;     //List of objects in the hitbox


    //Initialize the class
    private void Awake()
    {
        overlappedObjects = new List<GameObject>();     //Start a clear list

        //Set dimensions of structures in the game
        width = hitbox.bounds.size.x;                   
        height = hitbox.bounds.size.y;

        //Set the hitbox size a little bit smaller than the structure size to correct for any edges that may overlap
        hitbox.size *= 0.95f;

        AllowBuild();

    }

    // Update is called once per frame
    void Update()
    {
        //Get mouse's world coordinate position
        Vector3 mousePos = camera.ScreenToWorldPoint(Input.mousePosition);
        if (!bounds.Contains(mousePos))
            mousePos = bounds.ClosestPoint(mousePos);

        //Lock the blueprint's position to a grid-like position
        float x = Mathf.RoundToInt(mousePos.x / width) * width;
        float y = Mathf.RoundToInt(mousePos.y / height) * height;
        transform.position = new Vector3(x, y, 0);
    }

    //OnTriggerEnter is called when an object enters the blueprint's hitbox, which disables building.
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Untagged")
            return;

        overlappedObjects.Add(other.gameObject);
        DisableBuild();
    }

    //OnTriggerExit is called when an object exits the blueprint's hitbox. If there are no more overlapping objects, then enable building.
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Untagged" || other.tag == "Wall")
            return;

        overlappedObjects.Remove(other.gameObject);
        if (overlappedObjects.Count == 0)
            AllowBuild();
    }

    //AllowBuild toggles isClear and sets the blueprint's sprite to visually inform players that the structure can be built.
    private void AllowBuild()
    {
        isClear = true;
        sr.color = new Color(255, 255, 255, 0.5f);
    }

    //DisableBuild toggles isClear and sets the blueprint's sprite to visually inform players that the structure cannot be built.
    private void DisableBuild()
    {
        isClear = false;
        sr.color = new Color(255, 0, 0, 0.5f);
    }

    public bool IsClear()
    {
        return isClear;

    }

    private void OnDisable()
    {
        overlappedObjects.Clear();
    }
}
