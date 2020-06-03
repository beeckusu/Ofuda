using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhoulMovement : Movement
{

    public Character ghoul;
    private Transform target;
    private Vector3 moveVector;
    private void Start()
    {
        speed = 0.75f;
        moveVector = Vector3.down;
        SetTarget(Base.civilianBase.transform);
    }

    public override void Move()
    {
        if (target == null)
            return;



        if (character.GetState() == CHARSTATE.IDLE)
        {
            character.SetState(CHARSTATE.WALK);
        }
        else if (moveVector == Vector3.zero)
            character.SetState(CHARSTATE.IDLE);

        moveVector = (target.position - transform.position).normalized * Time.deltaTime * speed;
        transform.position += moveVector;


        CheckDirection();
    }


    public override void CheckDirection()
    {
        DIRECTION checkDirection;
        if (Mathf.Abs(moveVector.x) > Mathf.Abs(moveVector.y))
        {
            if (moveVector.x > 0)
                checkDirection = DIRECTION.RIGHT;
            else checkDirection = DIRECTION.LEFT;
        }
        else
        {
            if (moveVector.y > 0)
                checkDirection = DIRECTION.UP;
            else checkDirection = DIRECTION.DOWN;
        }
        if (checkDirection != direction)
            SetDirection(checkDirection);

    }

    public void SetTarget(Transform transform)
    {
        target = transform;

    }

}
