/* Class: Game
 * Author: Gavin Lu
 * 
 * This utility function loads a scene when executed.
 */
 
 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{

    /* Execute
     * Input:
     *  string scene:   name of scene to load
     *  
     * Output: Game loads the new scene
     * 
     */
    public void Execute(string scene)
    {
        SceneManager.LoadScene(scene);
    }
}
