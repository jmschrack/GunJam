using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Lazer : Weapon {
    bool isFiring=false;
    bool isOverheated=false;
    public float heatLevel;
    public float normalMax;
    public float optimumMax;
    public float coolDownRate=4;
    public float heatUpRate=5;
    public GameObject beam;
    
    public Image heatMeter;

	
    //this feels like a hack, but it stays true to the same logic everything else uses
    //pull trigger to shoot. In this case, we have a listener to stop shooting on trigger release.
	public override void shoot(){
        isFiring=!isOverheated;
        beam.SetActive(isFiring);
    }
	// Update is called once per frame
	void Update () {
        handleInput();
        isOverheated=isOverheated||heatLevel>100;
        if(isOverheated){
            isFiring=false;
            beam.SetActive(isFiring);
        }
            
        if(isFiring){
            heatLevel+=Time.deltaTime*heatUpRate;
        }else{
            heatLevel-=Time.deltaTime*coolDownRate;
            if(heatLevel<0){
                heatLevel=0;
                isOverheated=false;
            }
                    
        }
	    handleUI();
            
	}
    
    void handleInput(){
        if(PlayerControls.getPlayer().GetButtonUp("FireGun")){
            isFiring=false;
            beam.SetActive(isFiring);
        }
        
    }
    void handleUI(){
        heatMeter.fillAmount=heatLevel/100;
        if(heatLevel<100)
            heatMeter.color=Color.red;
        if(heatLevel<90)
            heatMeter.color=Color.yellow;
        if(heatLevel<75)
            heatMeter.color=Color.green;
    }
}
