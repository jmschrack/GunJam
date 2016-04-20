using UnityEngine;
using System.Collections;
using Rewired;

public class Shotgun : Weapon {
    public bool loaded;
    public int tube=8;
    public int currentAmmo;
    public ParticleSystem shot;
	// Use this for initialization
	void Start () {
	    currentAmmo=tube;
	}
	
	// Update is called once per frame
	void Update () {
	    handleInput();
	}
    
    void handleInput(){
        if(PlayerControls.getPlayer().GetButtonDown("PullSlide")&&currentAmmo>0){
            loaded=true;
        }
        if(PlayerControls.getPlayer().GetButtonDown("HitMagazine")&&currentAmmo<tube){
            currentAmmo++;
        }
    }
    
    public override void shoot(){
        if(currentAmmo>0&&loaded){
            shot.Emit(8);
            loaded=false;
            currentAmmo--;
        }
    }
    
    
}
