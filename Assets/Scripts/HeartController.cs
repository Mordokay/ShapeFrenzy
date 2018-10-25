using UnityEngine;
using System.Collections;

public class HeartController : MonoBehaviour
{
    GameManager gm;
    ProgressionManager pm;

    void Start()
    {
        pm = GameObject.FindGameObjectWithTag("Tube").GetComponent<ProgressionManager>();
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Player")
        {
            pm.myLog += "\nGrabbed a Heart";
            if (gm.numberOfLives < 5)
                gm.numberOfLives += 1;

            Destroy(this.gameObject);
        }
    }
}
