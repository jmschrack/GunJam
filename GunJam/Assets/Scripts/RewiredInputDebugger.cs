using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Rewired;

public class RewiredInputDebugger : MonoBehaviour {
    private Player player;
    private Text text;
    public IliumRewired ir;
	// Use this for initialization
	void Awake () {
	    player=ReInput.players.GetPlayer(0);
        text=GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
	    text.text="Trigger:"+player.GetButton("FireGun")+"\n Problem:"+ir.problem+"\nErrCd:"+ir.errorCode;
	}
}
