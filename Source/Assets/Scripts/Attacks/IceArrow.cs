using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceArrow : Arrow
{
    public float slowStrength;
    public float slowDuration;

    private void OnTriggerEnter(Collider other)
    {
        //Filter out non-ghouls
        if (other.tag != "Ghoul")
            return;

        //Ghoul character takes damage
        Character character = other.GetComponent<Character>();
        character.TakeDamage(damage);
        character.StartSlow(slowStrength, slowDuration);
    }

}
