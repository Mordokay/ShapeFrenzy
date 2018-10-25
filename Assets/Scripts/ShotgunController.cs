using UnityEngine;
using System.Collections;

public class ShotgunController : MonoBehaviour {

    GameObject myPlayer;
    GameManager gm;
    int shotgunAmmo;
    ProgressionManager pm;

    void Start()
    {
        pm = GameObject.FindGameObjectWithTag("Tube").GetComponent<ProgressionManager>();
        myPlayer = GameObject.FindGameObjectWithTag("Player");
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        shotgunAmmo = 10;
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Player")
        {
            pm.myLog += "\nGrabbed a Shotgun";
            gm.activateShotgun();
            myPlayer.GetComponent<PlayerMovement>().shotgunActive = true;
            myPlayer.GetComponent<PlayerMovement>().shotgunAmmo = shotgunAmmo;
            
            Destroy(this.gameObject);
        }
    }
}