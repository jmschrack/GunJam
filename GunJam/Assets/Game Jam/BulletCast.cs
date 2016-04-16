using UnityEngine;
using System.Collections;

public class BulletCast : MonoBehaviour {

    public bool isAutomatic;
    public GameObject bulletPrefab;
    public float force;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        if (isAutomatic)
        {
            if (Input.GetMouseButton(0))
            {

            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                GameObject bulletClone = Instantiate(bulletPrefab);
                bulletClone.transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x + 90, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
                bulletClone.transform.position = transform.position;
                Rigidbody rbody = bulletClone.GetComponent<Rigidbody>();
                rbody.AddForce(transform.forward * force, ForceMode.Impulse);
            }
        }
	}
}
