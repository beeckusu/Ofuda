using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SparkArrow : Arrow
{
    public DetectorHitbox detector;


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Ghoul")
            return;

        //Ghoul character takes damage
        Character character = other.GetComponent<Character>();
        character.TakeDamage(damage);
        Character nextChar = detector.GetNext();
        while (nextChar != null && (nextChar == character || !nextChar.IsAlive()))
        {
            nextChar = detector.GetNext();
        }

        if (nextChar == null)
            return;

        velocity = (nextChar.transform.position - transform.position).normalized;
        transform.rotation = Quaternion.FromToRotation(Vector3.right, velocity);

    }
}
