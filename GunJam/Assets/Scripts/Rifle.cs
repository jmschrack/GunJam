using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Rifle : Weapon {
    public int currentAmmo;
    public int magazineSize;
    public ParticleSystem bullet;
    public GameObject muzzleFlash;
    //penis joke
    private bool needsCocking; 
    public Text guage;
	// Use this for initialization
	void Start () {
	    currentAmmo=magazineSize;
        needsCocking=false;
	}
	
	// Update is called once per frame
	void Update () {
	    handleInput();
        updateUI();
	}
    
    void updateUI(){
        string ammo = (needsCocking?"! ":"  ")+currentAmmo+"/"+magazineSize;
        guage.text=ammo;
    }
    
    void handleInput(){
        if(PlayerControls.getPlayer().GetButtonDown("PullSlide")){
            needsCocking=false;
            //play cocking
        }
        if(PlayerControls.getPlayer().GetButtonDown("HitMagazine")){
            if(currentAmmo<1)
                needsCocking=true;
            currentAmmo=magazineSize;
            //play reload
        }
    }
    
    public override void shoot(){
        Debug.Log("Shooting");
        if(currentAmmo>0&&!needsCocking){
            Debug.Log("Firing");
            currentAmmo--;
            muzzleFlash.SetActive(true);
            bullet.Emit(1);
            StartCoroutine(turnOffMuzzleFlash());
            //play gun shot
        }else{
            Debug.Log("Empty");
            //play click
        }
    }
    
    
    IEnumerator turnOffMuzzleFlash(){
        yield return 1;
        muzzleFlash.SetActive(false);
    }
    
    public int getCurrentAmmo(){
        return currentAmmo;
    }
    
    public int getMagazineSize(){
        return magazineSize;
    }
}
