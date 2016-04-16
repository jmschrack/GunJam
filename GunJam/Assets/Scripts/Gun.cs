using UnityEngine;
using System.Collections;

public struct Shot
{
	public Vector3 Direction;
	public Vector3 HitPoint;
	public float Force;
}

public class Gun : MonoBehaviour
{
	public GameObject BulletOrigin;

	public AudioSource soundChamberA, soundChamberB, soundShot;

	public bool hasLaserPointer = false;
	LineRenderer laserRenderer;

	// Use this for initialization
	void Start()
	{
		laserRenderer = BulletOrigin.GetComponent<LineRenderer>();
		laserRenderer.enabled = hasLaserPointer;
	}

	// Update is called once per frame
	void Update()
	{
		//Fake a gun shot with left mouse
		if (Input.GetMouseButtonDown(0))
			OnIvrGunTriggerPressed();

		//Laser pointer logic
		if (hasLaserPointer)
		{
			laserRenderer.enabled = true;

			RaycastHit hit;
			Vector3 pos = BulletOrigin.transform.position, fwd = BulletOrigin.transform.forward;
			float hitDist = 1000;
			if (Physics.Raycast(pos, fwd, out hit))
			{
				hitDist = hit.distance;
			}

			laserRenderer.SetPosition(0, pos);
			laserRenderer.SetPosition(1, pos + fwd * hitDist);
			laserRenderer.SetWidth(0.02f, 0.02f * (hit.distance * 0.25f));
		}
		else
		{
			laserRenderer.enabled = false;
		}
	}

	void OnIvrGunTriggerPressed()
	{
		Vector3 pos = BulletOrigin.transform.position, fwd = BulletOrigin.transform.forward;

		RaycastHit hit;
		if (Physics.Raycast(pos, fwd, out hit))
		{
			Shot shot;
			shot.Direction = fwd;
			shot.Force = 200;
			shot.HitPoint = hit.point;
			hit.collider.gameObject.SendMessage("OnShot", shot);

		}

		soundShot.Play();
		BulletOrigin.GetComponent<ParticleSystem>().Play();
		BulletOrigin.GetComponent<Light>().enabled = true;

		StartCoroutine(WaitAndDisableMuzzleFlashLight(BulletOrigin.GetComponent<ParticleSystem>().duration));
	}

	IEnumerator WaitAndDisableMuzzleFlashLight(float time)
	{
		yield return new WaitForSeconds(time);

		BulletOrigin.GetComponent<Light>().enabled = false;
	}

	void OnIvrGunReloadStart()
	{
		soundChamberA.Play();
	}

	void OnIvrGunReloadEnd()
	{
		soundChamberB.Play();
	}
}
