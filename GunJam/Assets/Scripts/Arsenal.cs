using UnityEngine;
using System.Collections;

public class Arsenal : MonoBehaviour {

    public int currentWeapon=0;
    public Weapon[] weapons = new Weapon[4];
	
    
    public void cycleWeapon(){
        currentWeapon++;
        if(currentWeapon==weapons.Length)
            currentWeapon=0;
    }
    
    public Weapon getCurrentWeapon(){
        return weapons[currentWeapon];
    }
}
