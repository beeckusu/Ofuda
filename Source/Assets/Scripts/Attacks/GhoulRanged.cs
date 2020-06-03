/* Class: GhoulRanged
 * Author: Gavin Lu
 * 
 * This handles Ghoul ranged attacks
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhoulRanged : Attack
{

    public int projectileSpeed;
    public int projectileLife;
    public ObjectPool projectilePool;

    public void SetObjectPool(ObjectPool pool)
    {
        projectilePool = pool;
    }

    //Execute gets a projectile from the pool, then subscribes a listener that returns the projectile to the object pool
    public override IEnumerator Execute()
    {

        //Add a delay at the beginning to make the projectile coming out of the ghoul to sync with the animation
        yield return new WaitForSeconds(0.25f);

        //Generate a projectile object and initialize the arrow values
        Arrow projectile = projectilePool.GetGameObject().GetComponent<Arrow>();
        Vector3 velocity = target.transform.position - transform.position;
        projectile.Initialize(transform.position, velocity, damage, projectileSpeed, projectileLife);

        //Return the GameObject back to the object pool when the arrow expires
        projectile.onExpired += projectilePool.ReturnGameObject;

        //Put the attack on cooldown
        StartCoroutine(Cooldown());
    }

    public override void SetElement(OFUDAELEMENT element)
    {
        return;
    }

}
