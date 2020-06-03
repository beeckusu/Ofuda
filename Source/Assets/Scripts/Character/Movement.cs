using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public Character character;
    public CharacterSpriteController sc;



    protected DIRECTION direction;
    protected float speed;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        speed = 3;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public virtual void Move()
    {

    }

    public virtual void CheckDirection()
    {

    }

    protected void SetDirection(DIRECTION direction)
    {
        this.direction = direction;
        sc.SetDirection(direction);
    }
    public void Reset()
    {
        enabled = true;
        SetDirection(DIRECTION.DOWN);

    }

    public void MultiplySpeed(float factor)
    {
        speed *= factor;
    }
}
