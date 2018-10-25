using UnityEngine;
using System.Collections;

public class bubbleController : MonoBehaviour {

    GameObject myPlayer;
    float shieldTime;
    GameManager gm;
    ProgressionManager pm;

    void Start()
    {
        myPlayer = GameObject.FindGameObjectWithTag("Player");
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        pm = GameObject.FindGameObjectWithTag("Tube").GetComponent<ProgressionManager>();

        shieldTime =10.0f;
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Player")
        {
            myPlayer.GetComponent<PlayerMovement>().isShielded = true;
            myPlayer.GetComponent<PlayerMovement>().shieldTime = shieldTime;

            gm.activateBubble();

            pm.myLog += "\nGrabbed a Bubble Shield\n";

            Destroy(this.gameObject);

        }
    }
}
