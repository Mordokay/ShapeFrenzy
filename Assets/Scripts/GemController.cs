using UnityEngine;
using System.Collections;

public class GemController : MonoBehaviour {

    GameManager gm;
    public int numberPointsGainned;
    float startTime;
    public float timeOfLife;

    void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        startTime = Time.time;
        timeOfLife = 10.0f;
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Player")
        {

            gm.numberOfPoints += numberPointsGainned;
            GameObject combo = Instantiate(Resources.Load("Combo"), this.transform.position, Quaternion.identity) as GameObject;
            combo.GetComponent<Combo>().textInfo = "+" + numberPointsGainned;
            Destroy(this.gameObject);
        }
    }

    void Update() {
        if (Time.time - startTime > timeOfLife) {
            Destroy(this.gameObject);
        }
    }
}
