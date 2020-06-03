/* Class: UI Controller
 * Author: Gavin Lu
 * 
 * This class controls the HUD of the game and updates the information on it accordingly.
 */


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{

    //References used to subscribe to relevant events
    public Player player;               //Reference used to subscribe to Secondary changes
    public StructureBuilder builder;    //Reference used to subscribe to Structure changes


    //Top left Game state display
    public Text baseLivesText;
    public Text waveText;
    public Text enemyCountText;

    //Bottom left Player display
    public GameObject[] ofudaPositions;
    public GameObject ofudaSelectionIndicator;
    private OFUDAELEMENT selectedElement;
    private bool isBuilding;
    public Text ofudaName;
    public Text ofudaDescription;
    public Text ofudaCountText;
    public Text wallCountText;

    //Display the pops up when Player activates Ofuda
    public ActiveOfudaDisplayController activeOfudaDisplay;

    //Escape Panel holds the buttons that display when the escape key is pressed
    public GameObject escapePanel;

    //Display text associated with hotkey buttons displayed that changes based on what secondary is equipped
    public Text leftClickHelpText;
    public Text rightClickHelpText;
    public Text spaceBarText;


    //UI subscribes to several events to track when relevant information changes
    private void Awake()
    {
        player.onBuildMode += ChangeActiveMode;                 //Check if player changed between attacking or building
        //Game state events
        GameController.onWaveChange += UpdateWaveText;          //Check if wave increments
        GameController.onGhoulCount += UpdateGhoulText;         //Check if Ghoul Count changes (up or down)
        Base.onDamageTaken += UpdateBaseText;                   //Check if base takes damage
        isBuilding = true;

        //Ofuda events
        OfudaController.onChangeOfuda += ChangeOfudaDisplay;            //Check if player changes Ofuda selected
        OfudaController.onChangeOfudaCount += UpdateOfudaCount;  //Check if Ofuda count changes (up or down)
        OfudaController.onOfudaUse += activeOfudaDisplay.Initialize;    //Check if Ofuda is used and activate active ofuda display
        builder.onUpdateWallCount += UpdateWallCount;

        //Upon game state, active ofuda display should be disabled since the player does not start with an active ofuda
        activeOfudaDisplay.gameObject.SetActive(false);
        escapePanel.SetActive(false);
    }

    //OnDestroy unsubscribes all listeners from their respective events
    private void OnDestroy()
    {
        player.onBuildMode -= ChangeActiveMode;
        builder.onUpdateWallCount -= UpdateWallCount;

        //Game state events
        GameController.onWaveChange -= UpdateWaveText;
        GameController.onGhoulCount -= UpdateGhoulText;
        Base.onDamageTaken -= UpdateBaseText;

        //Ofuda events
        OfudaController.onChangeOfuda -= ChangeOfudaDisplay;
        OfudaController.onChangeOfudaCount -= UpdateOfudaCount;
        OfudaController.onOfudaUse -= activeOfudaDisplay.Initialize;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            escapePanel.SetActive(!escapePanel.activeInHierarchy);
        }
    }


    //*************************************************//
    //***************Update Text Methods***************//
    //*************************************************//

    //Update how many lives are left in the base
    void UpdateBaseText(int health)
    {
        baseLivesText.text = "Civilians Alive: " + health;
    }

    //Update what wave the game is on
    void UpdateWaveText(int wave, int totalWaves)
    {
        if (wave < totalWaves)
        {
            waveText.text = "Wave " + wave + "/" + totalWaves;
        }
        else
        {
            waveText.text = "FINAL WAVE";
        }
    }

    //Update how many ghouls are left
    void UpdateGhoulText(int count)
    {
        if (enemyCountText == null)
        {
            return;
        }
        if (count > 0)
        {
            enemyCountText.text = "Ghoul Left: " + count;
        }
        else
        {
            enemyCountText.text = "Wave cleared!";
        }
    }

    //Update what Ofuda has been selected
    private void ChangeOfudaDisplay(OFUDAELEMENT element)
    {
        selectedElement = element;
        if (element == OFUDAELEMENT.Fire)
        {
            ofudaSelectionIndicator.transform.position = ofudaPositions[0].transform.position;
        }
        else if (element == OFUDAELEMENT.Ice)
        {
            ofudaSelectionIndicator.transform.position = ofudaPositions[1].transform.position;
        }
        else if (element == OFUDAELEMENT.Spark)
        {
            ofudaSelectionIndicator.transform.position = ofudaPositions[2].transform.position;
        }
        UpdateOfudaAbility();
    }

    private void UpdateOfudaAbility()
    {
        if (isBuilding)
        {
            UpdateStructureText();
        }
        else
        {
            UpdateOfudaText();
        }

    }

    private void UpdateStructureText()
    {
        if (selectedElement == OFUDAELEMENT.Fire)
        {
            ofudaName.text = "Grand Pyre";
            ofudaDescription.text = "Arrows that pass through Grand Pyre become fire bolts.";
        }
        else if (selectedElement == OFUDAELEMENT.Ice)
        {
            ofudaName.text = "Glacier";
            ofudaDescription.text = "Creates an area of frost that slows enemies that step on it.";
        }
        else if (selectedElement == OFUDAELEMENT.Spark)
        {
            ofudaName.text = "Lightning Rod";
            ofudaDescription.text = "Creates a lightning rod that discharges, shocking targets that make them take more successive damage.";
        }
    }
    private void UpdateOfudaText()
    {
        if (selectedElement == OFUDAELEMENT.Fire)
        {
            ofudaName.text = "Fire Bolts";
            ofudaDescription.text = "Enchant the bow to shoot large fire bolts that deal massive damage.";
        }
        else if (selectedElement == OFUDAELEMENT.Ice)
        {
            ofudaName.text = "Ice Wave";
            ofudaDescription.text = "Arrows splinter to spread and spread a wave of frost, slowing enemies.";
        }
        else if (selectedElement == OFUDAELEMENT.Spark)
        {
            ofudaName.text = "Chain Lightning";
            ofudaDescription.text = "Arrows bounce off from hit targets to other targets.";
        }
    }

    //Update how many Ofudas are left
    private void UpdateOfudaCount(int count)
    {
        ofudaCountText.text = count + "remaining";
    }
    private void UpdateWallCount(int count)
    {
        wallCountText.text = count + "remaining";
    }

    //*************************************************//
    //******************Other Methods******************//
    //*************************************************//

    //Update which secondary has been selected
    private void ChangeActiveMode(bool isBuilding)
    {
        this.isBuilding = isBuilding;
        if (isBuilding)
        {
            leftClickHelpText.text = "Build Wall";
            rightClickHelpText.text = "Build Tower";
            spaceBarText.text = "Switch to Weapon";
        }
        else
        {
            leftClickHelpText.text = "Shoot Arrow";
            rightClickHelpText.text = "Activate Ability";
            spaceBarText.text = "Switch to Build";
        }
        UpdateOfudaAbility();
    }




}
