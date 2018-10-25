using UnityEngine;
using System.Collections;

public class flameThrower : MonoBehaviour {

    void OnParticleCollision(GameObject other)
    {
        if (other.tag.Equals("blueBall")) {
            other.GetComponent<BlueBallManager>().breakBall();
        }
        else if (other.tag.Equals("redBall"))
        {
            other.GetComponent<RedBallManager>().breakBall();
        }
        else if (other.tag.Equals("yellowBall"))
        {
            other.GetComponent<YellowBallManager>().breakBall();
        }
        else if (other.tag.Equals("orangeBall"))
        {
            other.GetComponent<OrangeBallManager>().breakBall();
        }
        else if (other.tag.Equals("pinkBall"))
        {
            other.GetComponent<PinkBallManager>().breakBall();
        }
        else if (other.tag.Equals("blackBall"))
        {
            other.GetComponent<BlackBallManager>().breakBall();
        }
        else if (other.tag.Equals("orangeWall"))
        {
            other.GetComponent<WallPieceController>().breakWall();
        }
        else if (other.tag.Equals("Bomb"))
        {
            if (!other.GetComponent<BombManager>().blastStarted)
            {
                other.GetComponent<BombManager>().igniteBomb();
            }
        }
    }
}
