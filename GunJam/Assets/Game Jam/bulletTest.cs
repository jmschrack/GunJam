using UnityEngine;
using System.Collections;

public class bulletTest : MonoBehaviour {

    public float damage;
    LayerMask layerMask;
	// Use this for initialization
	void Start () {
	 layerMask = 1 << LayerMask.NameToLayer("Hit Box");
	}

    // Update is called once per frame
    //void Update()
    //{
        
    //    Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
    //    if (Physics.Raycast(transform.position, transform.forward, 20, layerMask))
    //    {
    //        Debug.Log("hit!");
    //    }
    //}
}
