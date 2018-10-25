using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ChainManager : MonoBehaviour {

    public bool isChainning;
    public bool isChainning2;
    public bool isChainning3;
    public Vector3 chainStartPos;
    public Vector3 chainDirection;
    public GameObject chain;
    public GameObject chainArrow;
    public bool releasedArrow;
    public bool releasedArrow2;
    public bool releasedArrow3;
    public bool breakChain;
    public float speedChain;
    public float chainDelay;

    public bool fingerReleased;

    public List<GameObject> Chains;
    public List<GameObject> Chains2;
    public List<GameObject> Chains3;

    public GameManager gm;
    public float angleShift;
    public GameObject touchedChain;

    //Other variables
    float AngleRad;
    float angle;
    Object myChain;
    GameObject chainAux;
    Object myChain2;
    GameObject chainAux2;
    Object myChain3;
    GameObject chainAux3;
    Vector3 newDirectionWithShift;
    public float elapsedTime;
    public float elapsedTime2;
    public float elapsedTime3;
    float timeToReleaseChain;

    // Use this for initialization
    void Start () {
        elapsedTime = 0.0f;
        elapsedTime2 = 0.0f;
        elapsedTime3 = 0.0f;
        timeToReleaseChain = 0.03f;
        angleShift = 15.0f;
        isChainning = false;
        isChainning2 = false;
        isChainning3 = false;
        releasedArrow = false;
        releasedArrow2 = false;
        releasedArrow3 = false;
        speedChain = 8.0f;
        chainDelay = 0.1f;
        fingerReleased = true;
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    public void removeAllChains() {
        foreach (GameObject chain in Chains)
        {
            Destroy(chain);
        }
        Chains.Clear();
        isChainning = false;
        releasedArrow = false;
        foreach (GameObject chain in Chains2)
        {
            Destroy(chain);
        }
        Chains2.Clear();
        isChainning2 = false;
        releasedArrow2 = false;
        foreach (GameObject chain in Chains3)
        {
            Destroy(chain);
        }
        Chains3.Clear();
        isChainning3 = false;
        releasedArrow3 = false;
    }

    void Update() {

        if (breakChain)
        {
            if (gm.isTripleChainning)
            {
                if (Chains.Contains(touchedChain))
                {
                    foreach (GameObject chain in Chains)
                    {
                        Destroy(chain);
                    }
                    Chains.Clear();
                    isChainning = false;
                    releasedArrow = false;
                    breakChain = false;
                    elapsedTime = 0.0f;
                }
                else if (Chains2.Contains(touchedChain))
                {
                    foreach (GameObject chain in Chains2)
                    {
                        Destroy(chain);
                    }
                    Chains2.Clear();
                    isChainning2 = false;
                    releasedArrow2 = false;
                    breakChain = false;
                    elapsedTime2 = 0.0f;
                }
                else if (Chains3.Contains(touchedChain))
                {
                    foreach (GameObject chain in Chains3)
                    {
                        Destroy(chain);
                    }
                    Chains3.Clear();
                    isChainning3 = false;
                    releasedArrow3 = false;
                    breakChain = false;
                    elapsedTime3 = 0.0f;
                }
            }
            else {
                foreach (GameObject chain in Chains)
                {
                    Destroy(chain);
                }
                Chains.Clear();
                isChainning = false;
                releasedArrow = false;
                breakChain = false;
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate () {

        if (isChainning)
        {
            elapsedTime += Time.deltaTime;
            if (!releasedArrow)
            {
                Object myChainArrow = Instantiate(chainArrow, chainStartPos, Quaternion.identity);
                GameObject chainArrowAux = (GameObject)myChainArrow;
                Chains.Add(chainArrowAux);
                chainArrowAux.GetComponent<Rigidbody2D>().velocity = chainDirection.normalized * speedChain;

                AngleRad = Mathf.Atan2(chainDirection.y, chainDirection.x);
                angle = (180 / Mathf.PI) * AngleRad;
                chainArrowAux.GetComponent<Rigidbody2D>().rotation = angle - 90;

                releasedArrow = true;
            }
            //else if ((Chains[Chains.Count - 1].transform.position - chainStartPos).magnitude > 0.27f)
            else if (elapsedTime > timeToReleaseChain)
            {
                elapsedTime = 0.0f;
                myChain = Instantiate(chain, chainStartPos, Quaternion.identity);
                chainAux = (GameObject)myChain;
                Chains.Add(chainAux);
                chainAux.GetComponent<Rigidbody2D>().velocity = chainDirection.normalized * speedChain;

                AngleRad = Mathf.Atan2(chainDirection.y, chainDirection.x);
                angle = (180 / Mathf.PI) * AngleRad;
                chainAux.GetComponent<Rigidbody2D>().rotation = angle + 90;
            }
        }

        if (gm.isTripleChainning)
        {
            AngleRad = Mathf.Atan2(chainDirection.y, chainDirection.x);
            angle = (180 / Mathf.PI) * AngleRad;

            if (isChainning2)
            {
                elapsedTime2 =+ Time.deltaTime;

                if (!releasedArrow2)
                {
                    Object myChainArrow2 = Instantiate(chainArrow, chainStartPos, Quaternion.identity);
                    GameObject chainArrowAux2 = (GameObject)myChainArrow2;
                    Chains2.Add(chainArrowAux2);

                    newDirectionWithShift = new Vector3((float)Mathf.Cos((angle + angleShift) * (Mathf.PI / 180)), (float)Mathf.Sin((angle + angleShift) * (Mathf.PI / 180)));

                    chainArrowAux2.GetComponent<Rigidbody2D>().velocity = newDirectionWithShift.normalized * speedChain;
                    chainArrowAux2.GetComponent<Rigidbody2D>().rotation = angle + angleShift - 90;

                    releasedArrow2 = true;
                }
                //else if ((Chains2[Chains2.Count - 1].transform.position - chainStartPos).magnitude > 0.27f)
                else if (elapsedTime2 > timeToReleaseChain)
                {
                    elapsedTime2 = 0.0f;
                    myChain2 = Instantiate(chain, chainStartPos, Quaternion.identity);
                    chainAux2 = (GameObject)myChain2;
                    Chains2.Add(chainAux2);

                    newDirectionWithShift = new Vector3((float)Mathf.Cos((angle + angleShift) * (Mathf.PI / 180)), (float)Mathf.Sin((angle + angleShift) * (Mathf.PI / 180)));

                    chainAux2.GetComponent<Rigidbody2D>().velocity = newDirectionWithShift.normalized * speedChain;
                    chainAux2.GetComponent<Rigidbody2D>().rotation = angle + angleShift - 90;

                    releasedArrow2 = true;
                }
            }

            if (isChainning3)
            {
                elapsedTime3 =+ Time.deltaTime;

                if (!releasedArrow3)
                {
                    Object myChainArrow3 = Instantiate(chainArrow, chainStartPos, Quaternion.identity);
                    GameObject chainArrowAux3 = (GameObject)myChainArrow3;
                    Chains3.Add(chainArrowAux3);

                    newDirectionWithShift = new Vector3((float)Mathf.Cos((angle - angleShift) * (Mathf.PI / 180)), (float)Mathf.Sin((angle - angleShift) * (Mathf.PI / 180)));

                    chainArrowAux3.GetComponent<Rigidbody2D>().velocity = newDirectionWithShift.normalized * speedChain;
                    chainArrowAux3.GetComponent<Rigidbody2D>().rotation = angle - angleShift - 90;

                    releasedArrow3 = true;
                }
                //else if ((Chains3[Chains3.Count - 1].transform.position - chainStartPos).magnitude > 0.27f)
                else if (elapsedTime3 > timeToReleaseChain)
                {
                    elapsedTime3 = 0.0f;
                    myChain3 = Instantiate(chain, chainStartPos, Quaternion.identity);
                    chainAux3 = (GameObject)myChain3;
                    Chains3.Add(chainAux3);

                    newDirectionWithShift = new Vector3((float)Mathf.Cos((angle - angleShift) * (Mathf.PI / 180)), (float)Mathf.Sin((angle - angleShift) * (Mathf.PI / 180)));

                    chainAux3.GetComponent<Rigidbody2D>().velocity = newDirectionWithShift.normalized * speedChain;
                    chainAux3.GetComponent<Rigidbody2D>().rotation = angle - angleShift - 90;

                    releasedArrow3 = true;
                }
            }
        }
    }
}
