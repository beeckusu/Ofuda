/* Class: Base
 * Author: Gavin Lu
 * 
 * This class represents the civilian base that the player tries to defend. Ghouls
 * will path towards the base and, if they reach the base, decrement the civilian count.
 * Players lose the game when the civilian count reaches 0.
 */
 
 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Base : MonoBehaviour
{

    //Singleton Base reference
    public static Base civilianBase;        

    //Event that gets called when the base takes damage
    public delegate void OnDamageTaken(int lives);
    public static event OnDamageTaken onDamageTaken;

    private int lives;          //Counts how many lives the base has
    public Text livesText;      //Text that overlays the base and shows how many lives are left


    // Start is called before the first frame update
    void Awake()
    {
        //Enforce the Singleton design pattern
        if (civilianBase != null)
            Destroy(this);
        else civilianBase = this;

        //Initialize base and update events listening to the number of base lives
        lives = 10;
        onDamageTaken += ChangeLivesText;
        onDamageTaken(lives);
    }

    //Unsubscribe from events
    private void OnDestroy()
    {
        onDamageTaken -= ChangeLivesText;
    }

    //OnTriggerEnter triggers when a collider enters the base. If the collider is a Ghoul, then take damage.
    public void OnTriggerEnter(Collider other)
    {

        //Filter out objects that aren't Ghouls
        if (other.tag != "Ghoul")
            return;
        
        //Check if the Ghoul didn't flukishly reach the base while dead
        if (other.GetComponent<Character>().IsAlive())
        {
            GhoulAttack(other.gameObject);
        }
    }

    /* GhoulAttack is called when a Ghoul reachs the civilian base.
     * 
     * Input:
     *  GameObject ghoul:   Ghoul that attacked the base
     * 
     * Decrement lives and call onDamageTaken event that updates listeners.
     */
    
    public void GhoulAttack(GameObject ghoul)
    {
        ghoul.SetActive(false);

        if (ghoul.name == "Poltergeist")
            lives = 0;
        else
        {
            lives--;
        }
        onDamageTaken(lives);
    }

    //ChangeLivesText updates the text shown on top of the base
    private void ChangeLivesText(int i)
    {
        livesText.text = lives + "";

    }
}
