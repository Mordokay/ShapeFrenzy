using UnityEngine;
using System.Collections;

public class PinkBallManager : MonoBehaviour
{

    float magnitude = 100.0f;

    float maximumSpeed = 5.0f;
    Rigidbody2D rb;

    GameManager gm;
    public bool velocityLock;

    Vector3 originalSize;

    GameObject bigGem;
    GameObject gem;
    GameObject sparkle;

    public float sizeOfBall;
    float maxSizeOfBall;
    float minSizeOfBall;

    public Vector3 lastVelocity;

    //other Variables
    float brakeSpeed;
    Vector3 normalisedVelocity;
    Vector3 brakeVelocity;

    float ballLifespan;

    public GameObject itemToSpawn;

    void Start()
    {
        sizeOfBall = 1.0f;
        ballLifespan = 0.0f;
        maxSizeOfBall = 2.0f;
        minSizeOfBall = 0.5f;
        originalSize = this.transform.localScale;
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
        combo.GetComponent<Combo>().textInfo = "+" + (int)((maxSizeOfBall - sizeOfBall) * 10 * gm.comboValue);
        gm.numberOfPoints += (int)((maxSizeOfBall - sizeOfBall) * 10 * gm.comboValue);

        sizeOfBall -= 0.15f;

        if (sizeOfBall > minSizeOfBall)
        {
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

            this.transform.localScale = new Vector3(originalSize.x * sizeOfBall, originalSize.y * sizeOfBall, 1);
        }
        else
        {
            Destroy(this.gameObject);
            gm.AllBalls.Remove(this.gameObject);
            if (itemToSpawn != null)
                Instantiate(itemToSpawn, this.transform.position, Quaternion.identity);
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
        else if (coll.gameObject.tag == "Player" && coll.gameObject.GetComponent<PlayerMovement>().superStarMode)
        {
            if (ballLifespan > 0.2f)
                breakBall();
        }
        else if (coll.gameObject.tag == "blueBall"){
            sizeOfBall += ((3.0f - coll.gameObject.GetComponent<BlueBallManager>().blueBallStage) * (1/ 4.5f));
            gm.AllBalls.Remove(coll.gameObject);
            Destroy(coll.gameObject);
        }
        else if (coll.gameObject.tag == "yellowBall") {
            sizeOfBall += ((4.0f - coll.gameObject.GetComponent<YellowBallManager>().yellowBallStage) * (1 / 6.0f));
            gm.AllBalls.Remove(coll.gameObject);
            Destroy(coll.gameObject);
        }
        else if (coll.gameObject.tag == "redBall")
        {
            sizeOfBall += ((4.0f - coll.gameObject.GetComponent<RedBallManager>().redBallStage) * (1 / 6.0f));
            gm.AllBalls.Remove(coll.gameObject);
            Destroy(coll.gameObject);
        }
        else if (coll.gameObject.tag == "orangeBall")
        {
            sizeOfBall += ((2.0f - coll.gameObject.GetComponent<OrangeBallManager>().orangeBallStage) * (1 / 3.0f));
            gm.AllBalls.Remove(coll.gameObject);
            Destroy(coll.gameObject);
        }

        if (sizeOfBall > maxSizeOfBall) {
            this.transform.localScale = originalSize;
            sizeOfBall = 1.0f;

            GameObject newPinkBall = Instantiate(Resources.Load("PinkBall", typeof(GameObject)), this.GetComponent<Transform>().position + new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), 0.0f), Quaternion.identity) as GameObject;
            gm.AllBalls.Add(newPinkBall);

            newPinkBall.transform.localScale = new Vector3(newPinkBall.transform.localScale.x * 0.8f, newPinkBall.transform.localScale.y * 0.8f, 1);
            newPinkBall.GetComponent<Rigidbody2D>().velocity = this.GetComponent<Rigidbody2D>().velocity;
            newPinkBall.GetComponent<PinkBallManager>().lastVelocity = this.GetComponent<Rigidbody2D>().velocity;
            newPinkBall.GetComponent<PinkBallManager>().velocityLock = this.velocityLock;
            this.lastVelocity = this.GetComponent<Rigidbody2D>().velocity;
        }
    }

    void FixedUpdate()
    {
        //sizeOfBall += Time.deltaTime * 0.08f;
        this.transform.localScale = new Vector3(originalSize.x * sizeOfBall, originalSize.y * sizeOfBall, 1);
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
