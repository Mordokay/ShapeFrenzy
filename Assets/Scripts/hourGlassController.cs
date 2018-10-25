using UnityEngine;
using System.Collections;

public class hourGlassController : MonoBehaviour {

    GameManager gm;
    float hourGlassTime;
    ProgressionManager pm;

    void Start()
    {
        pm = GameObject.FindGameObjectWithTag("Tube").GetComponent<ProgressionManager>();
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        hourGlassTime = 5.0f;
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Player")
        {
            pm.myLog += "\nGrabbed a Hour Glass";
            gm.hourGlassActivated = true;
            gm.hourGlassTime = hourGlassTime;
            Destroy(this.gameObject);
        }
    }
}
