/* Class: PlayerSlash
 * Author: Gavin Lu
 * 
 * PlayerSlash represents the class of Attacks that come from a player's slash.
 * The class keeps track of ghouls in the slash hitbox, then damages them all when
 * Execute is called.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSlash : Attack
{

    public float pushbackStrength;      //This value represents how far back enemies are pushed on slash
    private List<Character> enemies;    //List of enemies that are targetable; that is, they are in the slash hitbox


    private void Awake()
    {
        enemies = new List<Character>();

        //Subscribe to Character Movement's onChangeDirection event, which should change the direction of the slash hitbox
        ((PlayerMovement)character.charMovement).onChangeDirection += ChangeDirection;
    }

    private void OnDestroy()
    {
        ((PlayerMovement)character.charMovement).onChangeDirection -= ChangeDirection;
    }


    //OnTriggerEnter triggers when a collider enters PlayerSlash's hitbox and functions to correctly
    //add enemies to the list of enemies found in the hitbox.
    private void OnTriggerEnter(Collider other)
    {

        //Filter out colliders that aren't Ghouls
        if (other.tag != "Ghoul")
            return;

        //If the Ghoul is alive, then keep track of it
        Character otherChar = other.GetComponent<Character>();
        if (otherChar.IsAlive())
            enemies.Add(otherChar);

    }

    //OnTriggerExit triggesr when a collider exits PlayerSlash's hitbox and functions to correctly
    //remove enemies from the list of enemies found in the hitbox.
    private void OnTriggerExit(Collider other)
    {

        //Filter out colliders that aren't Ghouls
        if (other.tag != "Ghoul")
            return;

        //Remove the Ghoul's Character from the list of targetable enemies
        Character otherChar = other.GetComponent<Character>();
        enemies.Remove(otherChar);

    }

    //Execute is the method that attacks all targetable enemies.
    //A slash damages enemies and pushes them back. Then, the list of targetable enemies
    //is updated to remove all enemies killed in the attack.
    public override IEnumerator Execute()
    {

        //Deal damage to enemies
        character.Attack(enemies);

        //Add slight delay before attack pushes enemies to match the animation
        yield return new WaitForSeconds(0.15f);


        //Check how many enemies died in the attack and pushback all live enemies
        List<Character> deadEnemies = new List<Character>();    //List of enemies that died in the attack
        foreach (Character enemy in enemies)
        {
            //If the enemy is dead, then add to list of dead enemies
            if (!enemy.IsAlive())
                deadEnemies.Add(enemy);
            //Otherwise, push them back
            else
                character.PushBack(enemy, pushbackStrength);
        }

        //Remove all dead enemies from the list of targetable enemies
        foreach (Character dead in deadEnemies)
        {
            enemies.Remove(dead);
        }


    }


    //This method triggers whenever the onChangeDirection event occurs from
    //the corresponding player.
    private void ChangeDirection(DIRECTION direction)
    {
        if (direction == DIRECTION.UP)
        {
            transform.eulerAngles = new Vector3(0, 0, 180);
        }
        else if (direction == DIRECTION.LEFT)
        {
            transform.eulerAngles = new Vector3(0, 0, 270);
        }
        else if (direction == DIRECTION.DOWN)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
        else
        {
            transform.eulerAngles = new Vector3(0, 0, 90);
        }
    }

    //SetElement changes the Slash based on the element.
    //
    //NOT IMPLEMENTED
    public override void SetElement(OFUDAELEMENT element)
    {
        this.element = element;
    }

}
