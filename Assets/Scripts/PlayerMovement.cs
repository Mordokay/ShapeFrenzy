using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerMovement : MonoBehaviour {

    public float maximumSpeed;
    public float speedMovement;
    GameObject gameManager;
    public ChainManager chainManager;

    public Joystick joystick;
    public Joystick joystickChain;

    public Vector3 movementTouchpad;
    public Vector3 bulletTouchpad;
    public Vector3 leftTouch;
    public Vector3 rightTouch;

    public bool isImune;
    public float imunityTime;
    public float imuneTimeAux;

    Vector3 normalizedmovement;
    public TouchManager touchManager;

    Rigidbody2D rb;
    Camera cam;

    bool loggedChainRelease;

    public bool isShielded;
    public float shieldTime;
    public GameObject shield;

    public int shotgunAmmo;
    public bool shotgunActive;
    public float lastShotgunTime;
    public float shotgunIntervalBetweenShoots;
    public float shotgunShrapAngle;

    public bool superStarMode;
    public float superStarTime;
    public GameObject starParticles;
    Color playerColor;
    public Color superStarColor;
    public float speedBullet;

    public GameObject flameThrowerParticles;

    public float CameraOriginalLimitX = 4.0f;
    public float CameraOriginalLimitY = 7.0f;

    //Other variables
    GameObject[] blueBalls;
    GameObject[] redBalls;
    GameObject[] yellowBalls;
    float brakeSpeed;  // calculate the speed decrease
    Vector3 normalisedVelocity;
    Vector3 brakeVelocity;  // make the brake Vector3 value
    float speed;

    ProgressionManager pm;
    bool holdingShotgunController;

    bool mouseClick;
    bool usingkeyboard;
    Vector3 mouseClickDirection;

    public float screenProportionHeight;
    public float screenProportionWidth;

    void Start()
    {
        loggedChainRelease = false;
        screenProportionHeight = Screen.height / 574.0f;
        screenProportionWidth = Screen.width / 1174.0f;

        Debug.Log("Screen.height " + Screen.height + "Screen.width " + Screen.width);
        if (Application.platform == RuntimePlatform.WebGLPlayer || Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer)
            usingkeyboard = true;
        else
            usingkeyboard = false;

        pm = GameObject.FindGameObjectWithTag("Tube").GetComponent<ProgressionManager>();
        holdingShotgunController = false;
        shotgunShrapAngle = 5.0f;
        shotgunIntervalBetweenShoots = 0.5f;
        lastShotgunTime = shotgunIntervalBetweenShoots;
        speedBullet = 12.0f;
        playerColor = this.GetComponent<SpriteRenderer>().color;
        isImune = false;
        imunityTime = 2.0f;
        rb = GetComponent<Rigidbody2D>();
        gameManager = GameObject.Find("GameManager");
        cam = Camera.main;
        maximumSpeed = 6.0f;
        speedMovement = 6.0f;
        imuneTimeAux = imunityTime;
    }

    public void disableSuperStarMode() {
        superStarMode = false;
        starParticles.SetActive(false);
        superStarTime = 0.0f;
    }

    public void disableShield() {
        Color color = shield.GetComponent<SpriteRenderer>().color;
        color.a = 255;
        shield.GetComponent<SpriteRenderer>().color = color;
        this.GetComponent<SpriteRenderer>().color = playerColor;
        isShielded = false;
        shield.SetActive(false);
    }

    void ignoreBalls(bool value) {
        blueBalls = GameObject.FindGameObjectsWithTag("blueBall");
        redBalls = GameObject.FindGameObjectsWithTag("redBall");
        yellowBalls = GameObject.FindGameObjectsWithTag("yellowBall");

        foreach (GameObject blue in blueBalls)
        {
            Physics2D.IgnoreCollision(blue.GetComponent<Collider2D>(), this.GetComponent<Collider2D>(), value);
        }
        foreach (GameObject red in redBalls)
        {
            Physics2D.IgnoreCollision(red.GetComponent<Collider2D>(), this.GetComponent<Collider2D>(), value);
        }
        foreach (GameObject yellow in yellowBalls)
        {
            Physics2D.IgnoreCollision(yellow.GetComponent<Collider2D>(), this.GetComponent<Collider2D>(), value);
        }
    }

        // Update is called once per frame
        void Update () {
        if (isImune)
        {
            imuneTimeAux -= Time.deltaTime;
            if (imuneTimeAux < 0.0f)
            {
                imuneTimeAux = imunityTime;
                isImune = false;
            }
            if ((((int)(imuneTimeAux * 10.0f)) % 2) == 0)
                this.GetComponent<SpriteRenderer>().color = new Color(0, 200, 0, 0);
            else
            {
                this.GetComponent<SpriteRenderer>().color = new Color(0, 200, 0, 255);
            }
            ignoreBalls(true);
        }
        else if (superStarMode) {
            starParticles.SetActive(true);
            superStarTime -= Time.deltaTime;
            if (superStarTime < 0)
            {
                superStarMode = false;
                starParticles.SetActive(false);
            }
            Color color = superStarColor;
            if (superStarTime < 2 && superStarTime > 0)
            {
                if ((((int)(superStarTime * 10.0f)) % 2) == 0)
                {
                    color.a = 0;
                    this.GetComponent<SpriteRenderer>().color = color;
                }
                else
                {
                    color.a = 255;
                    this.GetComponent<SpriteRenderer>().color = color;
                }
            }
            else
            {
                color.a = 255;
                this.GetComponent<SpriteRenderer>().color = color;
            }
        }
        else if (isShielded)
        {
            shield.SetActive(true);
            shieldTime -= Time.deltaTime;

            Color color = shield.GetComponent<SpriteRenderer>().color;
            if (shieldTime < 2 && shieldTime > 0)
            {
                if ((((int)(shieldTime * 10.0f)) % 2) == 0)
                {
                    color.a = 0;
                    shield.GetComponent<SpriteRenderer>().color = color;
                }
                else
                {
                    color.a = 255;
                    shield.GetComponent<SpriteRenderer>().color = color;
                }
            }
            else
            {
                color.a = 255;
                shield.GetComponent<SpriteRenderer>().color = color;
                this.GetComponent<SpriteRenderer>().color = playerColor;
            }

            if (shieldTime < 0)
            {
                isShielded = false;
                shield.SetActive(false);
            }
        }
        else
        {
            this.GetComponent<SpriteRenderer>().color = playerColor;
            ignoreBalls(false);
        }

        // Debug.Log("screenProportionHeight: " + screenProportionHeight + "screenProportionWidth: " + screenProportionWidth);
        //Camera follow player to a fixed range so camera does not go "out" of the map
        //if (pm.scaleMag < 1.0f)
        //{
        //if (Application.platform != RuntimePlatform.WebGLPlayer)
        //{
        //    cam.transform.position = new Vector3(0.0f, Mathf.Clamp(this.transform.position.y, screenProportionHeight * (-CameraOriginalLimitY - ((pm.scaleMag - 1.0f) * 9.0f)), screenProportionHeight * (CameraOriginalLimitY + ((pm.scaleMag - 1.0f) * 9.0f))), cam.transform.position.z);
        //}
        //else
        //{
        //    cam.transform.position = this.transform.position;
        //}
        //}
        //else
        //{
        //if (Application.platform != RuntimePlatform.WebGLPlayer)
        //{

        cam.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, cam.transform.position.z);
        //cam.transform.position = new Vector3(Mathf.Clamp(this.transform.position.x, screenProportionWidth  * (- CameraOriginalLimitX - ((pm.scaleMag - 1.0f) * 15.0f)), screenProportionWidth * (CameraOriginalLimitX + ((pm.scaleMag - 1.0f) * 15.0f))),
          //      Mathf.Clamp(this.transform.position.y, -screenProportionHeight * (CameraOriginalLimitY - ((pm.scaleMag - 1.0f) * 9.0f)), screenProportionHeight *  (CameraOriginalLimitY + ((pm.scaleMag - 1.0f) * 9.0f))), cam.transform.position.z);

            //}
            //else {
            //    cam.transform.position = this.transform.position;
            //}
        //}

        float AngleRad;
        float angle;
        if (!gameManager.GetComponent<GameManager>().isPaused)
        {

            if (!usingkeyboard)
            {
                ///left Joystick
                if (joystick.m_VerticalVirtualAxis.GetValue + joystick.m_HorizontalVirtualAxis.GetValue != 0)
                {
                    AngleRad = Mathf.Atan2(joystick.m_VerticalVirtualAxis.GetValue, joystick.m_HorizontalVirtualAxis.GetValue);
                    angle = (180 / Mathf.PI) * AngleRad;
                    rb.rotation = angle - 90;
                    this.GetComponent<Rigidbody2D>().velocity =
                        new Vector2(joystick.m_HorizontalVirtualAxis.GetValue, joystick.m_VerticalVirtualAxis.GetValue) * speedMovement;
                }


                ///Right Joystick
                if (joystickChain.m_VerticalVirtualAxis.GetValue + joystickChain.m_HorizontalVirtualAxis.GetValue != 0)
                {
                    if (gameManager.GetComponent<GameManager>().flameThrowerActivated)
                    {
                        AngleRad = Mathf.Atan2(joystickChain.m_VerticalVirtualAxis.GetValue, joystickChain.m_HorizontalVirtualAxis.GetValue);
                        angle = (180 / Mathf.PI) * AngleRad;
                        rb.rotation = angle - 90;

                        if (gameManager.GetComponent<GameManager>().flameThrowerTime < 0)
                        {
                            gameManager.GetComponent<GameManager>().flameThrowerActivated = false;
                            flameThrowerParticles.SetActive(false);
                        }
                        else
                        {
                            flameThrowerParticles.SetActive(true);
                        }
                    }
                    else if (shotgunActive)
                    {
                        if (holdingShotgunController)
                        {
                            lastShotgunTime += Time.deltaTime;
                        }
                        else
                        {
                            lastShotgunTime = shotgunIntervalBetweenShoots;
                            holdingShotgunController = true;
                        }


                        if (lastShotgunTime > shotgunIntervalBetweenShoots)
                        {
                            if (new Vector3(joystickChain.m_HorizontalVirtualAxis.GetValue, joystickChain.m_VerticalVirtualAxis.GetValue, 0.0f).magnitude > 0.5)
                            {
                                AngleRad = Mathf.Atan2(joystickChain.m_VerticalVirtualAxis.GetValue, joystickChain.m_HorizontalVirtualAxis.GetValue);
                                angle = (180 / Mathf.PI) * AngleRad;
                                rb.rotation = angle - 90;

                                for (int i = -2; i <= 2; i++)
                                {
                                    Vector3 newDirectionWithShift = new Vector3((float)Mathf.Cos((angle + (i * shotgunShrapAngle)) * (Mathf.PI / 180)), (float)Mathf.Sin((angle + (i * shotgunShrapAngle)) * (Mathf.PI / 180)));
                                    GameObject bullet = Instantiate(Resources.Load("Bullet"), this.transform.position + (new Vector3(joystickChain.m_HorizontalVirtualAxis.GetValue, joystickChain.m_VerticalVirtualAxis.GetValue, 0.0f).normalized * 0.5f), Quaternion.identity) as GameObject;
                                    bullet.GetComponent<Rigidbody2D>().velocity = newDirectionWithShift.normalized * speedBullet;
                                    bullet.GetComponent<Rigidbody2D>().rotation = angle + (i * shotgunShrapAngle);
                                }

                                lastShotgunTime = 0.0f;
                                shotgunAmmo -= 1;
                            }
                        }
                        if (shotgunAmmo < 0)
                        {
                            shotgunAmmo = 0;
                            shotgunActive = false;
                            lastShotgunTime = 0.0f;
                        }
                    }
                    else
                    {
                        if (chainManager.fingerReleased == true)
                        {
                            chainManager.breakChain = true;
                            chainManager.fingerReleased = false;
                        }

                        AngleRad = Mathf.Atan2(joystickChain.m_VerticalVirtualAxis.GetValue, joystickChain.m_HorizontalVirtualAxis.GetValue);
                        angle = (180 / Mathf.PI) * AngleRad;
                        rb.rotation = angle - 90;
                        if (new Vector3(joystickChain.m_HorizontalVirtualAxis.GetValue, joystickChain.m_VerticalVirtualAxis.GetValue, 0.0f).magnitude > 0.5)
                        {
                            if (!chainManager.isChainning)
                            {
                                chainManager.isChainning = true;
                                if (gameManager.GetComponent<GameManager>().isTripleChainning)
                                {
                                    chainManager.isChainning2 = true;
                                    chainManager.isChainning3 = true;
                                }
                                chainManager.chainStartPos = this.transform.position + (new Vector3(joystickChain.m_HorizontalVirtualAxis.GetValue, joystickChain.m_VerticalVirtualAxis.GetValue, 0.0f).normalized * 0.5f);
                                chainManager.chainDirection = new Vector3(joystickChain.m_HorizontalVirtualAxis.GetValue, joystickChain.m_VerticalVirtualAxis.GetValue, 0.0f);

                                if (!loggedChainRelease)
                                {
                                    gameManager.GetComponent<GameManager>().chainsReleased += 1;
                                    loggedChainRelease = true;
                                    Debug.Log("loggedChainRelease");
                                }
                            }
                            else if (loggedChainRelease)
                            {
                                loggedChainRelease = false;
                            }
                        }
                    }
                }
                else if (joystickChain.m_VerticalVirtualAxis.GetValue + joystickChain.m_HorizontalVirtualAxis.GetValue == 0)
                {
                    holdingShotgunController = false;
                    chainManager.fingerReleased = true;
                    flameThrowerParticles.SetActive(false);
                }
            }
            else
            {
                Vector3 keyboardMovement = Vector3.zero;
                bool keyboard = false;
                if (Input.GetKey(KeyCode.W))
                {
                    keyboardMovement = new Vector3(keyboardMovement.x, keyboardMovement.y + 1.0f, 0.0f);
                    keyboard = true;
                }
                if (Input.GetKey(KeyCode.A))
                {
                    keyboardMovement = new Vector3(keyboardMovement.x - 1.0f, keyboardMovement.y, 0.0f);
                    keyboard = true;
                }
                if (Input.GetKey(KeyCode.S))
                {
                    keyboardMovement = new Vector3(keyboardMovement.x, keyboardMovement.y - 1.0f, 0.0f);
                    keyboard = true;
                }
                if (Input.GetKey(KeyCode.D))
                {
                    keyboardMovement = new Vector3(keyboardMovement.x + 1.0f, keyboardMovement.y, 0.0f);
                    keyboard = true;
                }
                if (keyboard)
                {
                    keyboardMovement.Normalize();
                    this.GetComponent<Rigidbody2D>().velocity =
                        new Vector2(keyboardMovement.x, keyboardMovement.y) * speedMovement;
                    keyboard = false;
                }

                Vector3 mouseDir = mouseClickDirection = Vector3.Normalize(Camera.main.ScreenToWorldPoint(
                        new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10.0f)) - this.transform.position);
                AngleRad = Mathf.Atan2(mouseDir.y, mouseDir.x);
                angle = (180 / Mathf.PI) * AngleRad;
                rb.rotation = angle - 90;

                //Mouse click
                if (Input.GetMouseButton(0))
                {
                    mouseClick = true;
                    mouseClickDirection = Vector3.Normalize(Camera.main.ScreenToWorldPoint(
                        new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10.0f)) - this.transform.position);

                    if (gameManager.GetComponent<GameManager>().flameThrowerActivated)
                    {
                        AngleRad = Mathf.Atan2(mouseClickDirection.y, mouseClickDirection.x);
                        angle = (180 / Mathf.PI) * AngleRad;
                        rb.rotation = angle - 90;

                        if (gameManager.GetComponent<GameManager>().flameThrowerTime < 0)
                        {
                            gameManager.GetComponent<GameManager>().flameThrowerActivated = false;
                            flameThrowerParticles.SetActive(false);
                        }
                        else
                        {
                            flameThrowerParticles.SetActive(true);
                        }
                    }
                    else if (shotgunActive)
                    {
                        if (holdingShotgunController)
                        {
                            lastShotgunTime += Time.deltaTime;
                        }
                        else
                        {
                            lastShotgunTime = shotgunIntervalBetweenShoots;
                            holdingShotgunController = true;
                        }


                        if (lastShotgunTime > shotgunIntervalBetweenShoots)
                        {
                            if (new Vector3(mouseClickDirection.x, mouseClickDirection.y, 0.0f).magnitude > 0.5)
                            {
                                AngleRad = Mathf.Atan2(mouseClickDirection.y, mouseClickDirection.x);
                                angle = (180 / Mathf.PI) * AngleRad;
                                rb.rotation = angle - 90;

                                for (int i = -2; i <= 2; i++)
                                {
                                    Vector3 newDirectionWithShift = new Vector3((float)Mathf.Cos((angle + (i * shotgunShrapAngle)) * (Mathf.PI / 180)), (float)Mathf.Sin((angle + (i * shotgunShrapAngle)) * (Mathf.PI / 180)));
                                    GameObject bullet = Instantiate(Resources.Load("Bullet"), this.transform.position + (new Vector3(mouseClickDirection.x, mouseClickDirection.y, 0.0f).normalized * 0.5f), Quaternion.identity) as GameObject;
                                    bullet.GetComponent<Rigidbody2D>().velocity = newDirectionWithShift.normalized * speedBullet;
                                    bullet.GetComponent<Rigidbody2D>().rotation = angle + (i * shotgunShrapAngle);
                                }

                                lastShotgunTime = 0.0f;
                                shotgunAmmo -= 1;
                            }
                        }
                        if (shotgunAmmo < 0)
                        {
                            shotgunAmmo = 0;
                            shotgunActive = false;
                            lastShotgunTime = 0.0f;
                        }
                    }
                    else {
                        if (chainManager.fingerReleased == true)
                        {
                            chainManager.removeAllChains();

                            chainManager.breakChain = true;
                            chainManager.fingerReleased = false;
                        }

                        AngleRad = Mathf.Atan2(mouseClickDirection.y, mouseClickDirection.x);
                        angle = (180 / Mathf.PI) * AngleRad;
                        rb.rotation = angle - 90;

                        if (!chainManager.isChainning)
                        {
                            chainManager.isChainning = true;
                            if (gameManager.GetComponent<GameManager>().isTripleChainning)
                            {
                                chainManager.isChainning2 = true;
                                chainManager.isChainning3 = true;
                            }
                            chainManager.chainStartPos = this.transform.position + (new Vector3(mouseClickDirection.x, mouseClickDirection.y, 0.0f) * 0.5f);
                            chainManager.chainDirection = new Vector3(mouseClickDirection.x, mouseClickDirection.y, 0.0f);

                            if (!loggedChainRelease)
                            {
                                gameManager.GetComponent<GameManager>().chainsReleased += 1;
                                loggedChainRelease = true;
                                Debug.Log("loggedChainRelease");
                            }
                        }
                        else if (loggedChainRelease)
                        {
                            loggedChainRelease = false;
                        }
                    }
                }
                else
                {
                    holdingShotgunController = false;
                    chainManager.fingerReleased = true;
                    flameThrowerParticles.SetActive(false);
                }
            }

            speed = Vector3.Magnitude(rb.velocity);  // test current object speed
            if (speed > maximumSpeed)
            {
                brakeSpeed = speed - maximumSpeed;  // calculate the speed decrease

                normalisedVelocity = rb.velocity.normalized;
                brakeVelocity = normalisedVelocity * brakeSpeed;  // make the brake Vector3 value

                rb.AddForce(-brakeVelocity);  // apply opposing brake force
            }
        }
    }
}
