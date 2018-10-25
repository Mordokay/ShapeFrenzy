using UnityEngine;
using System.Collections;

public class Chain : MonoBehaviour {

    ChainManager chainManager;
    GameManager gm;
    void Start () {
        chainManager = GameObject.Find("GameManager").GetComponent<ChainManager>();
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "redBall" || coll.gameObject.tag == "Borders" || coll.gameObject.tag == "blueBall" || 
            coll.gameObject.tag == "yellowBall" || coll.gameObject.tag == "orangeBall" || coll.gameObject.tag == "orangeWall" || 
            coll.gameObject.tag == "pinkBall" || coll.gameObject.tag == "blackBall")
        {
            if (coll.gameObject.tag == "Borders")
            {
                gm.chainBallMiss += 1;
            }
            else {
                gm.chainBallHits += 1;
            }
            chainManager.breakChain = true;
            chainManager.touchedChain = this.gameObject;
            
        }
    }
}
