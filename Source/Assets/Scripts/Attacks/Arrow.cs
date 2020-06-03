/* Class: Arrow
 * Author: Gavin Lu
 * 
 * An arrow is fired from a Shooter attached to a player and hits all
 * enemies that collide with the hitbox.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{

    //Event that triggers when the Arrow's lifespan expires
    public delegate void OnExpired(GameObject go);      
    public event OnExpired onExpired;


    public string[] targets;                            //List of targets the arrow can damage
    public int damage;                                  //Damage that arrow will do to enemies
    private float deltaTime;                            //Time that has past since being fired
    public float timeToLive;                            //Time that arrow can last before expiring
    protected Vector3 velocity;                         //Direction that arrow will travel
    public float speed;                                 //Speed that arrow will travel


    //Clear subscribed listeners when arrow disables
    private void OnDisable()
    {
        onExpired = null;
    }


    //FixedUpdate was used to keep the physics calculation of travel consistent. This method
    //increases time and expires the arrow when the arrow has lasted long enough.
    void FixedUpdate()
    {

        //Move the arrow
        transform.position += velocity * Time.fixedDeltaTime * speed;

        //Increase time and check if the arrow expires
        deltaTime += Time.fixedDeltaTime;
        if (deltaTime >= timeToLive)
        {
            onExpired(gameObject);
        }

    }

    //OnTriggerEnter triggesr when the arrow collider enters another collider.
    //The method damages enemies it collides with.
    private void OnTriggerEnter(Collider other)
    {

        foreach (string target in targets)
        {
            if (target == other.tag)
            {
                //Target character takes damage
                other.GetComponent<Character>().TakeDamage(damage);
                return;
            }
        }

    }

    //Initialize is called when the arrow is first enabled and initializes many variables.
    //Input:
    //  Vector3 position:   Initial position of arrow
    //  Vector3 velocity:   Direction arrow travels
    //  float speed         Travel speed of arrow
    public void Initialize(Vector3 position, Vector3 velocity)
    {
        transform.position = position;
        this.velocity = velocity.normalized;
        deltaTime = 0;

        //Rotate arrow to face right direction
        transform.rotation = Quaternion.FromToRotation(Vector3.right, velocity);
    }

    public void Initialize(Vector3 position, Vector3 velocity, int damage, float speed, float timeToLive)
    {
        Initialize(position, velocity);
        this.damage = damage;
        this.speed = speed;
        this.timeToLive = timeToLive;
    }

    //Function used by other classes to clear the arrow
    public void Expire()
    {
        onExpired(gameObject);
    }


    //Getters and Setters//
    public Vector3 GetPosition()
    {
        return transform.position;
    }
    public Vector3 GetDirection()
    {
        return velocity;
    }
    public int GetDamage()
    {
        return damage;
    }
    public float GetSpeed()
    {
        return speed;
    }
    public float GetTimeToLive()
    {
        return timeToLive;
    }

}
