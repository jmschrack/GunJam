﻿using UnityEngine;
using System.Collections;
using RootMotion.FinalIK;

public class HitTest : MonoBehaviour {

    public float dmgModifier;
    public int damage;
    [SerializeField]
    HitReaction hitReaction;
    [SerializeField]
    float hitForce = 1f;

    // Use this for initialization
    void Start () {
        hitReaction = GetComponentInParent<HitReaction>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnCollisionEnter(Collision collision)
    {
        Actor actor = GetComponentInParent<Actor>();
        
        hitReaction.Hit(GetComponent<Collider>(), collision.contacts[0].normal * hitForce, collision.contacts[0].point);
        bulletTest bullet = collision.collider.gameObject.GetComponent<bulletTest>();
        actor.TakeDamage(bullet.damage * dmgModifier);
        Destroy(collision.collider.gameObject);
    }
}
