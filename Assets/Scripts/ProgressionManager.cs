using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ProgressionManager : MonoBehaviour
{
    int StageLimit;
    int waveLimit;
    
    float ballShift;
    float timeShift;
    float initialBalls;
    float initialTime;

    float totalTime;
    float newWaveWaitTime;
    public List<ball> balls;

    //aditional variables
    float stageInitialBalls;
    float stageInitialTime;
    float waveBalls;
    float waveTime;

    float[] redMinRange;
    float[] redMaxRange;
    float[] yellowMinRange;
    float[] yellowMaxRange;
    float[] blueMinRange;
    float[] blueMaxRange;
    float[] orangeMinRange;
    float[] orangeMaxRange;
    float[] pinkMinRange;
    float[] pinkMaxRange;
    float[] blackMinRange;
    float[] blackMaxRange;

    float redMinLastRange;
    float redMaxLastRange;
    float yellowLastMinRange;
    float yellowLastMaxRange;
    float blueLastMinRange;
    float blueLastMaxRange;
    float orangeLastMinRange;
    float orangeLastMaxRange;
    float pinkLastMinRange;
    float pinkLastMaxRange;
    float blackLastMinRange;
    float blackLastMaxRange;

    float percentageShift;

    GameObject[] items;

    GameObject player;
    public WaveBarController waveBarController;

    bool shiftLock;

    public int lastStage;
    public int lastWave;
    GameManager gm;

    float delayBallSpawn;

    float initialStageSize;
    float stageSizeShift;
    int stageSizeLimit;
    public float scaleMag;

    public GameObject backround;
    public GameObject map;
    public GameObject path;
    Vector3 backroundInitialScale;
    Vector3 mapInitialScale;
    Vector3 pathInitialScale;

    float totalTimeShift;

    //Item variables
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

    public string myLog;
    float waveDuration;

    public class ball {
        public float releaseTime;
        public int stage;
        public int wave;

        public bool hasItem;
        public GameObject itemToSpawn;

        public ball(float ballReleaseTime, int ballStage, int waveStage)
        {
            releaseTime = ballReleaseTime;
            stage = ballStage;
            wave = waveStage;
            hasItem = false;
        }
    }

    void Start()
    {
        myLog = "";
        waveDuration = 0.0f;

         Heart = Resources.Load("Heart") as GameObject;
        Shield = Resources.Load("ShieldItem") as GameObject;
        HourGlass = Resources.Load("HourGlass") as GameObject;
        Star = Resources.Load("Star") as GameObject;
        TripleChain = Resources.Load("TripleChain") as GameObject;
        FlameThrower = Resources.Load("FlameThrower") as GameObject;
        Shotgun = Resources.Load("Shotgun") as GameObject;

        heartMinRange = 0;
        heartMaxRange = 29;
        shieldMinRange = 30;
        shieldMaxRange = 47;
        hourGlassMinRange = 48;
        hourGlassMaxRange = 64;
        starMinRange = 65;
        starMaxRange = 74;
        tripleChainMinRange = 75;
        tripleChainMaxRange = 90;
        flameThrowerMinRange = 91;
        flameThrowerMaxRange = 100;
        shotgunMinRange = 101;
        shotgunMaxRange = 150;


        totalTimeShift = 0.0f;

        initialStageSize = 0.75f;
        stageSizeShift = 0.01f;
        stageSizeLimit = 20;
        backroundInitialScale = backround.transform.localScale;
        mapInitialScale = map.transform.localScale;
        pathInitialScale = path.transform.localScale;

        delayBallSpawn = 1.0f;

        player = GameObject.FindGameObjectWithTag("Player");

        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        waveBarController = gm.GetComponent<WaveBarController>();

        gm.GetComponent<AnnouncementController>().enabled = false;

        waveBarController.disable();
        /*if (!gm.showFeedback)
        {
            waveBarController.disable();
        }
        else
        {
            waveBarController.enable();
        }*/

        lastStage = -1;
        lastWave = -1;
        shiftLock = false;

        redMinRange =    new float[] {      0.0f,       0.0f,       0.0f,       0.0f,       0.0f,       0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f };
        redMaxRange =    new float[] {     75.0f,      60.0f,      50.0f,      35.0f,      40.0f,      30.0f, 30.0f, 30.0f, 30.0f, 30.0f, 30.0f };

        yellowMinRange = new float[] {      76.0f,      61.0f,      51.0f,      36.0f,      41.0f,      31.0f, 31.0f, 31.0f, 31.0f, 31.0f, 31.0f };
        yellowMaxRange = new float[] {      100.0f,     100.0f,      80.0f,      60.0f,      54.0f,      50.0f, 50.0f, 50.0f, 50.0f, 50.0f, 50.0f };

        orangeMinRange = new float[] {      0.0f,       0.0f,       81.0f,       61.0f,       55.0f,       51.0f, 51.0f, 51.0f, 51.0f, 51.0f, 51.0f };
        orangeMaxRange = new float[] {      0.0f,       0.0f,      100.0f,       70.0f,       65.0f,       61.0f, 61.0f, 61.0f, 61.0f, 61.0f, 61.0f };

        blueMinRange =    new float[] {     0.0f,       0.0f,       0.0f,      71.0f,      66.0f,      62.0f, 62.0f, 62.0f, 62.0f, 62.0f, 62.0f };
        blueMaxRange =    new float[] {     0.0f,       0.0f,       0.0f,     100.0f,      83.0f,      80.0f, 80.0f, 80.0f, 80.0f, 80.0f, 80.0f };

        blackMinRange = new float[] {       0.0f,      0.0f,      0.0f,       0.0f,       84.0f,       81.0f, 81.0f, 81.0f, 81.0f, 81.0f, 81.0f };
        blackMaxRange = new float[] {       0.0f,      0.0f,      0.0f,       0.0f,      100.0f,       100.0f, 100.0f, 100.0f, 100.0f, 100.0f, 100.0f };

        //pinkMinRange = new float[] { 0.0f, 0.0f, 96.0f, 91.0f, 86.0f, 81.0f, 81.0f, 81.0f, 81.0f, 76.0f, 76.0f };
        //pinkMaxRange = new float[] { 0.0f, 0.0f, 100.0f, 100.0f, 100.0f, 100.0f, 100.0f, 100.0f, 100.0f, 100.0f, 100.0f };

        percentageShift = 60.0f;

        balls = new List<ball>();
        StageLimit = 50;
        waveLimit = 3;

        ballShift = 1.0f;

        initialBalls = 2;
        //initialTime = 60;
        initialTime = 20;

        timeShift = initialTime / initialBalls * 0.8f;

        //newWaveWaitTime = 30.0f;
        newWaveWaitTime = (timeShift * 2);
        totalTime = 0.0f;

        //Debug.Log("Time shift: " + timeShift);

        for (int stage = 0; stage < StageLimit; stage++) {
            stageInitialBalls = initialBalls + (ballShift * stage);
            stageInitialTime = initialTime + (timeShift * stage);

            for (int wave = 0; wave < waveLimit; wave++) {
                //Debug.Log("Stage: " + stage + "Wave: " + wave);

                waveBalls = stageInitialBalls + (ballShift * wave);
                waveTime = stageInitialTime + (timeShift * wave);

                int itemSpawn = (int)(Random.Range(0.0f, waveBalls));

               // Debug.Log("Balls: " + waveBalls + " Time: " + waveTime + " mValue " + mValue);
                for (int ballNum = 0; ballNum <= waveBalls; ballNum ++ ) {

                    float percentage = (-percentageShift / (waveBalls - 1)) * (ballNum - 1) + ((percentageShift / 2) + 100);
                    percentage /= 100;

                    if (ballNum != 0)
                    {
                        totalTime += (waveTime / waveBalls) * percentage;
                    }

                    //Debug.Log("Time of ball " + ballNum + " is " + totalTime);

                    ball b = new ball(totalTime, stage, wave);
                    balls.Add(b);

                    if (ballNum == itemSpawn) {
                        b.hasItem = true;
                        int random = Random.Range(0, 150);
                        if (random >= heartMinRange && random <= heartMaxRange)
                            b.itemToSpawn = Heart;
                        else if (random >= shieldMinRange && random <= shieldMaxRange)
                            b.itemToSpawn = Shield;
                        else if (random >= hourGlassMinRange && random <= hourGlassMaxRange)
                            b.itemToSpawn = HourGlass;
                        else if (random >= starMinRange && random <= starMaxRange)
                            b.itemToSpawn = Star;
                        else if (random >= tripleChainMinRange && random <= tripleChainMaxRange)
                            b.itemToSpawn = TripleChain;
                        else if (random >= flameThrowerMinRange && random <= flameThrowerMaxRange)
                            b.itemToSpawn = FlameThrower;
                        else if (random >= shotgunMinRange && random <= shotgunMaxRange)
                            b.itemToSpawn = Shotgun;
                    }
                }
                totalTime += newWaveWaitTime;
            }
            //Debug.Log("///////////////////////////////////////////////////////");
        }
    }

    public string getLog(){
        return myLog;
    }

    void FixedUpdate()
    {
        if (!gm.isPaused && Time.timeScale != 0.0f)
        {
            waveDuration += Time.deltaTime;
            if (gm.AllBalls.Count == 0 && !shiftLock && Time.timeSinceLevelLoad > 0.5f)
            {
                shiftLock = true;
                float timeToSync = (balls[0].releaseTime - totalTimeShift) - Time.timeSinceLevelLoad;
                totalTimeShift += (timeToSync - delayBallSpawn);
            }
            else if (balls.Count > 0)
            {
                if (Time.timeSinceLevelLoad > (balls[0].releaseTime - totalTimeShift))
                {
                    if (balls[0].stage != lastStage)
                    {
                        if (gm.AllBalls.Count != 0)
                        {
                        }
                        else
                        {
                            if (Time.timeSinceLevelLoad > 0.5f)
                            {
                                myLog += "\nStage: " + (lastStage + 1) + " Wave: " + (lastWave + 1) + "Duration: " + waveDuration;
                                waveDuration = 0.0f;
                            }
                            lastStage = balls[0].stage;
                            lastWave = balls[0].wave;

                            float timeToSync = (balls[0].releaseTime - totalTimeShift) - Time.timeSinceLevelLoad;
                            totalTimeShift += (timeToSync - delayBallSpawn);

                            if (gm.showFeedback)
                            {
                                float timeOfThisWave = initialTime + (timeShift * lastStage) + (timeShift * lastWave);
                            
                                waveBarController.waveBarInitialValue = timeOfThisWave + newWaveWaitTime;
                                waveBarController.lifeTime = 0.0f;
                                waveBarController.comboText.text = "Wave Number 1";

                                gm.GetComponent<AnnouncementController>().enabled = true;
                                gm.GetComponent<AnnouncementController>().textInfo = "Stage " + (lastStage + 1) + "!!! \nBeginning the first Wave!!!";
                                gm.GetComponent<AnnouncementController>().Start();
                            }

                            if (balls[0].stage < stageSizeLimit)
                            {
                                scaleMag = initialStageSize + (balls[0].stage * stageSizeShift);
                                backround.transform.localScale = backroundInitialScale * scaleMag;
                                map.transform.localScale = mapInitialScale * scaleMag;
                                path.transform.localScale = pathInitialScale * scaleMag;
                            }
                            else
                            {
                                scaleMag = initialStageSize + (stageSizeLimit * stageSizeShift);
                                backround.transform.localScale = backroundInitialScale;
                                map.transform.localScale = mapInitialScale;
                                path.transform.localScale = pathInitialScale;
                            }
                            this.GetComponent<SplineController>().Start();

                            if (gm.numberOfLives < 5)
                                gm.numberOfLives += 1;

                            
                        }
                    }
                    else if (balls[0].wave != lastWave)
                    {
                        if (gm.AllBalls.Count != 0)
                        {
                        }
                        else
                        {
                            if (Time.timeSinceLevelLoad > 0.5f)
                            {
                                myLog += "\nStage: " + (lastStage + 1) + " Wave: " + (lastWave + 1) + "Duration: " + waveDuration;
                                waveDuration = 0.0f;
                            }
                            lastWave = balls[0].wave;
                            float timeToSync = (balls[0].releaseTime - totalTimeShift) - Time.timeSinceLevelLoad;
                            totalTimeShift += (timeToSync - delayBallSpawn);

                            if (gm.showFeedback)
                            {
                                gm.GetComponent<AnnouncementController>().enabled = true;

                                switch (lastWave)
                                {
                                    case 1:
                                        gm.GetComponent<AnnouncementController>().textInfo = "2nd Wave!";
                                        gm.GetComponent<AnnouncementController>().Start();
                                        break;
                                    case 2:
                                        gm.GetComponent<AnnouncementController>().textInfo = "3rd Wave!";
                                        gm.GetComponent<AnnouncementController>().Start();
                                        break;
                                    default:
                                        gm.GetComponent<AnnouncementController>().textInfo = (lastWave + 1) + "th Wave!";
                                        gm.GetComponent<AnnouncementController>().Start();
                                        break;
                                }


                                if (waveBarController.frontImage.fillAmount > 0)
                                {
                                    GameObject combo = Instantiate(Resources.Load("Combo"), player.transform.position, Quaternion.identity) as GameObject;
                                    combo.GetComponent<Combo>().fixedPos = true;
                                    combo.GetComponent<TextMesh>().color = new Color32(230, 160, 0, 255);
                                    combo.GetComponent<Combo>().animationTime = 3.0f;

                                    int points = (int)(waveBarController.frontImage.fillAmount * 100 * gm.comboValue);
                                    gm.numberOfPoints += points;
                                    combo.GetComponent<Combo>().textInfo = "Time points: " + points;
                                }

                                float timeOfThisWave = initialTime + (timeShift * lastStage) + (timeShift * lastWave);
                                waveBarController = gm.GetComponent<WaveBarController>();
                                waveBarController.waveBarInitialValue = timeOfThisWave + newWaveWaitTime;
                                waveBarController.lifeTime = 0.0f;
                                waveBarController.comboText.text = "Wave Number " + (lastWave + 1);
                            }
                            if(gm.numberOfLives < 5)
                                gm.numberOfLives += 1;

                            
                        }

                    }
                    else
                    {
                        shiftLock = false;

                        Vector3 direction = Vector3.up;
                        float angle = transform.eulerAngles.z * Mathf.Deg2Rad;
                        float sin = Mathf.Sin(angle);
                        float cos = Mathf.Cos(angle);
                        Vector3 forward = new Vector3(
                               direction.x * cos - direction.y * sin,
                               direction.x * sin + direction.y * cos,
                               0f);


                        int random = Random.Range(0, 100);
                        int randomBomb = Random.Range(0, 10);
                        int stage = Mathf.Clamp(lastStage, 0, 10);
                        if (random >= redMinRange[stage] && random <= redMaxRange[stage])
                        {
                            GameObject newBall = Instantiate(Resources.Load("RedBall", typeof(GameObject)), this.GetComponent<Transform>().position, Quaternion.identity) as GameObject;
                            newBall.GetComponent<Rigidbody2D>().velocity = forward * 10.0f;
                            if (balls[0].hasItem) {
                                newBall.GetComponent<RedBallManager>().itemToSpawn = balls[0].itemToSpawn;
                            }
                            gm.AllBalls.Add(newBall);
                        }
                        else if (random >= yellowMinRange[stage] && random <= yellowMaxRange[stage])
                        {
                            GameObject newBall = Instantiate(Resources.Load("YellowBall", typeof(GameObject)), this.GetComponent<Transform>().position, Quaternion.identity) as GameObject;
                            newBall.GetComponent<Rigidbody2D>().velocity = forward * 10.0f;
                            if (balls[0].hasItem)
                            {
                                newBall.GetComponent<YellowBallManager>().itemToSpawn = balls[0].itemToSpawn;
                            }
                            gm.AllBalls.Add(newBall);
                        }
                        else if (random >= blueMinRange[stage] && random <= blueMaxRange[stage])
                        {
                            GameObject newBall = Instantiate(Resources.Load("BlueBall", typeof(GameObject)), this.GetComponent<Transform>().position, Quaternion.identity) as GameObject;
                            newBall.GetComponent<Rigidbody2D>().velocity = forward * 10.0f;
                            if (balls[0].hasItem)
                            {
                                newBall.GetComponent<BlueBallManager>().itemToSpawn = balls[0].itemToSpawn;
                            }
                            gm.AllBalls.Add(newBall);
                        }
                        else if (random >= orangeMinRange[stage] && random <= orangeMaxRange[stage])
                        {
                            GameObject newBall = Instantiate(Resources.Load("OrangeBall", typeof(GameObject)), this.GetComponent<Transform>().position, Quaternion.identity) as GameObject;
                            newBall.GetComponent<Rigidbody2D>().velocity = forward * 10.0f;
                            if (balls[0].hasItem)
                            {
                                newBall.GetComponent<OrangeBallManager>().itemToSpawn = balls[0].itemToSpawn;
                            }
                            gm.AllBalls.Add(newBall);
                        }
                        else if (random >= pinkMinRange[stage] && random <= pinkMaxRange[stage])
                        {
                            GameObject newBall = Instantiate(Resources.Load("PinkBall", typeof(GameObject)), this.GetComponent<Transform>().position, Quaternion.identity) as GameObject;
                            newBall.GetComponent<Rigidbody2D>().velocity = forward * 10.0f;
                            if (balls[0].hasItem)
                            {
                                newBall.GetComponent<PinkBallManager>().itemToSpawn = balls[0].itemToSpawn;
                            }
                            gm.AllBalls.Add(newBall);
                        }
                        else if (random >= blackMinRange[stage] && random <= blackMaxRange[stage])
                        {
                            GameObject newBall = Instantiate(Resources.Load("BlackBall", typeof(GameObject)), this.GetComponent<Transform>().position, Quaternion.identity) as GameObject;
                            newBall.GetComponent<Rigidbody2D>().velocity = forward * 10.0f;
                            if (balls[0].hasItem)
                            {
                                newBall.GetComponent<BlackBallManager>().itemToSpawn = balls[0].itemToSpawn;
                            }
                            gm.AllBalls.Add(newBall);
                        }
                        if (randomBomb == 4)
                        {
                            GameObject newBall = Instantiate(Resources.Load("Bomb", typeof(GameObject)), this.GetComponent<Transform>().position, Quaternion.identity) as GameObject;
                            newBall.GetComponent<Rigidbody2D>().velocity = forward * 10.0f;
                            if (balls[0].hasItem)
                            {
                                newBall.GetComponent<BombManager>().itemToSpawn = balls[0].itemToSpawn;
                            }
                            gm.AllBalls.Add(newBall);
                        }

                        balls.RemoveAt(0);
                    }
                }
            }
        }
    }
}