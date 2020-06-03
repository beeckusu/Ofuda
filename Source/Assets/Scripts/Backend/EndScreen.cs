/* Class: EndScreen
 * Author: Gavin Lu
 * 
 * This class handles changing the End Screen text.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndScreen : MonoBehaviour
{
    public Text endText;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
        endText.text = "Victory!\nThe exorcists have reclaimed land on the Overworld!";
    }


    //Update end screen text to Victory statement.
    public void WinGame()
    {
        gameObject.SetActive(true);
        endText.text = "Victory!\nThe exorcists have reclaimed land on the Overworld!";
    }

    //Update end screen text to Defeat statement.
    public void LoseGame()
    {
        gameObject.SetActive(true);
        endText.text = "The Ghouls have reached the civilians. Humanity has fallen.";
    }
}
