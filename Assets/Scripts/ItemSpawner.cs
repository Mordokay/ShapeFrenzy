using UnityEngine;
using System.Collections;

public class ItemSpawner : MonoBehaviour {

    GameManager gm;

    //This variable stores the last time(in points) an item was dropped
    int spawnPointbased;
    public int frequencyDrop;
    GameObject Heart;
    GameObject Shield;
    GameObject HourGlass;
    GameObject Star;
    GameObject TripleChain;
    GameObject FlameThrower;
    GameObject Shotgun;
    float heartMinRange;
    float heartMaxRange;
    float shieldMinRange;
    float shieldMaxRange;
    float hourGlassMinRange;
    float hourGlassMaxRange;
    float starMinRange;
    float starMaxRange;
    float tripleChainMinRange;
    float tripleChainMaxRange;
    float flameThrowerMinRange;
    float flameThrowerMaxRange;
    float shotgunMinRange;
    float shotgunMaxRange;

    float originalLimitX;
    float originalLimitY;
    public float mapScaleWalls;
    ProgressionManager pm;

    // Use this for initialization
    void Start () {
        originalLimitX = 13.5f;
        originalLimitY = 9.0f;
        pm = GameObject.FindGameObjectWithTag("Tube").GetComponent<ProgressionManager>();

        Heart = Resources.Load("Heart") as GameObject;
        Shield = Resources.Load("ShieldItem") as GameObject;
        HourGlass = Resources.Load("HourGlass") as GameObject;
        Star = Resources.Load("Star") as GameObject;
        TripleChain = Resources.Load("TripleChain") as GameObject;
        FlameThrower = Resources.Load("FlameThrower") as GameObject;
        Shotgun = Resources.Load("Shotgun") as GameObject;

        heartMinRange = 0;
        heartMaxRange = 16;
        shieldMinRange = 17;
        shieldMaxRange = 32;
        hourGlassMinRange = 33;
        hourGlassMaxRange = 48;
        starMinRange = 49;
        starMaxRange = 65;
        tripleChainMinRange = 66;
        tripleChainMaxRange = 81;
        flameThrowerMinRange = 82;
        flameThrowerMaxRange = 100;
        shotgunMinRange = 101;
        shotgunMaxRange = 150;

        spawnPointbased = 0;
        gm = this.GetComponent<GameManager>();
	}
	
	void FixedUpdate() {
        //items not dropped in the last "frequency" points
        if (spawnPointbased + frequencyDrop < gm.numberOfPoints) {
            spawnPointbased += frequencyDrop;
            int random = Random.Range(0, 150);
            if (random >= heartMinRange && random <= heartMaxRange)
                Instantiate(Heart, new Vector3(Random.Range(-(originalLimitX * pm.scaleMag), (originalLimitX * pm.scaleMag)), Random.Range(-(originalLimitY * pm.scaleMag), (originalLimitY * pm.scaleMag)), 0.0f), Quaternion.identity);
            else if (random >= shieldMinRange && random <= shieldMaxRange)
                Instantiate(Shield, new Vector3(Random.Range(-(originalLimitX * pm.scaleMag), (originalLimitX * pm.scaleMag)), Random.Range(-(originalLimitY * pm.scaleMag), (originalLimitY * pm.scaleMag)), 0.0f), Quaternion.identity);
            else if (random >= hourGlassMinRange && random <= hourGlassMaxRange)
                Instantiate(HourGlass, new Vector3(Random.Range(-(originalLimitX * pm.scaleMag), (originalLimitX * pm.scaleMag)), Random.Range(-(originalLimitY * pm.scaleMag), (originalLimitY * pm.scaleMag)), 0.0f), Quaternion.identity);
            else if (random >= starMinRange && random <= starMaxRange)
                Instantiate(Star, new Vector3(Random.Range(-(originalLimitX * pm.scaleMag), (originalLimitX * pm.scaleMag)), Random.Range(-(originalLimitY * pm.scaleMag), (originalLimitY * pm.scaleMag)), 0.0f), Quaternion.identity);
            else if (random >= tripleChainMinRange && random <= tripleChainMaxRange)
                Instantiate(TripleChain, new Vector3(Random.Range(-(originalLimitX * pm.scaleMag), (originalLimitX * pm.scaleMag)), Random.Range(-(originalLimitY * pm.scaleMag), (originalLimitY * pm.scaleMag)), 0.0f), Quaternion.identity);
            else if (random >= flameThrowerMinRange && random <= flameThrowerMaxRange)
                Instantiate(FlameThrower, new Vector3(Random.Range(-(originalLimitX * pm.scaleMag), (originalLimitX * pm.scaleMag)), Random.Range(-(originalLimitY * pm.scaleMag), (originalLimitY * pm.scaleMag)), 0.0f), Quaternion.identity);
            else if (random >= shotgunMinRange && random <= shotgunMaxRange)
                Instantiate(Shotgun, new Vector3(Random.Range(-(originalLimitX * pm.scaleMag), (originalLimitX * pm.scaleMag)), Random.Range(-(originalLimitY * pm.scaleMag), (originalLimitY * pm.scaleMag)), 0.0f), Quaternion.identity);
        }
	}
}
