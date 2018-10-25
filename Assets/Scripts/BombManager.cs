using UnityEngine;
using System.Collections;

public class BombManager : MonoBehaviour {

    GameManager gm;
    public int bombPoints;
    public GameObject bombDestructionWave;
    public float waveSize;
    public bool blastStarted;

    Rigidbody2D rb;
    float maximumSpeed;

    float bombBlastSpeed;
    public int pointValue;

    public GameObject itemToSpawn;

    void Start () {
        bombBlastSpeed = 1.6f;
        pointValue = 3;
        blastStarted = false;
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        rb = this.GetComponent<Rigidbody2D>();
        bombPoints = 100;
        waveSize = 1.6f;
        maximumSpeed = 5.0f;
    }

    public void igniteBomb()
    {
        this.GetComponent<SpriteRenderer>().enabled = false;
        this.GetComponent<CircleCollider2D>().enabled = false;
        this.GetComponent<Rigidbody2D>().isKinematic = true;
        GameObject combo = Instantiate(Resources.Load("Combo"), this.transform.position, Quaternion.identity) as GameObject;
        gm.numberOfPoints += bombPoints * gm.comboValue;
        gm.comboBarValue += (Mathf.Clamp(100 - gm.comboValue, 1, 100) / 1000.0f);
        combo.GetComponent<Combo>().textInfo = "+" + (bombPoints * gm.comboValue);
        blastStarted = true;
        bombDestructionWave.SetActive(true);

        if (itemToSpawn != null)
            Instantiate(itemToSpawn, this.transform.position, Quaternion.identity);
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if ((coll.gameObject.tag == "Chain" || coll.gameObject.tag == "Bullet") && !blastStarted)
        {
            igniteBomb();
        }
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "Player" && !coll.gameObject.GetComponent<PlayerMovement>().isImune && !coll.gameObject.GetComponent<PlayerMovement>().isShielded && !coll.gameObject.GetComponent<PlayerMovement>().superStarMode && !blastStarted)
        {
            gm.comboValue = 1;
            gm.comboBarValue = 0.0f;
            gm.barController.barText = "Combo level " + gm.comboValue;
            gm.numberOfLives -= 1;
            coll.gameObject.GetComponent<PlayerMovement>().isImune = true;
            igniteBomb();
        }
        else if (coll.gameObject.tag == "Player" && coll.gameObject.GetComponent<PlayerMovement>().superStarMode && !blastStarted)
        {
            igniteBomb();
        }
    }
    void Update() {

        if (blastStarted) {
            if (bombDestructionWave.transform.localScale.x < waveSize)
            {
                bombDestructionWave.transform.localScale = new Vector3(bombDestructionWave.transform.localScale.x + Time.deltaTime * bombBlastSpeed,
                    bombDestructionWave.transform.localScale.y + Time.deltaTime * bombBlastSpeed, bombDestructionWave.transform.localScale.z);
            }
            else {
                Destroy(this.gameObject);
                gm.AllBalls.Remove(this.gameObject);
            }
        }

        if (!gm.GetComponent<GameManager>().isPaused)
        {
            float brakeSpeed = Vector3.Magnitude(rb.velocity) - maximumSpeed;
            Vector3 normalisedVelocity = rb.velocity.normalized;
            Vector3 brakeVelocity = normalisedVelocity * brakeSpeed;

            rb.AddForce(-brakeVelocity);
        }
    }
}
