using UnityEngine;
using System.Collections;
using System;

public class WallPieceController : MonoBehaviour {

    public float timeOfLife;
    public float timeSinceSpawn;
    GameManager gm;

    void Start(){
        timeSinceSpawn = 0.0f;
        timeOfLife = 12.0f;

        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    void FixedUpdate() {
        timeSinceSpawn += Time.deltaTime;
        if (timeOfLife < timeSinceSpawn) {
            Destroy(this.gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Chain" || coll.gameObject.tag == "BombDetonation" || coll.gameObject.tag == "Bullet")
        {
            Destroy(this.gameObject);
            GameObject combo = Instantiate(Resources.Load("Combo"), this.transform.position, Quaternion.identity) as GameObject;
            gm.comboBarValue += (Mathf.Clamp(100 - gm.comboValue, 1, 100) / 1000.0f);
            combo.GetComponent<Combo>().textInfo = "+" + (15 * gm.comboValue);
            gm.numberOfPoints += 15 * gm.comboValue;
        }
    }

    internal void breakWall()
    {
        Destroy(this.gameObject);
        GameObject combo = Instantiate(Resources.Load("Combo"), this.transform.position, Quaternion.identity) as GameObject;
        gm.comboBarValue += (Mathf.Clamp(100 - gm.comboValue, 1, 100) / 1000.0f);
        combo.GetComponent<Combo>().textInfo = "+" + (15 * gm.comboValue);
        gm.numberOfPoints += 15 * gm.comboValue;
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "Player" && coll.gameObject.GetComponent<PlayerMovement>().superStarMode || coll.gameObject.tag == "orangeWall")
        {
            Destroy(this.gameObject);
        }
    }
}
