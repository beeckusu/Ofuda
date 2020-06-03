using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : Movement
{

    public static Bounds playerBounds;
    public delegate void ChangeDirection(DIRECTION direction);
    public event ChangeDirection onChangeDirection;

    protected override void Start()
    {
        base.Start();
        onChangeDirection += SetDirection;
    }

    public override void Move()
    {
        CHARSTATE charState = character.GetState();


        if (Input.anyKey)
        {
            Vector3 moveVector = Vector3.zero;
            if (Input.GetKey(KeyCode.W))
            {
                moveVector += Vector3.up;
            }
            if (Input.GetKey(KeyCode.A))
            {
                moveVector += Vector3.left;
            }
            if (Input.GetKey(KeyCode.S))
            {
                moveVector += Vector3.down;
            }
            if (Input.GetKey(KeyCode.D))
            {
                moveVector += Vector3.right;
            }

            if (moveVector != Vector3.zero)
            {
                if (charState == CHARSTATE.IDLE)
                {
                    character.SetState(CHARSTATE.WALK);
                }
                moveVector = moveVector.normalized * speed * Time.deltaTime;
                if (Input.GetKey(KeyCode.LeftShift))
                    moveVector *= 2;
                transform.Translate(moveVector);
                if (playerBounds != null && !playerBounds.Contains(transform.position))
                {
                    transform.position = playerBounds.ClosestPoint(transform.position);
                }
            }

        }
        else
        {
            if (charState != CHARSTATE.IDLE)
                character.SetState(CHARSTATE.IDLE);
        }
        CheckDirection();

    }


    public override void CheckDirection()
    {

        //Get Character direction
        float x = Input.mousePosition.x - Screen.width / 2;
        float y = Input.mousePosition.y - Screen.height / 2;
        DIRECTION checkDirection;

        if (Mathf.Abs(x) > Mathf.Abs(y))
        {
            if (x > 0)
                checkDirection = DIRECTION.RIGHT;
            else checkDirection = DIRECTION.LEFT;
        }
        else
        {
            if (y > 0)
                checkDirection = DIRECTION.UP;
            else checkDirection = DIRECTION.DOWN;
        }
        if (checkDirection != direction)
        {

            onChangeDirection(checkDirection);

        }



    }

}
