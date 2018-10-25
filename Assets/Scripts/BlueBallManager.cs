using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class BlueBallManager : MonoBehaviour {

    float magnitude = 100.0f;
    public float timeToSplit = 7.0f;

    float startTime;
    float elapsedTime = 0.0f;

    float maximumSpeed = 5.0f;
    Rigidbody2D rb;
    public int numSplits = 2;
    public int blueBallStage;

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

    public GameObject itemToSpawn;
    
    void Start()
    {
        ballLifespan = 0.0f;
        bigGem = Resources.Load("BigGem") as GameObject;
        gem = Resources.Load("Gem") as GameObject;
        sparkle = Resources.Load("Sparkle") as GameObject;
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        
        rb = GetComponent<Rigidbody2D>();
        startTime = Time.time;
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

    public void breakBall() {
        GameObject combo = Instantiate(Resources.Load("Combo"), this.transform.position, Quaternion.identity) as GameObject;
        gm.comboBarValue += (Mathf.Clamp(100 - gm.comboValue, 1, 100) / 1000.0f);
        if (blueBallStage < 2)
        {
            switch (blueBallStage)
            {
                case 0:
                    combo.GetComponent<Combo>().textInfo = "+" + (20 * gm.comboValue);
                    gm.numberOfPoints += 20 * gm.comboValue;
                    if (itemToSpawn != null)
                        Instantiate(itemToSpawn, this.transform.position, Quaternion.identity);
                    break;
                case 1:
                    combo.GetComponent<Combo>().textInfo = "+" + (30 * gm.comboValue);
                    gm.numberOfPoints += 30 * gm.comboValue;
                    break;
            }

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
            blueBallStage += 1;
            startTime = Time.time;

            GameObject newBlueBall = Instantiate(Resources.Load("Blueball", typeof(GameObject)), this.GetComponent<Transform>().position + new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), 0.0f), Quaternion.identity) as GameObject;
            gm.AllBalls.Add(newBlueBall);
            newBlueBall.GetComponent<BlueBallManager>().blueBallStage = blueBallStage;

            if (blueBallStage == 1)
            {
                newBlueBall.transform.localScale = new Vector3(newBlueBall.transform.localScale.x * 0.8f, newBlueBall.transform.localScale.y * 0.8f, 1);
                newBlueBall.GetComponent<Rigidbody2D>().velocity = this.GetComponent<Rigidbody2D>().velocity;
                newBlueBall.GetComponent<BlueBallManager>().lastVelocity = this.GetComponent<Rigidbody2D>().velocity;
                newBlueBall.GetComponent<BlueBallManager>().velocityLock = this.velocityLock;
                this.lastVelocity = this.GetComponent<Rigidbody2D>().velocity;
                numSplits = 1;
                newBlueBall.GetComponent<BlueBallManager>().numSplits = numSplits;
            }
            else if (blueBallStage == 2)
            {
                newBlueBall.transform.localScale = new Vector3(newBlueBall.transform.localScale.x * 0.8f * 0.8f, newBlueBall.transform.localScale.y * 0.8f * 0.8f, 1);
                newBlueBall.GetComponent<Rigidbody2D>().velocity = this.GetComponent<Rigidbody2D>().velocity;
                newBlueBall.GetComponent<BlueBallManager>().velocityLock = this.velocityLock;
                newBlueBall.GetComponent<BlueBallManager>().lastVelocity = this.GetComponent<Rigidbody2D>().velocity;
                this.lastVelocity = this.GetComponent<Rigidbody2D>().velocity;
                numSplits = 0;
                newBlueBall.GetComponent<BlueBallManager>().numSplits = numSplits;
            }
        }
        else if (blueBallStage == 2)
        {
            gm.numberOfPoints += 40 * gm.comboValue;
            combo.GetComponent<Combo>().textInfo = "+" + (40 * gm.comboValue);
            Destroy(this.gameObject);
            gm.AllBalls.Remove(this.gameObject);
        }
    }
    
    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Chain" || coll.gameObject.tag == "BombDetonation" || coll.gameObject.tag == "Bullet")
        {
            if (ballLifespan > 0.2f)
                breakBall();
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
        else if (coll.gameObject.tag == "Player" && coll.gameObject.GetComponent<PlayerMovement>().superStarMode) {
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
            if (velocityLock)
                startTime += Time.deltaTime;
            if (!velocityLock)
                elapsedTime = Time.time - startTime;
            if (elapsedTime > timeToSplit)
            {
                if (numSplits > 0)
                {
                    numSplits -= 1;
                    GameObject blueBall = Instantiate(Resources.Load("Blueball", typeof(GameObject)), this.GetComponent<Transform>().position, Quaternion.identity) as GameObject;
                    blueBall.GetComponent<BlueBallManager>().numSplits = numSplits;
                    blueBall.GetComponent<BlueBallManager>().blueBallStage = blueBallStage;
                    gm.AllBalls.Add(blueBall);
                    if (blueBallStage == 1)
                    {
                        blueBall.transform.localScale = new Vector3(blueBall.transform.localScale.x * 0.8f, blueBall.transform.localScale.y * 0.8f, 1);
                    }
                    else if (blueBallStage == 2)
                    {
                        blueBall.transform.localScale = new Vector3(blueBall.transform.localScale.x * 0.8f * 0.8f, blueBall.transform.localScale.y * 0.8f * 0.8f, 1);
                    }
                    startTime = Time.time;
                }
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
}
