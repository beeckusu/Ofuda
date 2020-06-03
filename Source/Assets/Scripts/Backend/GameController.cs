/* Class: GameController
 * Author: Gavin Lu
 * 
 * This class controls the flow of the game and the wave of ghouls that spawn.
 * The GameController also keeps track of win and loss conditions.
 */


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{

    //Static reference to the singleton GameController
    private static GameController gc;

    //Events that trigger that the UI subscribes to
    public delegate void OnWaveChange(int wave, int totalWaves);    //Event that triggers when wave changes
    public static OnWaveChange onWaveChange;
    public delegate void OnGhoulCount(int count);                   //Event that triggers when ghoul count changes
    public static OnGhoulCount onGhoulCount;

    //Wave Attributes
    public int totalWaves;              //Total number of waves of ghoul attacks
    public float timeBetweenSpawns;     //Time that the game waits before spawning another pulse of Ghouls in a wave
    public int initialSpawnCount;       //Number of Ghouls that spawn in the first wave
    public int increaseSpawnRate;       //Number of Ghouls added to the total number of Ghouls spawned in a wave per wave
    public int spawnCountPerSpawn;      //Number of Ghouls that spawn during a small pulse of Ghouls in a wave
    private static bool gameEnded;
    
    //Current Wave Information
    private int wave;                   //Current wave the player is on
    private int ghoulsLeft;             //Number of ghouls left in the wave

    public GhoulSpawner ghoulSpawner;
    public AudioSource audio;
    public AudioClip waveBegin;
    public AudioClip waveCleared;

    //Misc Attributes
    public Bounds playerBounds;         //Bounds that players can move in
    public EndScreen endScreen;         //End screen that appears after a loss or defeat


    //Awake handles some housekeeping actions, such as:
    //  Enforcing the Singleton design of GameController
    //  Setting the player bounds
    private void Awake()
    {
        //Enforce Singleton design
        if (gc != null)
            Destroy(this);
        gc = this;

        //Set player bounds
        PlayerMovement.playerBounds = playerBounds;
        Blueprint.bounds = playerBounds;

        //Subscribe to event when Base takes damage
        Base.onDamageTaken += DecrementGhoul;
    }


    // Start is the method that starts the game's waves
    void Start()
    {
        wave = 1;
        onWaveChange(wave, totalWaves);
        StartCoroutine(SpawnWave(wave));
        gameEnded = false;
    }


    //SpawnWave handles all the spawning that occurs during a single wave
    //SpawnWave initially spawns half of the total of Ghouls before spawning them in smaller pulses until all Ghouls have been spawned.
    private IEnumerator SpawnWave(int wave)
    {
        audio.clip = waveBegin;
        audio.Play();
        //Variables that hold Ghoul spawning information
        int totalSpawnCount = initialSpawnCount + increaseSpawnRate * (wave - 1);   //Holds the total number of Ghouls that will spawn in the wave
        int spawnCount = totalSpawnCount;                                           //Holds how many Ghouls are left to spawn in the wave
        ghoulsLeft = totalSpawnCount;                                               //Holds how many Ghouls are alive in the wave

        //GhoulsLeft value updates to trigger onGhoulCount event
        onGhoulCount(ghoulsLeft);

        if (wave == totalWaves)
        {
            ghoulSpawner.SpawnPoltergeist();
            spawnCount--;
        }

        //Spawn initial wave of Ghouls; half of the total Ghouls in the wave spawn at the start
        for (; spawnCount > totalSpawnCount / 2; spawnCount--)
        {
            ghoulSpawner.SpawnGhoul(spawnCount);

        }

        //Wait a period of time before starting the pulse spawns
        yield return new WaitForSeconds(timeBetweenSpawns);

        //Spawn the rest of the Ghouls in small groups
        while (spawnCount > 0)
        {
            for (int i = 0; i < spawnCountPerSpawn; i ++)
            {
                if (spawnCount == 0)
                    break;

                if (Random.Range(0, 4) < 2)
                {
                    ghoulSpawner.SpawnGhoul(spawnCount);
                }
                else ghoulSpawner.SpawnRangedGhoul(spawnCount);
                spawnCount--;
            }
            yield return new WaitForSeconds(timeBetweenSpawns);
        }

    }



    //EndWave handles checking if all waves have been cleared or starting the next wave.
    private IEnumerator EndWave()
    {
        audio.clip = waveCleared;
        audio.Play();
        if (gameEnded)
            yield return null;
        else
        {
            wave++;

            //Check if all waves have been completed
            if (wave > totalWaves)
            {
                gameEnded = true;
                //Win the game
                endScreen.WinGame();
                yield return null;
            }
            else
            {
                //Add delay until next wave
                yield return new WaitForSeconds(1);

                //Start the next wave
                StartCoroutine(SpawnWave(wave));
                onWaveChange(wave, totalWaves);
            }
        }
    }

    //GhoulDeath is called when a Ghoul dies. The game object is returned to the object pool
    //and the ghoul count is decremented.
    //
    //Note: Update this to event system
    public static void GhoulDeath(Character ghoul)
    {
        gc.ghoulSpawner.ghoulPool.ReturnGameObject(ghoul.gameObject);
        gc.DecrementGhoul();
    }

    /* DecrementGhoul decrements the ghouls left in the wave. This method keeps track of whether
     * the ghoul decrement is due to a ghoul attacking the base, and potentially losing the game,
     * or if the ghoul decrement was the last ghoul dying to the player and ends the wave.
     * 
     * Input:
     *      int baseLives:  This optional input, if called from Base.onDamageTaken, tracks how much
     *                      health the base has. If called anywhere else, the default value is passed.
     */
    private void DecrementGhoul(int baseLives = 1)
    {
        if (gameEnded)
            return;

        //Decrement Ghoul count
        gc.ghoulsLeft--;
        onGhoulCount(gc.ghoulsLeft);

        //Check if base is dead, in which case lose the game
        if (baseLives == 0)
        {
            gameEnded = true;
            endScreen.LoseGame();
        }
        else
        {
            //Check if there are no more ghouls left in the wave, in which case end the wave
            if (gc.ghoulsLeft == 0)
            {
                gc.StartCoroutine(gc.EndWave());
            }

        }
    }

    //PlayerDeath is called when the player dies, which triggers a loss condition.
    public static void PlayerDeath()
    {
        if (gameEnded)
            return;

        gameEnded = true;
        gc.endScreen.LoseGame();
    }

}
