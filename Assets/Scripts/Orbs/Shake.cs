using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shake : MonoBehaviour
{
    public float maxChange;
    public float minChange;
    float change;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        change=Random.Range(minChange, maxChange);
        transform.localScale = new Vector3(change,change,change);
    }
}
