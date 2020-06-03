using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    public StructureBuilder builder;
    public OfudaController ofuda;

    public delegate void OnBuildMode(bool isBuilding);
    public OnBuildMode onBuildMode;
    private bool isBuilding;

    protected override void Start()
    {
        base.Start();
        SetBuildingMode(false);
    }

    protected override void Action()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SetBuildingMode(!isBuilding);
        }
        if (Input.GetMouseButton(0))
        {
            if (isBuilding)
            {
                builder.Build();
            }
            else
            {
                charMovement.CheckDirection();
                StartCoroutine(attack.Execute());
                return;
            }
        }
        charMovement.Move();

    }

    private void SetBuildingMode(bool isBuilding)
    {
        this.isBuilding = isBuilding;
        onBuildMode(isBuilding);
        if (isBuilding)
        {
            builder.enabled = true;
        }
        else builder.enabled = false;
    }

    public bool IsBuilding()
    {
        return isBuilding;
    }

}
