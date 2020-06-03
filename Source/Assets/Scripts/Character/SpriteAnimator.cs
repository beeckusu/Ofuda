/* Class: SpriteAnimator
 * Author: Gavin Lu
 * 
 * This class handles changing the sprite in a sprite renderer to animation a sequence of sprites.
 */
 
 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteAnimator : MonoBehaviour
{

    public SpriteRenderer sr;       //SpriteRenderer whose sprite will be updated to create animation

    //Animation attributes
    public float fps;
    private int frame;              //Current frame of the animation
    private float deltaTime;        //Time passed since the last frame change
    private float frameTime;        //Time to pass until frame changes
    private int startFrame;         //Starting index from the animationSprites array
    private int animationLength;    //Length of animation in number of sprites
    public bool loop;              //Determines whether the animation should be looped

    //Sprite arrays
    public Sprite[] defaultSprites;     //Allows for developer to manually set an animation sprite in Editor instead of by script
    private Sprite[] animationSprites;  //Array of sprites that the class will take animation sprites from

    //Event that calls when an animation finishes a loop
    public delegate void OnAnimationComplete(SpriteAnimator sa);
    public OnAnimationComplete onAnimationComplete;


    //Awake initializes values to the SpriteAnimator
    private void Awake()
    {
        if (fps != 0)
            SetFPS(fps);
        frame = 0;
        startFrame = 0;
        deltaTime = 0;
        if (defaultSprites != null)
            animationSprites = defaultSprites;
            animationLength = defaultSprites.Length;
    }


    // Update is called once per frame
    void Update()
    {
        //Filter instances where there are no animation sprites
        if (animationSprites == null)
            return;

        //Filter instances where there is only a single frame of animation
        if (animationLength < 2)
            return;

        //Filter instances where the animation has reached the last frame. This condition only occurs when an animation does not loop
        if (animationLength == frame)
            return;

        //Increase time passed and check if enough time has passed for a new frame
        deltaTime += Time.deltaTime;
        if (deltaTime >= frameTime)
        {
            deltaTime -= frameTime;
            frame++;

            //Check if last frame has been reached
            if (frame == animationLength)
            {
                //If the animation loops, then set frame to 0 and the animation repeats
                if (loop)
                    frame = 0;
                else
                {
                    //Otherwise, trigger the onAnimationComplete event and continue to stay on the last frame
                    onAnimationComplete(this);
                    return;
                }

            }
            //Set new Frame
            sr.sprite = animationSprites[startFrame + frame];
            sr.sortingOrder = (int)(transform.position.y * -10);

        }
    }

    /* SetSprites sets a new set of animation sprites, thus changing the animation that the class plays
     * Input:
     *  Sprite[] sprites:       Animation sprites to play
     *  int startFrame:         Index of where to start the animation from in sprites
     *  int animationLength:    Length of animation in sprites
     *  bool loop:              Notates whether the animation should be looped
     */
    public void SetSprites(Sprite[] sprites, int startFrame, int animationLength, bool loop = true)
    {
        animationSprites = sprites;
        this.startFrame = startFrame;
        this.animationLength = animationLength;
        this.loop = loop;
        frame = 0;
    }

    public Sprite[] GetDefaultSprites()
    {
        return defaultSprites;
    }

    public void SetFPS(float fps)
    {
        frameTime = 1 / fps;
    }
    public float GetFPS()
    {
        return 1 / frameTime;
    }


    public void ResetFrame()
    {
        frame = 0;
        deltaTime = 0;
        sr.sprite = animationSprites[startFrame];
    }

}
