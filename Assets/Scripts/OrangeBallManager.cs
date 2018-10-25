using UnityEngine;
using System.Collections;

public class OrangeBallManager : MonoBehaviour {
    float magnitude = 100.0f;
    public float timeToSplit = 7.0f;

    float maximumSpeed = 5.0f;
    Rigidbody2D rb;
    public int orangeBallStage;

    GameManager gm;

    GameObject bigGem;
    GameObject gem;
    GameObject sparkle;

    public Vector3 lastVelocity;
    public bool velocityLock;

    //Other variables
    float brakeSpeed;
    Vector3 normalisedVelocity;
    Vector3 brakeVelocity;

    float ballLifespan;

    ProgressionManager pm;

    public GameObject itemToSpawn;

    void Start()
    {
        pm = GameObject.FindGameObjectWithTag("Tube").GetComponent<ProgressionManager>();
        ballLifespan = 0.0f;
        bigGem = Resources.Load("BigGem") as GameObject;
        gem = Resources.Load("Gem") as GameObject;
        sparkle = Resources.Load("Sparkle") as GameObject;
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();

        rb = GetComponent<Rigidbody2D>();

        if (this.GetComponent<Rigidbody2D>().velocity == Vector2.zero)
        {
            int signal1 = 1;
            int signal2 = 1;
            if (Random.value > 0.5) signal1 = -1;
            if (Random.value > 0.5) signal2 = -1;

            this.GetComponent<Rigidbody2D>().velocity =
                new Vector3(Random.value * magnitude * signal1, Random.value * magnitude * signal2, 0.0f);

            this.GetComponent<Rigidbody2D>().velocity = Vector3.ClampMagnitude(this.GetComponent<Rigidbody2D>().velocity, maximumSpeed);
        }
    }

    public void breakBall()
    {
        GameObject combo = Instantiate(Resources.Load("Combo"), this.transform.position, Quaternion.identity) as GameObject;
        gm.comboBarValue += (Mathf.Clamp(100 - gm.comboValue, 1, 100) / 1000.0f);
        if (orangeBallStage < 1)
        {
            combo.GetComponent<Combo>().textInfo = "+" + (20 * gm.comboValue);
            gm.numberOfPoints += 20 * gm.comboValue;

            if (itemToSpawn != null)
                Instantiate(itemToSpawn, this.transform.position, Quaternion.identity);

            int random = Random.Range(0, 10);
            if (random < 5)
            {
                if (Random.Range(0, 8) == 1)
                    Instantiate(sparkle, this.transform.position, Quaternion.identity);
            }
            else if (random < 8)
            {
                if (Random.Range(0, 8) == 1)
                    Instantiate(gem, this.transform.position, Quaternion.identity);
            }
            else
            {
                if (Random.Range(0, 8) == 1)
                    Instantiate(bigGem, this.transform.position, Quaternion.identity);
            }

            this.transform.localScale = new Vector3(this.transform.localScale.x * 0.8f, this.transform.localScale.y * 0.8f, 1);
            orangeBallStage += 1;

            GameObject newOrangeBall = Instantiate(Resources.Load("OrangeBall", typeof(GameObject)), this.GetComponent<Transform>().position + new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), 0.0f), Quaternion.identity) as GameObject;
            gm.AllBalls.Add(newOrangeBall);

            newOrangeBall.GetComponent<OrangeBallManager>().orangeBallStage = orangeBallStage;
            if (orangeBallStage == 1)
            {
                newOrangeBall.transform.localScale = new Vector3(newOrangeBall.transform.localScale.x * 0.8f, newOrangeBall.transform.localScale.y * 0.8f, 1);
                newOrangeBall.GetComponent<Rigidbody2D>().velocity = this.GetComponent<Rigidbody2D>().velocity;
                newOrangeBall.GetComponent<OrangeBallManager>().lastVelocity = this.GetComponent<Rigidbody2D>().velocity;
                this.lastVelocity = this.GetComponent<Rigidbody2D>().velocity;
                newOrangeBall.GetComponent<OrangeBallManager>().velocityLock = this.velocityLock;
            }
        }
        else if (orangeBallStage == 1)
        {
            gm.numberOfPoints += 40 * gm.comboValue;
            combo.GetComponent<Combo>().textInfo = "+" + (40 * gm.comboValue);
            Destroy(this.gameObject);
            gm.AllBalls.Remove(this.gameObject);

            GameObject orangeWall = Instantiate(Resources.Load("OrangeWallMaker")) as GameObject;
            orangeWall.transform.position = this.transform.position;
            orangeWall.GetComponent<OrangeWallController>().mapScaleWalls = pm.scaleMag;
        }
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Chain" || coll.gameObject.tag == "BombDetonation" || coll.gameObject.tag == "Bullet")
        {
            if (ballLifespan > 0.2f) {
                breakBall();
            }
        }
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "Player" && !coll.gameObject.GetComponent<PlayerMovement>().isImune && !coll.gameObject.GetComponent<PlayerMovement>().isShielded && !coll.gameObject.GetComponent<PlayerMovement>().superStarMode)
        {
            gm.comboValue = 1;
            gm.comboBarValue = 0.0f;
            gm.barController.barText = "Combo level " + gm.comboValue;
            gm.numberOfLives -= 1;
            coll.gameObject.GetComponent<PlayerMovement>().isImune = true;
        }
        else if (coll.gameObject.tag == "Player" && coll.gameObject.GetComponent<PlayerMovement>().superStarMode)
        {
            if (ballLifespan > 0.2f)
                breakBall();
        }
    }

    void FixedUpdate()
    {
        ballLifespan += Time.deltaTime;
        if (velocityLock)
        {
            rb.velocity = Vector3.zero;
        }
        if (!gm.isPaused)
        {

            brakeSpeed = Vector3.Magnitude(rb.velocity) - maximumSpeed;
            normalisedVelocity = rb.velocity.normalized;
            brakeVelocity = normalisedVelocity * brakeSpeed;

            rb.AddForce(-brakeVelocity);
        }
    }
}
