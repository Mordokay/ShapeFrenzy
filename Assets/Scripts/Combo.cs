using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Combo : MonoBehaviour {

    public float minSize;
    public float maxSize;

    public float verticalMovement;
    public float startTime;
    float speed;
    public float animationTime = 0.5f;
    public string textInfo;

    Vector3 startPos;
    public bool fixedPos;

    void Start() {
        minSize = 20.0f;
        maxSize = 60.0f;
        speed = 1.0f;
        verticalMovement = 1.0f;
        startTime = Time.time;
        startPos = this.transform.position;
        this.GetComponent<TextMesh>().fontSize = (int)minSize;
        this.GetComponent<TextMesh>().text = textInfo;
        //this.GetComponent<TextMesh>().color = new Color32((byte)this.GetComponent<TextMesh>().color.r, (byte)this.GetComponent<TextMesh>().color.g, (byte)this.GetComponent<TextMesh>().color.b, 255);
    }

    void FixedUpdate() {
        if (!fixedPos)
        {
            transform.position = new Vector3(startPos.x, startPos.y + ((Time.time - startTime) / animationTime) * speed, 0.0f);
        }

         if ((Time.time - startTime) / animationTime > 1)
        {
            Destroy(this.gameObject);
        }
        else {

            float x = (Time.time - startTime) / animationTime;
            this.GetComponent<TextMesh>().fontSize = (int)(x * maxSize) + (int)minSize;
            x *= 255;
            //this.GetComponent<TextMesh>().color = new Color32(218, 10, 83, (byte)x);
        }
    }
}
