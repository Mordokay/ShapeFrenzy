using UnityEngine;
using System.Collections;

public class BulletController : MonoBehaviour {

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "redBall" || coll.gameObject.tag == "Borders" || 
            coll.gameObject.tag == "blueBall" || coll.gameObject.tag == "yellowBall" || 
            coll.gameObject.tag == "Bomb" || coll.gameObject.tag == "orangeBall" || 
            coll.gameObject.tag == "orangeWall" || coll.gameObject.tag == "pinkBall" ||
            coll.gameObject.tag == "blackBall")
        {
            Destroy(this.gameObject);
        }
    }

}
