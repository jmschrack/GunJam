using UnityEngine;
using System.Collections;

public class PracticeTarget : MonoBehaviour
{

	public bool Shot { get; private set; }

	// Use this for initialization
	void Start()
	{
		Shot = false;
	}

	// Update is called once per frame
	void Update()
	{

	}

	void OnShot(Shot shot)
	{
		GetComponentInParent<Rigidbody>().isKinematic = false;
		GetComponentInParent<Rigidbody>().AddForceAtPosition(shot.Direction * shot.Force, shot.HitPoint);

		Shot = true;
	}
}
