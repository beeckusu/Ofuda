/* Class: Game
 * Author: Gavin Lu
 * 
 * This class uses the Object Pool design pattern to help reduce the costs of instantiating new objects.
 * The three functions available are GetGameObjet, ReturnGameObject, and Count.
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{

    public GameObject pooledObject;     //Object to pool and instantiate
    public int initialPoolSize;         //Number of pooledObjects that will be initialized into the pool
    private Stack<GameObject> pool;     //Stack that represents the object pool


    //Awake initializes the Object Pool by adding in an initial buffer of pooled objects
    void Awake()
    {
        //Initialize the Stack
        pool = new Stack<GameObject>();

        //Add in the initial amount of objects
        for (int i = 0; i < initialPoolSize; i++)
        {
            GameObject gameObject = Instantiate(pooledObject);
            gameObject.SetActive(false);
            pool.Push(gameObject);
        }
    }


    /* GetGameObject
     * Function pops a game object from the Stack, enables it, then returns the game object.
     * If the stack is empty, then instantiate a new game object.
     */

    public GameObject GetGameObject()
    {
        if (pool.Count > 0)
        {
            GameObject gameObject = pool.Pop();
            gameObject.SetActive(true);
            return gameObject;
        }
        else
        {
            return Instantiate(pooledObject);
        }
    }

    /* ReturnGameObject
     * Returns the input gameObject to the pool.
     */
    public void ReturnGameObject(GameObject gameObject)
    {
        gameObject.SetActive(false);
        pool.Push(gameObject);
    }

    //Count returns the number of game objects in the pool.
    public int Count()
    {
        return pool.Count;
    }
}
