using UnityEngine;
using System.Collections;
using Rewired;

public class PlayerControls : MonoBehaviour {
    private static Player player;
    public Transform teleportLocation;
    private Arsenal arsenal;
	// Use this for initialization
	void Start () {
	    player=ReInput.players.GetPlayer(0);
        arsenal=GetComponent<Arsenal>();
	}
	
	// Update is called once per frame
	void Update () {
	    handleInput();
	}
    
    void handleInput(){
        if(player.GetButtonDown("Teleport")){
            this.transform.position=teleportLocation.position;
        }
        if(player.GetButtonDown("ChangeWeapon")){
            arsenal.cycleWeapon();
        }
        if(player.GetButtonDown("FireGun")){
           // Debug.Log("Shooting");
            arsenal.getCurrentWeapon().shoot();
        }
    }
    
    public static Player getPlayer(){
        return player;
    }
}
