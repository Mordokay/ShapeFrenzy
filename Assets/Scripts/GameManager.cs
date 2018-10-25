using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

    public bool isPaused;
    public GameObject player;

    public GameObject toggleMute;
    public GameObject toggleFeedback;
    public GameObject toggleFreejoystickMode;
    public GameObject mainMenu;
    public GameObject optionsMenu;
    public GameObject helpMenu;
    public Button continueButton;
    public Slider volumeSlider;
    public Slider itemRatioSlider;
    public Text itemRatioValue;

    public GameObject controlsText;
    public GameObject RedBallinfo;
    public GameObject BlueBallInfo;
    public GameObject YellowBallInfo;

    public float lastVolume;

    public GameObject heart1;
    public GameObject heart2;
    public GameObject heart3;
    public GameObject heart4;
    public GameObject heart5;

    public Text scoreText;
    public Text highScoreText;

    public int numberOfPoints;
    public int numberOfLives;
    public int highScore;
    public int combo;

    //thisvalue controlls the combo value
    public int comboValue;
    public int maxCombo;
    //this value controls the scale of the bar
    public float comboBarValue;
    //value of combo loss over time
    float comboDecreaseValue = 0.05f;

    public BarController barController;

    public bool hourGlassActivated;
    public float hourGlassTime;

    public bool isTripleChainning;
    public float tripleChainTime;

    float maxItemRatio;

    public bool flameThrowerActivated;
    public float flameThrowerTime;

    public bool showFeedback;

    //Saves all the balls in the game
    public List<GameObject> AllBalls;

    public GameObject flameThrowerParticles;

    ProgressionManager progressionManager;
    bool textFileSaved;

    public InputField playerNameBox;

    public float chainsReleased;
    public float chainBallHits;
    public float chainBallMiss;

    void Start()
    {
        chainsReleased = 0;
        chainBallHits = 0;
        chainBallMiss = 0;

        textFileSaved = false;
        progressionManager = GameObject.FindGameObjectWithTag("Tube").GetComponent<ProgressionManager>();

        //garbage Collector every New Game
        System.GC.Collect();

        AllBalls = new List<GameObject>();
        AllBalls.Clear();

        maxItemRatio = 100000.0f;
        isTripleChainning = true;
        comboValue = 1;
        maxCombo = 1;
        comboBarValue = 0.0f;
        barController = this.GetComponent<BarController>();
        barController.barDisplay = comboBarValue;
        barController.barText = "Combo level " + comboValue;

        if (Time.time == 0)
        {
            Time.timeScale = 0.0f;
            isPaused = true;
            Cursor.visible = true;
            showHelpMenu();
            continueButton.interactable = false;
        }
        lastVolume = 50.0f;
        isPaused = false;

        numberOfPoints = 0;
        numberOfLives = 3;
        combo = 0;
        highScore = PlayerPrefs.GetInt("highscore");

        if (PlayerPrefs.GetInt("FreeJoystick") == 1)
        {
            this.GetComponent<TouchManager>().freeTouchMovement = true;
            toggleFreejoystickMode.GetComponent<Toggle>().isOn = true;

        }
        else {
            this.GetComponent<TouchManager>().freeTouchMovement = false;
            toggleFreejoystickMode.GetComponent<Toggle>().isOn = false;
        }

        if (PlayerPrefs.GetInt("ShowFeedback") == 1)
        {
            showFeedback = true;
            toggleFeedback.GetComponent<Toggle>().isOn = true;

        }
        else
        {
            showFeedback = false;
            toggleFeedback.GetComponent<Toggle>().isOn = false;
        }

        playerNameBox.text = PlayerPrefs.GetString("CurrentPlayer");
    }

    public void resetHighScore()
    {
        PlayerPrefs.SetInt("highscore", 0);
        this.highScore = 0;
    }

    public void showMenu()
    {
        mainMenu.SetActive(true);
        continueButton.interactable = true;
        optionsMenu.SetActive(false);
        helpMenu.SetActive(false);
    }

    public void showHelpMenu()
    {
        mainMenu.SetActive(true);
        continueButton.interactable = true;
        optionsMenu.SetActive(false);
        helpMenu.SetActive(true);
        showInstructions();
    }

    public void hideMenu()
    {
        mainMenu.SetActive(false);
        optionsMenu.SetActive(false);
        helpMenu.SetActive(false);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void NewGame()
    {
        Time.timeScale = 1.0f;
        isPaused = false;
        SceneManager.LoadScene(0);
    }

    public void showInstructions()
    {
        mainMenu.SetActive(false);
        helpMenu.SetActive(true);
        showControlsInfo();
    }

    public void OptionsMenu()
    {
        mainMenu.SetActive(false);
        optionsMenu.SetActive(true);
    }

    public void returnToMainMenu()
    {
        mainMenu.SetActive(true);
        optionsMenu.SetActive(false);
        helpMenu.SetActive(false);
    }

    public void returnToGame()
    {
        Time.timeScale = 1.0f;
        isPaused = false;
        Cursor.visible = false;
        hideMenu();
    }

    public void CheckMute()
    {
        if (toggleMute.GetComponent<Toggle>().isOn)
        {
            AudioListener.pause = true;
            lastVolume = volumeSlider.value;
            volumeSlider.value = 0;
        }
        else
        {
            AudioListener.pause = false;
            volumeSlider.value = lastVolume;
        }
    }

    public void CheckFeedback()
    {
        if (toggleFeedback.GetComponent<Toggle>().isOn)
        {
            showFeedback = true;
            PlayerPrefs.SetInt("ShowFeedback", 1);
        }
        else
        {
            showFeedback = false;
            PlayerPrefs.SetInt("ShowFeedback", 0);
        }
    }

    public void CheckFreeJoystick()
    {
        if (toggleFreejoystickMode.GetComponent<Toggle>().isOn)
        {
            PlayerPrefs.SetInt("FreeJoystick", 1);
            this.GetComponent<TouchManager>().freeTouchMovement = true;
        }
        else
        {
            PlayerPrefs.SetInt("FreeJoystick", 0);
            this.GetComponent<TouchManager>().freeTouchMovement = false;
        }
    }

    public void updateNickname() {
        PlayerPrefs.SetString("CurrentPlayer", playerNameBox.text);
    }

    public void CheckSoundVolume()
    {
        AudioListener.volume = volumeSlider.value;
        if (volumeSlider.value == 0)
        {
            toggleMute.GetComponent<Toggle>().isOn = true;
        }
        else
        {
            toggleMute.GetComponent<Toggle>().isOn = false;
        }
    }

    public void CheckItemRatio()
    {
        if (Time.timeSinceLevelLoad != 0)
        {
            itemRatioValue.text = ((int)(itemRatioSlider.value * maxItemRatio)).ToString();
            this.GetComponent<ItemSpawner>().frequencyDrop = (int)(itemRatioSlider.value * maxItemRatio);
            PlayerPrefs.SetFloat("itemRatio", itemRatioSlider.value);
        }
    }

    public void showControlsInfo()
    {
        controlsText.SetActive(true);
        RedBallinfo.SetActive(false);
        BlueBallInfo.SetActive(false);
        YellowBallInfo.SetActive(false);
    }
    public void showRedBallinfo()
    {
        controlsText.SetActive(false);
        RedBallinfo.SetActive(true);
        BlueBallInfo.SetActive(false);
        YellowBallInfo.SetActive(false);
    }
    public void showBlueBallInfo()
    {
        controlsText.SetActive(false);
        RedBallinfo.SetActive(false);
        BlueBallInfo.SetActive(true);
        YellowBallInfo.SetActive(false);
    }
    public void showYellowBallInfo()
    {
        controlsText.SetActive(false);
        RedBallinfo.SetActive(false);
        BlueBallInfo.SetActive(false);
        YellowBallInfo.SetActive(true);
    }

    public void handleUIMenuButton() {
        if (isPaused)
        {
            Time.timeScale = 1.0f;
            isPaused = false;
            hideMenu();
        }
        else
        {
            Time.timeScale = 0.0f;
            isPaused = true;
            continueButton.interactable = true;
            showMenu();
        }
    }
    public void releaseBalls()
    {
        foreach (GameObject ball in AllBalls)
        {
            switch (ball.tag)
            {
                case "blueBall":
                    if (ball.GetComponent<BlueBallManager>().lastVelocity.Equals(Vector3.zero))
                    {
                        ball.GetComponent<BlueBallManager>().lastVelocity = new Vector3(Random.Range(-20.0f, 20.0f), Random.Range(-20.0f, 20.0f), 0.0f);
                    }
                    ball.GetComponent<Rigidbody2D>().velocity = ball.GetComponent<BlueBallManager>().lastVelocity;
                    ball.GetComponent<BlueBallManager>().velocityLock = false;
                    break;
                case "redBall":
                    if (ball.GetComponent<RedBallManager>().lastVelocity.Equals(Vector3.zero))
                    {
                        ball.GetComponent<RedBallManager>().lastVelocity = new Vector3(Random.Range(-20.0f, 20.0f), Random.Range(-20.0f, 20.0f), 0.0f);
                    }
                    ball.GetComponent<Rigidbody2D>().velocity = ball.GetComponent<RedBallManager>().lastVelocity;
                    ball.GetComponent<RedBallManager>().velocityLock = false;
                    break;
                case "yellowBall":
                    if (ball.GetComponent<YellowBallManager>().lastVelocity.Equals(Vector3.zero))
                    {
                        ball.GetComponent<YellowBallManager>().lastVelocity = new Vector3(Random.Range(-20.0f, 20.0f), Random.Range(-20.0f, 20.0f), 0.0f);
                    }
                    ball.GetComponent<Rigidbody2D>().velocity = ball.GetComponent<YellowBallManager>().lastVelocity;
                    ball.GetComponent<YellowBallManager>().velocityLock = false;
                    break;
                case "orangeBall":
                    if (ball.GetComponent<OrangeBallManager>().lastVelocity.Equals(Vector3.zero))
                    {
                        ball.GetComponent<OrangeBallManager>().lastVelocity = new Vector3(Random.Range(-20.0f, 20.0f), Random.Range(-20.0f, 20.0f), 0.0f);
                    }
                    ball.GetComponent<Rigidbody2D>().velocity = ball.GetComponent<OrangeBallManager>().lastVelocity;
                    ball.GetComponent<OrangeBallManager>().velocityLock = false;
                    break;
                case "pinkBall":
                    if (ball.GetComponent<PinkBallManager>().lastVelocity.Equals(Vector3.zero))
                    {
                        ball.GetComponent<PinkBallManager>().lastVelocity = new Vector3(Random.Range(-20.0f, 20.0f), Random.Range(-20.0f, 20.0f), 0.0f);
                    }
                    ball.GetComponent<Rigidbody2D>().velocity = ball.GetComponent<PinkBallManager>().lastVelocity;
                    ball.GetComponent<PinkBallManager>().velocityLock = false;
                    break;
                case "blackBall":
                    if (ball.GetComponent<BlackBallManager>().lastVelocity.Equals(Vector3.zero))
                    {
                        ball.GetComponent<BlackBallManager>().lastVelocity = new Vector3(Random.Range(-20.0f, 20.0f), Random.Range(-20.0f, 20.0f), 0.0f);
                    }
                    ball.GetComponent<Rigidbody2D>().velocity = ball.GetComponent<BlackBallManager>().lastVelocity;
                    ball.GetComponent<BlackBallManager>().velocityLock = false;
                    break;
            }
        }
    }

    public void activateBubble() {
        /*isTripleChainning = false;
        tripleChainTime = 0.0f;
        flameThrowerActivated = false;
        flameThrowerTime = 0.0f;*/
        player.GetComponent<PlayerMovement>().disableSuperStarMode();

        //player.GetComponent<PlayerMovement>().shotgunAmmo = 0;
        //player.GetComponent<PlayerMovement>().shotgunActive = false;
    }

    public void activateFlame()
    {
        isTripleChainning = false;
        tripleChainTime = 0.0f;
        //player.GetComponent<PlayerMovement>().disableSuperStarMode();
        //player.GetComponent<PlayerMovement>().disableShield();

        player.GetComponent<PlayerMovement>().shotgunAmmo = 0;
        player.GetComponent<PlayerMovement>().shotgunActive = false;
    }

    public void activateTriple() {
        //player.GetComponent<PlayerMovement>().disableSuperStarMode();
        //player.GetComponent<PlayerMovement>().disableShield();
        flameThrowerActivated = false;
        flameThrowerTime = 0.0f;
        flameThrowerParticles.SetActive(false);

        player.GetComponent<PlayerMovement>().shotgunAmmo = 0;
        player.GetComponent<PlayerMovement>().shotgunActive = false;
    }

    public void activateSuperStar()
    {
        player.GetComponent<PlayerMovement>().disableShield();
        /*flameThrowerActivated = false;
        flameThrowerTime = 0.0f;
        isTripleChainning = false;
        tripleChainTime = 0.0f;

        player.GetComponent<PlayerMovement>().shotgunAmmo = 0;
        player.GetComponent<PlayerMovement>().shotgunActive = false;*/
    }

    public void activateShotgun()
    {
        //player.GetComponent<PlayerMovement>().disableSuperStarMode();
        //player.GetComponent<PlayerMovement>().disableShield();
        flameThrowerActivated = false;
        flameThrowerTime = 0.0f;
        flameThrowerParticles.SetActive(false);
        isTripleChainning = false;
        tripleChainTime = 0.0f;
    }

    public void removeWeaponsAndAddons()
    {
        player.GetComponent<PlayerMovement>().disableSuperStarMode();
        player.GetComponent<PlayerMovement>().disableShield();
        flameThrowerActivated = false;
        flameThrowerTime = 0.0f;
        flameThrowerParticles.SetActive(false);
        isTripleChainning = false;
        tripleChainTime = 0.0f;
        player.GetComponent<PlayerMovement>().shotgunAmmo = 0;
        player.GetComponent<PlayerMovement>().shotgunActive = false;
        hourGlassActivated = false;
    }

    void Update()
    {
        //Combo updater
        if (comboBarValue < 0)
        {
            if (comboValue > 1)
            {
                comboValue -= 1;
                comboBarValue = 1.0f;
                barController.barText = "Combo level " + comboValue;
            }
        }
        else if (comboBarValue > 1.0f)
        {
            comboValue += 1;
            if (comboValue > maxCombo) {
                maxCombo = comboValue;
            }
            comboBarValue = 0.5f;
            barController.barText = "Combo level " + comboValue;
        }
        else {
            comboBarValue -= (Time.deltaTime * comboDecreaseValue * (comboValue / 5.0f));
            Mathf.Clamp(comboBarValue, 0.0f, 1.0f);
            this.GetComponent<BarController>().barDisplay = comboBarValue;
        }

        //HourGlass updater
        if (hourGlassActivated)
        {
            foreach (GameObject ball in AllBalls) {
                switch (ball.tag)
                {
                    case "blueBall":
                        if (!ball.GetComponent<BlueBallManager>().velocityLock)
                        {
                            ball.GetComponent<BlueBallManager>().lastVelocity = ball.GetComponent<Rigidbody2D>().velocity;
                            ball.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
                            ball.GetComponent<BlueBallManager>().velocityLock = true;
                        }
                        break;
                    case "redBall":
                        if (!ball.GetComponent<RedBallManager>().velocityLock)
                        {
                            ball.GetComponent<RedBallManager>().lastVelocity = ball.GetComponent<Rigidbody2D>().velocity;
                            ball.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
                            ball.GetComponent<RedBallManager>().velocityLock = true;
                        }
                        break;
                    case "yellowBall":
                        if (!ball.GetComponent<YellowBallManager>().velocityLock)
                        {
                            ball.GetComponent<YellowBallManager>().lastVelocity = ball.GetComponent<Rigidbody2D>().velocity;
                            ball.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
                            ball.GetComponent<YellowBallManager>().velocityLock = true;
                        }
                        break;
                    case "orangeBall":
                        if (!ball.GetComponent<OrangeBallManager>().velocityLock)
                        {
                            ball.GetComponent<OrangeBallManager>().lastVelocity = ball.GetComponent<Rigidbody2D>().velocity;
                            ball.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
                            ball.GetComponent<OrangeBallManager>().velocityLock = true;
                        }
                        break;
                    case "pinkBall":
                        if (!ball.GetComponent<PinkBallManager>().velocityLock)
                        {
                            ball.GetComponent<PinkBallManager>().lastVelocity = ball.GetComponent<Rigidbody2D>().velocity;
                            ball.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
                            ball.GetComponent<PinkBallManager>().velocityLock = true;
                        }
                        break;
                    case "blackBall":
                        if (!ball.GetComponent<BlackBallManager>().velocityLock)
                        {
                            ball.GetComponent<BlackBallManager>().lastVelocity = ball.GetComponent<Rigidbody2D>().velocity;
                            ball.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
                            ball.GetComponent<BlackBallManager>().velocityLock = true;
                        }
                        break; 
                }
            }
            hourGlassTime -= Time.deltaTime;
            if (hourGlassTime < 0) {
                hourGlassActivated = false;
                releaseBalls();
            }
        }

        //TripleChainning updater
        if (isTripleChainning) {
            tripleChainTime -= Time.deltaTime;
            if(tripleChainTime < 0) {
                isTripleChainning = false;
                this.GetComponent<ChainManager>().isChainning = false;
                this.GetComponent<ChainManager>().isChainning2 = false;
                this.GetComponent<ChainManager>().isChainning3 = false;
            }
        }

        //Flame Thrower updater
        if (flameThrowerActivated) {
            flameThrowerTime -= Time.deltaTime;
            if (flameThrowerTime < 0)
            {
                flameThrowerTime = 0.0f;
                flameThrowerActivated = false;
                flameThrowerParticles.SetActive(false);
            }
        }

        if (numberOfPoints > highScore)
        {
            highScore = numberOfPoints;
            PlayerPrefs.SetInt("highscore", highScore);
        }

        scoreText.text = "SCORE: " + numberOfPoints;
        highScoreText.text = "HIGHSCORE: " + highScore.ToString();

        //Number of lives Controller
        switch (numberOfLives)
        {
            case 0:
                mainMenu.SetActive(true);
                isPaused = true;
                continueButton.interactable = false;

                if (!textFileSaved)
                {

                    this.GetComponent<WriteToFile>().prepareFile();
                    this.GetComponent<WriteToFile>().writeToFile("Time Elapsed: " + Time.time + " SCORE: " + numberOfPoints +  " MAX COMBO: " + maxCombo);
                    this.GetComponent<WriteToFile>().writeToFile("Number of released chains: " + chainsReleased + " Balls Missed: " + chainBallMiss + " Ball Hits: "  + chainBallHits);
                    this.GetComponent<WriteToFile>().writeToFile(progressionManager.getLog());
                    this.GetComponent<WriteToFile>().writeToFile("\n//////////////////////////////////////////////////////////////////////////////////");
                    this.GetComponent<WriteToFile>().writeToFile("//////////////////////////////////////////////////////////////////////////////////\n");
                    this.GetComponent<WriteToFile>().closeFile();
                    textFileSaved = true;
                }

                //Time.timeScale = 0.0f;
                player.SetActive(false);
                heart1.SetActive(false);
                heart2.SetActive(false);
                heart3.SetActive(false);
                heart4.SetActive(false);
                heart5.SetActive(false);
                break;
            case 1:
                heart1.SetActive(true);
                heart2.SetActive(false);
                heart3.SetActive(false);
                heart4.SetActive(false);
                heart5.SetActive(false);
                break;
            case 2:
                heart1.SetActive(true);
                heart2.SetActive(true);
                heart3.SetActive(false);
                heart4.SetActive(false);
                heart5.SetActive(false);
                break;
            case 3:
                heart1.SetActive(true);
                heart2.SetActive(true);
                heart3.SetActive(true);
                heart4.SetActive(false);
                heart5.SetActive(false);
                break;
            case 4:
                heart1.SetActive(true);
                heart2.SetActive(true);
                heart3.SetActive(true);
                heart4.SetActive(true);
                heart5.SetActive(false);
                break;
            case 5:
                heart1.SetActive(true);
                heart2.SetActive(true);
                heart3.SetActive(true);
                heart4.SetActive(true);
                heart5.SetActive(true);
                break;
            default:
                break;
        }
    }
}
