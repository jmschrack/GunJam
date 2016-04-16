using UnityEngine;
using System.Collections;

public class Actor : MonoBehaviour {

    public float hitpointMax;
    public float currentHitpoints;


    void Start()
    {
        currentHitpoints = hitpointMax;
    }
    public void TakeDamage(float amount)
    {
        currentHitpoints -= amount;
    }
}
