using UnityEngine;
using System.Collections;

public class flameThrowerItem : MonoBehaviour {

    GameManager gm;
    float flameThrowerTime;
    ProgressionManager pm;

    void Start()
    {
        flameThrowerTime = 10.0f;
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        pm = GameObject.FindGameObjectWithTag("Tube").GetComponent<ProgressionManager>();
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Player")
        {
            gm.flameThrowerTime = this.flameThrowerTime;
            gm.flameThrowerActivated = true;

            pm.myLog += "\nGrabbed a Flame Thrower";

            gm.activateFlame();
            Destroy(this.gameObject);
        }
    }
}
