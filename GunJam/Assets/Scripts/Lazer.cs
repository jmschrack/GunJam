using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Lazer : Weapon {
    public float heatLevel;
    public float normalMax;
    public float optimumMax;
    
    public Image heatMeter;

	// Use this for initialization
	void Start () {
	
	}
	public override void shoot(){
        
    }
	// Update is called once per frame
	void Update () {
	    heatLevel-=Time.deltaTime*4.0f;
        if(heatLevel<0)
            heatLevel=0;
            
	}
    
    void handleUI(){
        heatMeter.fillAmount=heatLevel;
        if(heatLevel<100)
            heatMeter.color=Color.red;
        if(heatLevel<90)
            heatMeter.color=Color.yellow;
        if(heatLevel<75)
            heatMeter.color=Color.green;
    }
}
