/* Class: Game
 * Author: Gavin Lu
 * 
 * This helper class toggles a set of colliders on or off. This functionality is often
 * used when a character dies and needs their hitboxes disabled.
 */
 
 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleColliders : MonoBehaviour
{

    public Collider[] colliders;    //List of colliders to toggle


    //Enable all colliders
    public void Enable()
    {
        foreach (Collider c in colliders)
            c.enabled = true;
    }

    //Disable all collisder
    public void Disable()
    {
        foreach (Collider c in colliders)
            c.enabled = false;
    }
}
