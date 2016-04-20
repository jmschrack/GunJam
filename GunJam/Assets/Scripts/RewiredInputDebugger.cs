using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Rewired;

public class RewiredInputDebugger : MonoBehaviour {
    private Player player;
    private Text text;
	// Use this for initialization
	void Awake () {
	    player=ReInput.players.GetPlayer(0);
        text=GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
	    text.text="Trigger:"+player.GetButton("FireGun")+"\nSlide:"+player.GetButton("PullSlide")+"\nMagazine:"+player.GetButton("HitMagazine")+"\nA:"+player.GetButton("Teleport")+"\nB:"+player.GetButton("ChangeWeapon")+"\nC:"+player.GetButton("C")+"\nD:"+player.GetButton("Recenter")+"\nThumbX:"+player.GetAxis("DuckSide")+"\nThumbY:"+player.GetAxis("DuckDown");
	}
}
