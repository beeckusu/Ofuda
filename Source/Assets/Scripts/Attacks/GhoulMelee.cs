/* Class: GhoulMelee
 * Author: Gavin Lu
 * 
 * This MonoBehaviour handles melee attacks from a Ghoul.
 */
 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhoulMelee : Attack
{

    //The Execute method attacks either the player or structure target
    public override IEnumerator Execute()
    {
        yield return new WaitForSeconds(0.25f);
        if (target.tag == "Player")
        {
            target.GetComponent<Character>().TakeDamage(damage);
        }
        else if (target.tag == "Structure")
            target.GetComponent<Structure>().TakeDamage(damage);
    }

    public override void SetElement(OFUDAELEMENT element)
    {
        return;
    }


}
