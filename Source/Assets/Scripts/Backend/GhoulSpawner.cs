/* Class: GameController
 * Author: Gavin Lu
 * 
 * This class controls the flow of the game and the wave of ghouls that spawn.
 * The GameController also keeps track of win and loss conditions.
 */


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhoulSpawner : MonoBehaviour
{

    //Ghoul Attributes
    public ObjectPool ghoulPool;        //ObjectPool that holds Melee Ghouls
    public ObjectPool rangedGhoulPool;  //ObjectPool that holds Ranged Ghouls
    public ObjectPool ghoulBallPool;    //ObjectPool that holds the projectiles fired by ranged ghouls
    public ObjectPool poltergeistPool;  //ObjectPool that holds Poltergeists
    public Vector2 ghoulSpawnCenter;    //Center of the circle that Ghouls spawn on the perimeter of
    public float ghoulSpawnRadius;      //Radius of the circle that Ghouls spawn on the perimeter of
    public float ghoulSpawnAngleMin;    //ghoulSpawnAngleMin and Max control which area of the circle's perimeter
    public float ghoulSpawnAngleMax;        //ghouls can spawn from

    //Awake ensures ghoulSpawnAngleMin < ghoulSpawnAngleMax
    private void Awake()
    {
        //Swap ghoulSpawnAngleMin and ghoulSpawnAngleMax values if Max < Min
        if (ghoulSpawnAngleMax < ghoulSpawnAngleMin)
        {
            float f = ghoulSpawnAngleMin;
            ghoulSpawnAngleMin = ghoulSpawnAngleMax;
            ghoulSpawnAngleMax = f;
        }
    }

    /*SpawnGhoul is the method that handles spawning a Ghoul into the game world
     * 
     * A Ghoul GameObject is popped from the ObjectPool and a random point on the GhoulSpawnCircle
     * for the Ghoul to spawn from.
     * 
     * Input:
     *      int number: The n'th ghoul spawned, where n decrements
     */

    public void SpawnGhoul(int number)
    {
        //Get Ghoul GameObject
        GameObject ghoulObject = ghoulPool.GetGameObject();
        ghoulObject.name = "Ghoul " + number;
        SetSpawnPosition(ghoulObject);
    }

    public void SpawnRangedGhoul(int number)
    {
        //Get Ghoul GameObject
        GameObject ghoulObject = rangedGhoulPool.GetGameObject();
        ghoulObject.GetComponent<GhoulRanged>().SetObjectPool(ghoulBallPool);
        ghoulObject.name = "Ghoul " + number;
        SetSpawnPosition(ghoulObject);

    }
    public void SpawnPoltergeist()
    {
        GameObject poltergeist = poltergeistPool.GetGameObject();
        SetSpawnPosition(poltergeist);
    }

    private void SetSpawnPosition(GameObject ghoulObject)
    {
        //Select a random point on the Ghoul Spawn Circle
        float angle = Random.Range(ghoulSpawnAngleMin, ghoulSpawnAngleMax) * Mathf.Deg2Rad;
        float x = Mathf.Cos(angle) * ghoulSpawnRadius + ghoulSpawnCenter.x;
        float y = Mathf.Sin(angle) * ghoulSpawnRadius + ghoulSpawnCenter.y;

        //Set the Ghoul's position
        ghoulObject.transform.position = new Vector3(x, y, 0);
    }

}