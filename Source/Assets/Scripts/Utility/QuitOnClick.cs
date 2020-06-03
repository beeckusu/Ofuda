/* Class: Game
 * Author: Gavin Lu
 * 
 * This utility function that closes the game when executed.
 */
 
 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitOnClick : MonoBehaviour
{
    
    //Execute closes the game.
    public void Execute()
    {

        Application.Quit();
    }
}
