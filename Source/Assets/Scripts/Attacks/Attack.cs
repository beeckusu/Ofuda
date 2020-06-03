/* Class: Attack
 * Author: Gavin Lu
 * 
 * This class represents the base class that all character attack types are derived from.
 * Each attack has an Execute function, which represents executing the attack, and a
 * SetElement function, which changes the element of the attack.
 */



using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Attack : MonoBehaviour
{

    //Object references
    protected GameObject target;
    public Character character;


    //Attack Attributes
    public int damage;
    public float range;
    public float cooldown;
    protected bool onCooldown;
    protected OFUDAELEMENT element;


    private void Start()
    {
        element = OFUDAELEMENT.None;
    }

    //Inherited Methods
    public abstract IEnumerator Execute();
    public abstract void SetElement(OFUDAELEMENT element);


    //Method that checks if the attack can be executed. Attack must be off cooldown and target must be in range
    public bool CanAttack(GameObject target)
    {
        return !onCooldown && (target.transform.position - transform.position).magnitude <= range;
    }


    //This method puts an attack on cooldown, waits the cooldown duration, then reenables the attack
    protected IEnumerator Cooldown()
    {
        onCooldown = true;
        yield return new WaitForSeconds(cooldown);
        onCooldown = false;
    }


    //Getters and Setters
    public bool OnCooldown()
    {
        return onCooldown;
    }

    public void SetTarget(GameObject gameObject)
    {
        target = gameObject;
    }

}
