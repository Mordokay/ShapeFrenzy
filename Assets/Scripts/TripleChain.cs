using UnityEngine;
using System.Collections;

public class TripleChain : MonoBehaviour {

    GameManager gm;
    float tripleChainTime;
    ProgressionManager pm;

    void Start()
    {
        pm = GameObject.FindGameObjectWithTag("Tube").GetComponent<ProgressionManager>();
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        tripleChainTime = 10.0f;
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Player")
        {
            pm.myLog += "\nGrabbed a Tripple Chain";
            gm.isTripleChainning = true;
            gm.tripleChainTime = tripleChainTime;
            gm.activateTriple();
            Destroy(this.gameObject);
        }
    }
}
