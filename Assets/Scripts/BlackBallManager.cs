using UnityEngine;
using System.Collections;

public class BlackBallManager : MonoBehaviour {

    float magnitude = 100.0f;

    float maximumSpeed = 5.0f;
    Rigidbody2D rb;

    public int blackBallStage;
    GameManager gm;

    GameObject bigGem;
    GameObject gem;
    GameObject sparkle;

    public Vector3 lastVelocity;
    public bool velocityLock;

    //other Variables
    float brakeSpeed;
    Vector3 normalisedVelocity;
    Vector3 brakeVelocity;

    float ballLifespan;

    Color originalColor;
    float shiftPhase;
    float maxShiftWaitTime;
    float shiftTime;
    bool isShifting;
    bool isShiftingDown;

    public GameObject itemToSpawn;

    void Start()
    {
        isShifting = false;
        isShiftingDown = true;
        shiftPhase = 1.0f;
        maxShiftWaitTime = 2.0f;
        shiftTime = 0;
        originalColor = this.GetComponent<SpriteRenderer>().color;

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

    public void incrementStage()
    {
        blackBallStage += 1;
    }

    public void breakBall()
    {
        GameObject combo = Instantiate(Resources.Load("Combo"), this.transform.position, Quaternion.identity) as GameObject;
        gm.comboBarValue += (Mathf.Clamp(100 - gm.comboValue, 1, 100) / 1000.0f);
        if (blackBallStage < 3)
        {
            switch (blackBallStage)
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
                case 2:
                    combo.GetComponent<Combo>().textInfo = "+" + (40 * gm.comboValue);
                    gm.numberOfPoints += 40 * gm.comboValue;
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
            blackBallStage += 1;

            GameObject newBlackBall = Instantiate(Resources.Load("BlackBall", typeof(GameObject)), this.GetComponent<Transform>().position + new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), 0.0f), Quaternion.identity) as GameObject;
            this.isShifting = false;
            this.isShiftingDown = true;
            this.shiftPhase = 1.0f;
            this.maxShiftWaitTime = 2.0f;
            this.shiftTime = 0;

            gm.AllBalls.Add(newBlackBall);

            newBlackBall.GetComponent<BlackBallManager>().blackBallStage = blackBallStage;
            if (blackBallStage == 1)
            {
                newBlackBall.transform.localScale = new Vector3(newBlackBall.transform.localScale.x * 0.8f, newBlackBall.transform.localScale.y * 0.8f, 1);
                newBlackBall.GetComponent<Rigidbody2D>().velocity = this.GetComponent<Rigidbody2D>().velocity;
                newBlackBall.GetComponent<BlackBallManager>().lastVelocity = this.GetComponent<Rigidbody2D>().velocity;
                this.lastVelocity = this.GetComponent<Rigidbody2D>().velocity;
                newBlackBall.GetComponent<BlackBallManager>().velocityLock = this.velocityLock;
            }
            else if (blackBallStage == 2)
            {
                newBlackBall.transform.localScale = new Vector3(newBlackBall.transform.localScale.x * 0.8f * 0.8f, newBlackBall.transform.localScale.y * 0.8f * 0.8f, 1);
                newBlackBall.GetComponent<Rigidbody2D>().velocity = this.GetComponent<Rigidbody2D>().velocity;
                newBlackBall.GetComponent<BlackBallManager>().lastVelocity = this.GetComponent<Rigidbody2D>().velocity;
                this.lastVelocity = this.GetComponent<Rigidbody2D>().velocity;
                newBlackBall.GetComponent<BlackBallManager>().velocityLock = this.velocityLock;
            }
            else if (blackBallStage == 3)
            {
                newBlackBall.transform.localScale = new Vector3(newBlackBall.transform.localScale.x * 0.8f * 0.8f * 0.8f, newBlackBall.transform.localScale.y * 0.8f * 0.8f * 0.8f, 1);
                newBlackBall.GetComponent<Rigidbody2D>().velocity = this.GetComponent<Rigidbody2D>().velocity;
                newBlackBall.GetComponent<BlackBallManager>().lastVelocity = this.GetComponent<Rigidbody2D>().velocity;
                this.lastVelocity = this.GetComponent<Rigidbody2D>().velocity;
                newBlackBall.GetComponent<BlackBallManager>().velocityLock = this.velocityLock;
            }
        }
        else if (blackBallStage == 3)
        {
            gm.numberOfPoints += 50 * gm.comboValue;
            combo.GetComponent<Combo>().textInfo = "+" + (50 * gm.comboValue);
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
        else if (coll.gameObject.tag == "Player" && coll.gameObject.GetComponent<PlayerMovement>().superStarMode)
        {
            if (ballLifespan > 0.2f)
                breakBall();
        }
    }

    void FixedUpdate()
    {
        if (isShifting)
        {
            if (isShiftingDown)
            {
                shiftPhase -= (Time.deltaTime * 0.5f);
                if (shiftPhase < 0.05f)
                {
                    shiftPhase = 0.05f;
                    isShifting = false;
                    isShiftingDown = false;
                }
            }
            else
            {
                shiftPhase += (Time.deltaTime * 0.5f);
                if (shiftPhase > 1.0f)
                {
                    shiftPhase = 1.0f;
                    isShifting = false;
                    isShiftingDown = true;
                }
            }
        }
        else if (shiftTime > maxShiftWaitTime)
        {
            isShifting = true;
            shiftTime = 0.0f;
        }
        else {
            shiftTime += Time.deltaTime;
        }

        Color originalColorAux = new Color(originalColor.r, originalColor.g, originalColor.b, originalColor.a * shiftPhase);

        this.GetComponent<SpriteRenderer>().color = originalColorAux;
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
