using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightningrod : MonoBehaviour
{

    public DetectorHitbox detector;
    private List<Ghoul> targetList;
    public float timeBetweenStrikes;
    public ObjectPool strikePool;
    public int damage;

    private void Awake()
    {
        targetList = new List<Ghoul>();
        detector.onDetected += AddTarget;
        detector.unDetected += RemoveTarget;
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LightningStrike());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator LightningStrike()
    {
        while(true)
        {
            if (targetList.Count == 0)
            {
                yield return null;
            }
            else
            {
                int index = Random.Range(0, targetList.Count);
                Ghoul target = targetList[index];
                //Subscribe return to pool
                target.TakeDamage(damage);
                GameObject strikeObject = strikePool.GetGameObject();
                strikeObject.transform.position = target.transform.position;
                strikeObject.GetComponentInChildren<SpriteAnimator>().onAnimationComplete += ReturnStrikeToPool;
                yield return new WaitForSeconds(timeBetweenStrikes);
            }
        }
    }

    void AddTarget(GameObject gameObject)
    {
        targetList.Add(gameObject.GetComponent<Ghoul>());
    }
    void RemoveTarget(GameObject gameObject)
    {
        targetList.Remove(gameObject.GetComponent<Ghoul>());
    }

    void ReturnStrikeToPool(SpriteAnimator sa)
    {
        sa.ResetFrame();
        strikePool.ReturnGameObject(sa.gameObject);
    }
}
