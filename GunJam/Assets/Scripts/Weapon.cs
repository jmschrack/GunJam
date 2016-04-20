using UnityEngine;
using System.Collections;

public abstract class Weapon : MonoBehaviour {

    public GameObject objects;
	public abstract void shoot();
    public void activate(){
        objects.SetActive(true);
    }
    public void deactivate(){
        objects.SetActive(false);
    }
}
