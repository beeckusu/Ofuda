using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OfudaController : MonoBehaviour
{

    public Player player;
    public AudioSource ofudaAudio;

    public delegate void OnChangeOfuda(OFUDAELEMENT element);
    public static OnChangeOfuda onChangeOfuda;
    public delegate void OnChangeOfudaCount(int count);
    public static OnChangeOfudaCount onChangeOfudaCount;
    public delegate void OnOfudaUse(OFUDAELEMENT element, float ofudaTime);
    public static OnOfudaUse onOfudaUse;
    public float ofudaTime;

    private OFUDAELEMENT selectedElement;
    private int ofudaCount;
    private int initialOfudaCount = 3;


    private bool canEnchant;

    private void Awake()
    {
        onChangeOfuda += ChangeOfuda;

    }
    // Start is called before the first frame update
    void Start()
    {
        ofudaCount = initialOfudaCount;
        onChangeOfudaCount?.Invoke(ofudaCount);

        onChangeOfuda(OFUDAELEMENT.Fire);
        canEnchant = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!canEnchant)
            return;

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            onChangeOfuda(OFUDAELEMENT.Fire);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            onChangeOfuda(OFUDAELEMENT.Ice);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            onChangeOfuda(OFUDAELEMENT.Spark);
        }

        if (ofudaCount > 0)
        {
            if (Input.GetMouseButtonDown(1))
            {
                if (player.IsBuilding())
                {
                    player.builder.BuildTower(selectedElement);
                }
                    else
                {
                    StartCoroutine(EnchantWeapon());
                }
                ofudaCount--;
                onChangeOfudaCount(ofudaCount);
            }
        }
    }

    private void ChangeOfuda(OFUDAELEMENT element)
    {
        selectedElement = element;
    }

    private IEnumerator EnchantWeapon()
    {
        ofudaAudio.Play();
        onOfudaUse(selectedElement, ofudaTime);

        canEnchant = false;
        player.attack.SetElement(selectedElement);

        yield return new WaitForSeconds(ofudaTime);
        canEnchant = true;
        player.attack.SetElement(OFUDAELEMENT.None);
    }

    private void OnDestroy()
    {
        onChangeOfudaCount = null;
        onChangeOfuda = null;
    }
}
