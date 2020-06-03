using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class CharacterSpriteController : MonoBehaviour
{

    public Character character;
    public SpriteAnimator sa;
    public Movement m;
    public float fps;

    //Sprite sets
    public Sprite[] idleSprites;
    public Sprite[] walkingSprites;
    public Sprite[] dyingSprites;
    public Sprite[] attackingSprites;
    public Sprite[] castingSprites;

    private Sprite[] currentAnimationSprites;
    private DIRECTION direction;
    private CHARSTATE charState;


    // Start is called before the first frame update
    void Start()
    {
        direction = DIRECTION.DOWN;
        SetState(CHARSTATE.IDLE);
        sa.SetFPS(fps);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private int DirectionToInt(DIRECTION direction)
    {
        if (direction == DIRECTION.UP)
            return 0;
        else if (direction == DIRECTION.LEFT)
            return 1;
        else if (direction == DIRECTION.DOWN)
            return 2;
        else return 3;
    }

    public void SetState(CHARSTATE charState)
    {
        this.charState = charState;
        bool loop = true;
        if (charState == CHARSTATE.IDLE)
        {
            currentAnimationSprites = idleSprites;
        }
        else if (charState == CHARSTATE.WALK)
            currentAnimationSprites = walkingSprites;
        else if (charState == CHARSTATE.ATTACK)
        {
            currentAnimationSprites = attackingSprites;
            loop = false;
        }
        else if (charState == CHARSTATE.DIE)
        {
            currentAnimationSprites = dyingSprites;
            direction = 0;
            loop = false;
        }
        else if (charState == CHARSTATE.CAST)
        {
            currentAnimationSprites = castingSprites;
            loop = false;

        }

        int animationLength = currentAnimationSprites.Length;
        if (charState != CHARSTATE.DIE)
            animationLength /= 4;

        sa.SetSprites(currentAnimationSprites, DirectionToInt(direction) * animationLength, animationLength, loop);
        sa.ResetFrame();

    }


    public void SetDirection(DIRECTION direction)
    {
        if (direction == this.direction)
            return;

        this.direction = direction;
        sa.SetSprites(currentAnimationSprites, DirectionToInt(direction) * currentAnimationSprites.Length / 4, currentAnimationSprites.Length / 4);
        sa.ResetFrame();
    }

    public void SetFPS(float fps)
    {
        sa.SetFPS(fps);
    }
    public float GetFPS()
    {
        return sa.GetFPS();
    }

}
