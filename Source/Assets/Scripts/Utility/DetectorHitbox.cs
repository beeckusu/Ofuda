/* Class: Game
 * Author: Gavin Lu
 * 
 * This generic class simply acts to detect colliders entering and exiting the hitbox
 * and triggers the respective event.
 */
 
 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectorHitbox : MonoBehaviour
{

    public string[] target;               //Target is the tag of characters that the class wants to track, null will track all objects
    private List<Collider> detected;    //List of objects that are in the hitbox

    //Event that notates when characters enter the hitbox
    public delegate void OnDetected(GameObject gameObject);
    public event OnDetected onDetected;
    
    //Event that notates when characters exit the hitbox
    public delegate void UnDetected(GameObject gameObject);
    public event UnDetected unDetected;


    private void Awake()
    {
        //Initialize the list of detected objects
        detected = new List<Collider>();

    }

    private void OnEnable()
    {
        Collider hitbox = gameObject.GetComponent<Collider>();
        foreach (string targetTag in target)
        {
            GameObject[] objects = GameObject.FindGameObjectsWithTag(targetTag);
            foreach (GameObject otherObject in objects)
            {
                Collider otherHitbox = otherObject.GetComponent<Collider>();
                if (hitbox.bounds.Intersects(otherHitbox.bounds))
                {
                    detected.Add(otherHitbox);
                    onDetected?.Invoke(otherObject);
                }
            }
        }
    }

    //OnTriggerEnter is called when a collider enters the gameObject's hitbox.
    //If the object is the correct tag, then detect the object.
    private void OnTriggerEnter(Collider other)
    {
        if (target != null)
        {
            bool isTarget = false;
            for (int i = 0; i < target.Length; i++)
            {
                if (target[i] == other.tag)
                {
                    isTarget = true;
                    break;
                }
            }
            if (!isTarget)
                return;
        }

        detected.Add(other);
        onDetected?.Invoke(other.gameObject);
    }

    //OnTriggerExit is called when a collider exits the gameObject's hitbox.
    //If the object is the correct tag, then undetect the object.
    private void OnTriggerExit(Collider other)
    {
        if (target != null)
        {
            bool isTarget = false;
            for (int i = 0; i < target.Length; i++)
            {
                if (target[i] == other.tag)
                {
                    isTarget = true;
                    break;
                }
            }
            if (!isTarget)
                return;
        }

        detected.Remove(other);
        if (unDetected == null)
            return;

        if (detected.Count == 0)
            unDetected(null);
        else
            unDetected(other.gameObject);
    }

    public Character GetNext()
    {
        if (detected.Count == 0)
            return null;

        Character target = detected[0].GetComponent<Character>();
        detected.RemoveAt(0);
        return target;
    }

}
