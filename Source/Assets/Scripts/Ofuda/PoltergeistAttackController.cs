using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoltergeistAttackController : MonoBehaviour
{

    public Character character;
    public Attack melee;
    public Attack ranged;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(CycleAttacks());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator CycleAttacks()
    {

        do
        {
            yield return new WaitForSeconds(2);
            if (!ranged.OnCooldown())
            {
                character.attack = ranged;
            }
            else
            {
                character.attack = melee;
            }
        } while (character.IsAlive());
    }
}
