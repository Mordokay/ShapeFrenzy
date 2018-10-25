using UnityEngine;
using System.Collections;

public class OrangeWallController : MonoBehaviour {

    Vector3 upPos;
    Vector3 downPos;
    Vector3 leftPos;
    Vector3 rightPos;

    float timeShiftToMakeWall;
    GameObject wallPiece;

    GameObject pieceAux;

    bool isVertical;

    float lastWallSpawnTime;

    float originalLimitX;
    float originalLimitY;
    public float mapScaleWalls;

    void Start () {
        originalLimitX = 13.5f;
        originalLimitY = 9.0f;
        timeShiftToMakeWall = 0.15f;

        lastWallSpawnTime = 0.0f;
        wallPiece = Resources.Load("WallPiece") as GameObject;

        if ((originalLimitX * mapScaleWalls) - Mathf.Abs(this.transform.position.x) < (originalLimitY * mapScaleWalls) - Mathf.Abs(this.transform.position.y)) {
            leftPos = this.transform.position + new Vector3(-1.0f, 0.0f, 0.0f);
            rightPos = this.transform.position + new Vector3(1.0f, 0.0f, 0.0f);

            pieceAux = Instantiate(wallPiece, this.transform.position, Quaternion.identity) as GameObject;
            pieceAux.transform.localScale = new Vector3(pieceAux.transform.localScale.x * 2, pieceAux.transform.localScale.y, pieceAux.transform.localScale.z);
            isVertical = false;
        }
        else {
            upPos = this.transform.position + new Vector3(0.0f, 1.0f, 0.0f);
            downPos = this.transform.position + new Vector3(0.0f, -1.0f, 0.0f);

            pieceAux = Instantiate(wallPiece, this.transform.position, Quaternion.identity) as GameObject;
            pieceAux.transform.localScale = new Vector3(pieceAux.transform.localScale.x, pieceAux.transform.localScale.y * 2, pieceAux.transform.localScale.z);
            isVertical = true;
        }

        
    }

    bool beyondLimit(Vector3 pos) {
        return ((Mathf.Abs(pos.x) > (originalLimitX * mapScaleWalls)) || (Mathf.Abs(pos.y) > (originalLimitY * mapScaleWalls)));
    }

	void FixedUpdate () {
        lastWallSpawnTime += Time.deltaTime;

        if (lastWallSpawnTime > timeShiftToMakeWall) {
            if (isVertical)
            {
                if (!beyondLimit(upPos))
                {
                    pieceAux = Instantiate(wallPiece, upPos, Quaternion.identity) as GameObject;
                    pieceAux.transform.localScale = new Vector3(pieceAux.transform.localScale.x, pieceAux.transform.localScale.y * 2, pieceAux.transform.localScale.z);
                    upPos += new Vector3(0.0f, 1.0f, 0.0f);
                }
                if (!beyondLimit(downPos))
                {
                    pieceAux = Instantiate(wallPiece, downPos, Quaternion.identity) as GameObject;
                    pieceAux.transform.localScale = new Vector3(pieceAux.transform.localScale.x, pieceAux.transform.localScale.y * 2, pieceAux.transform.localScale.z);
                    downPos += new Vector3(0.0f, -1.0f, 0.0f);
                }
                if (beyondLimit(upPos) && beyondLimit(downPos))
                {
                    Destroy(this.gameObject);
                }
            }
            else
            {
                if (!beyondLimit(leftPos))
                {
                    pieceAux = Instantiate(wallPiece, leftPos, Quaternion.identity) as GameObject;
                    pieceAux.transform.localScale = new Vector3(pieceAux.transform.localScale.x * 2, pieceAux.transform.localScale.y, pieceAux.transform.localScale.z);
                    leftPos += new Vector3(-1.0f, 0.0f, 0.0f);
                }
                if (!beyondLimit(rightPos))
                {
                    pieceAux = Instantiate(wallPiece, rightPos, Quaternion.identity) as GameObject;
                    pieceAux.transform.localScale = new Vector3(pieceAux.transform.localScale.x * 2, pieceAux.transform.localScale.y, pieceAux.transform.localScale.z);
                    rightPos += new Vector3(1.0f, 0.0f, 0.0f);
                }
                if (beyondLimit(leftPos) && beyondLimit(rightPos))
                {
                    Destroy(this.gameObject);
                }
            }
            lastWallSpawnTime = 0.0f;
        }
	}
}
