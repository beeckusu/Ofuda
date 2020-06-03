using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StructureHealthBar : MonoBehaviour
{

    public Structure structure;
    public Slider slider;

    // Start is called before the first frame update
    void Awake()
    {
        structure.onDamageTaken += UpdateHealthBar;
        gameObject.SetActive(false);
        slider.maxValue = structure.GetMaxHealth();
        slider.value = slider.maxValue;
    }

    private void UpdateHealthBar(int health)
    {
        if (!gameObject.activeInHierarchy)
        {
            gameObject.SetActive(true);
        }
        slider.value = health;
    }

}
