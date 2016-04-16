using UnityEngine;
using System.Collections;

public class HitTest : MonoBehaviour {

    public float dmgModifier;
    public int damage;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnCollisionEnter(Collision collision)
    {
        Actor actor = GetComponentInParent<Actor>();
        actor.TakeDamage(damage * dmgModifier);
        Destroy(collision.collider.gameObject);
    }
}
