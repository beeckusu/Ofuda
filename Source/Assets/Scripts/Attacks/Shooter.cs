/* Class: Shooter
 * Author: Gavin Lu
 * 
 * Shooter is the Attack subtype that shoots projectiles from the player. The two methods
 * present are Execute, which shoots an arrow at the mouse's position, and SetElement, which
 * changes the element of the arrows.
 * 
 * Currently, only normal arrows and Fire Arrows are implemented.
 */
 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : Attack
{

    private ObjectPool currentArrowPool;    //This holds a reference to the currently selected arrow type
    public ObjectPool normalArrowPool;      //Object pool for when shooter is shooting normal arrows
    public ObjectPool fireArrowPool;        //Object pool for when shooter is shooting fire arrows
    public ObjectPool iceArrowPool;
    public ObjectPool sparkArrowPool;



    void Start()
    {
        //Initialize selected arrow type to normal
        currentArrowPool = normalArrowPool;
    }


    //Execute is called when the player shoots an arrow.
    //First, increase the FPS of the animation since the original animation is slow.
    //Then, set a delay so the arrow is timed with the animation.
    //Finally, instantiate the arrow and return FPS to normal.
    public override IEnumerator Execute()
    {
        //Increase animation speed
        float fps = character.sc.GetFPS();
        float increaseFPSRate = 2;
        character.sc.SetFPS(fps * increaseFPSRate);

        //Set character animation to Shoot
        character.Attack();

        //Set delay to time arrow release with the animation
        yield return new WaitForSeconds(0.18f * fps / (15 * increaseFPSRate));

        //Get Arrow object from ObjectPool, then initialize the arrow with a target direction and speed.
        Arrow arrow = currentArrowPool.GetGameObject().GetComponent<Arrow>();
        arrow.onExpired += currentArrowPool.ReturnGameObject;
        float x = Input.mousePosition.x - Screen.width / 2;
        float y = Input.mousePosition.y - Screen.height / 2;
        arrow.Initialize(transform.position, new Vector3(x, y, 0));

        //Return animation speed to normal
        character.sc.SetFPS(fps);

    }

    //SetElement changes what type of arrows are fired based on the element.
    public override void SetElement(OFUDAELEMENT element)
    {
        if (element == OFUDAELEMENT.None)
            currentArrowPool = normalArrowPool;
        else if (element == OFUDAELEMENT.Fire)
            currentArrowPool = fireArrowPool;
        else if (element == OFUDAELEMENT.Ice)
            currentArrowPool = iceArrowPool;
        else if (element == OFUDAELEMENT.Spark)
            currentArrowPool = sparkArrowPool;
    }
}
