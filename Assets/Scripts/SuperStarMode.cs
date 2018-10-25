using UnityEngine;
using System.Collections;

public class SuperStarMode : MonoBehaviour {

    GameObject myPlayer;
    GameManager gm;
    float superStarTime;
    ProgressionManager pm;

    void Start()
    {
        pm = GameObject.FindGameObjectWithTag("Tube").GetComponent<ProgressionManager>();
        myPlayer = GameObject.FindGameObjectWithTag("Player");
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        superStarTime = 10.0f;
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Player")
        {
            pm.myLog += "\nGrabbed a SuperStar";

            myPlayer.GetComponent<PlayerMovement>().superStarMode = true;
            myPlayer.GetComponent<PlayerMovement>().superStarTime = superStarTime;

            gm.activateSuperStar();
            Destroy(this.gameObject);
        }
    }
}
