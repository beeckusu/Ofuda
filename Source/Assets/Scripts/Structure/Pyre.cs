using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pyre : Structure
{

    public DetectorHitbox detector;
    public ObjectPool pool;

    private void Awake()
    {
        detector.onDetected += IgniteArrow;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void IgniteArrow(GameObject arrowObject)
    {
        Arrow arrow = arrowObject.GetComponent<Arrow>();
        arrow.Expire();
        GameObject fireArrowObject = pool.GetGameObject();
        Arrow fireArrow = fireArrowObject.GetComponent<Arrow>();
        fireArrow.Initialize(arrow.GetPosition(), arrow.GetDirection());
        fireArrow.onExpired += pool.ReturnGameObject;
    }
 
}
