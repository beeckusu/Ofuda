using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActiveOfudaDisplayController : MonoBehaviour
{

    public Text ofudaNameText;
    public Slider timeSlider;
    public Image sliderBar;


    // Update is called once per frame
    void Update()
    {
        timeSlider.value -= Time.deltaTime;
        if (timeSlider.value == 0)
        {
            gameObject.SetActive(false);
        }
    }

    public void Initialize(OFUDAELEMENT element, float ofudaTime)
    {
        if (element == OFUDAELEMENT.Fire)
        {
            ofudaNameText.text = "Fire Arrows";
            sliderBar.color = new Color(1, 0, 0);
        }
        else if (element == OFUDAELEMENT.Ice)
        {
            ofudaNameText.text = "Ice Wave";
            sliderBar.color = new Color(125f/255, 190f/255, 1);
        }
        else if (element == OFUDAELEMENT.Spark)
        {
            ofudaNameText.text = "Chain Lightning";
            sliderBar.color = new Color(1, 1, 0);
        }

        timeSlider.maxValue = ofudaTime;
        timeSlider.value = ofudaTime;
        if (gameObject != null)
            gameObject.SetActive(true);
    }
}
