using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghoul : Character
{

    private GameObject attackTarget;
    public DetectorHitbox playerDetection;
    public DetectorHitbox attackableRange;


    protected override void Start()
    {
        base.Start();
        playerDetection.onDetected += FollowTarget;
        playerDetection.unDetected += FollowTarget;
        attackableRange.onDetected += SetAttackTarget;
        attackableRange.unDetected += SetAttackTarget;
    }

    protected override void Action()
    {

        if (attackTarget != null && attack.CanAttack(attackTarget))
        {
            attack.SetTarget(attackTarget);
            Attack();
            StartCoroutine(attack.Execute());
            if (attackTarget.tag == "Player")
            {
                Character player = attackTarget.GetComponent<Character>();
                if (!player.IsAlive())
                {
                    attackTarget = null;
                    ((GhoulMovement)charMovement).SetTarget(Base.civilianBase.transform);
                }

            }
            else if (attackTarget.tag == "Structure")
            {
                Structure structure = attackTarget.GetComponent<Structure>();
                if (!structure.IsAlive())
                {
                    attackTarget = null;
                    ((GhoulMovement)charMovement).SetTarget(Base.civilianBase.transform);
                }

            }
        }
        charMovement.Move();
    }

    protected override IEnumerator Die()
    {

        yield return base.Die();
        yield return new WaitForSeconds(3.0f);
        Reset();
        gameObject.SetActive(false);
        GameController.GhoulDeath(this);
        attackTarget = null;   

    }

    private void FollowTarget(GameObject gameObject)
    {
        Transform target = (gameObject !=null) ? gameObject.transform : Base.civilianBase.transform;
        ((GhoulMovement)charMovement).SetTarget(target);
    }

    private void SetAttackTarget(GameObject gameObject)
    {
        if (gameObject != null || attackTarget == null)
            attackTarget = gameObject;

    }
}
