/* Class: Game
 * Author: Gavin Lu
 * 
 * This class is attached to a player and handles building structures. When the player selects 'Structures' as a
 * secondary fire, then this class is enabled and shows a blueprint of where a structure can be placed under the mouse.
 * 
 */
 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StructureBuilder : MonoBehaviour
{
    public Blueprint selector;                          //Selector is the hitbox that will be placed under the mouse and indicates whether the space is cleared
    public GameObject wall;                             //Wall that can be built
    public GameObject fireTower;
    public GameObject iceTower;
    public GameObject sparkTower;
    public int wallsLeft;


    //Event that calls when the player equips a new structure to build
    //Note: Rename OnEquip to something else more informative
    public delegate void OnUpdateWallCount(int wallsLeft);
    public event OnUpdateWallCount onUpdateWallCount;


    private void Start()
    {
        onUpdateWallCount?.Invoke(wallsLeft);
    }
    public void Build()
    {
        if (wallsLeft <= 0)
            return;
        if (selector.IsClear())
        {
            GameObject newWall = Instantiate(wall);
            newWall.transform.position = selector.transform.position;
            newWall.GetComponent<SpriteRenderer>().sortingOrder = (int)newWall.transform.position.y * -10;
            wallsLeft--;
            onUpdateWallCount(wallsLeft);
        }
    }
    public void BuildTower(OFUDAELEMENT element)
    {
        GameObject tower = null;
        if (element == OFUDAELEMENT.Fire)
        {
            tower = Instantiate(fireTower);
        }
        else if (element == OFUDAELEMENT.Ice)
        {
            tower = Instantiate(iceTower);
        }
        else if (element == OFUDAELEMENT.Spark)
        {
            tower = Instantiate(sparkTower);
        }
        if (tower == null)
            tower = Instantiate(wall);

        tower.transform.position = selector.transform.position;
        tower.GetComponent<SpriteRenderer>().sortingOrder = (int)tower.transform.position.y * -10;


    }

    private void OnEnable()
    {
        selector.gameObject.SetActive(true);
    }
    private void OnDisable()
    {
        selector.gameObject.SetActive(false);
    }
}
