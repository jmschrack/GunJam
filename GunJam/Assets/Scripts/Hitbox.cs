using UnityEngine;
using System.Collections;

public class Hitbox : MonoBehaviour
{

	public int Damage = 20;

	public bool lastHit = false;
	public Vector3 lastForce = Vector3.zero;

	// Use this for initialization
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

	}

	void OnShot(Shot shot)
	{
		SendMessageUpwards("OnDamaged", Damage);

		lastHit = true;
		lastForce = shot.Direction * shot.Force;
	}

	void ClearLastHit()
	{
		lastHit = false;
		lastForce = Vector3.zero;
	}
}
