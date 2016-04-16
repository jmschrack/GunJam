using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{

	public Transform target;

	private Animator anim;
	private int health = 100;

	private CharacterController controller;

	public bool IsDead { get { return health <= 0; } }

	// Use this for initialization
	void Start()
	{
		anim = GetComponent<Animator>();
		anim.SetFloat("Speed", 0);

		controller = GetComponent<CharacterController>();

		foreach (Rigidbody rb in gameObject.GetComponentsInChildren<Rigidbody>())
		{
			rb.detectCollisions = true;
		}
	}

	// Update is called once per frame
	void Update()
	{
		if (target == null || !anim.isInitialized)
			return;

		anim.SetInteger("Health", health);

		if (IsDead)
			return;

		Vector3 diff = target.position - transform.position;
		diff.y = 0;

		if (diff.magnitude < 1.5f)
		{
			anim.SetBool("Attacking", true);
			anim.SetFloat("Speed", 0);
		}
		else
		{
			anim.SetBool("Attacking", false);
			anim.SetFloat("Speed", 1);

			//motor.SetVelocity(diff.normalized * Time.deltaTime * 0.5f);
			//motor.SetControllable(true);

			controller.Move(diff.normalized * Time.deltaTime * 0.5f);


			//transform.position += ;
			transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(diff), 0.25f);
		}


	}

	void OnDamaged(int damage)
	{
		health -= damage;

		BroadcastMessage("ClearLastHit");

		if (health <= 0)
		{
			StartCoroutine(DestroyAfterTime(3));
		}
	}

	private IEnumerator DestroyAfterTime(float time)
	{
		yield return new WaitForSeconds(time);

		Destroy(gameObject);
	}
}
