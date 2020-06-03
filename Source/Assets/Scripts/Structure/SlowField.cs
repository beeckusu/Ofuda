using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowField : MonoBehaviour
{

    public DetectorHitbox detector;

    // Start is called before the first frame update
    void Start()
    {
        detector.onDetected += StartSlow;
        detector.unDetected += EndSlow;
    }

    private void StartSlow(GameObject gameObject)
    {
        Character character = gameObject.GetComponent<Character>();
        character.StartSlow(0.5f, 9999);
    }

    private void EndSlow(GameObject gameObject)
    {
        Character character = gameObject.GetComponent<Character>();
        character.EndSlow();
    }

}
