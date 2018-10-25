using UnityEngine;
using System.Collections;

public class TouchManager : MonoBehaviour {

    Touch myTouch1;
    Touch myTouch2;
    public PlayerMovement playerMovement;

    ChainManager chainManager;
    Vector3 touchAuxPos;

    public GameObject mobileJoystick;
    public GameObject mobileJoystickBackround;
    public GameObject mobileChainJoystick;
    public GameObject mobileChainJoystickBackround;

    public Vector3 mobileJoystickOriginalPos;
    public Vector3 mobileJoystickBackroundOriginalPos;
    public Vector3 mobileChainJoystickOriginalPos;
    public Vector3 mobileChainJoystickBackroundOriginalPos;

    public bool freeTouchMovement;

    void Start () {
        chainManager = this.GetComponent<ChainManager>();

        mobileJoystickOriginalPos = mobileJoystick.transform.position;
        mobileJoystickBackroundOriginalPos = mobileJoystickBackround.transform.position;
        mobileChainJoystickOriginalPos = mobileChainJoystick.transform.position;
        mobileChainJoystickBackroundOriginalPos = mobileChainJoystickBackround.transform.position;

        if (freeTouchMovement)
        {
            mobileJoystick.SetActive(false);
            mobileJoystickBackround.SetActive(false);
            mobileChainJoystick.SetActive(false);
            mobileChainJoystickBackround.SetActive(false);
        }
        else
        {
            mobileJoystick.SetActive(true);
            mobileJoystickBackround.SetActive(true);
            mobileChainJoystick.SetActive(true);
            mobileChainJoystickBackround.SetActive(true);
        }
    }

    void Update()
    {
        if (Input.touchCount == 0) {
            mobileJoystick.SetActive(false);
            mobileJoystickBackround.SetActive(false);

            mobileJoystick.GetComponent<Joystick>().resetPos();

            //chainManager.fingerReleased = true;
            mobileChainJoystick.SetActive(false);
            mobileChainJoystickBackround.SetActive(false);

            mobileChainJoystick.GetComponent<Joystick>().resetPos();
        }

        if (Input.touchCount > 0)
        {
            myTouch1 = Input.GetTouch(0);
            if (myTouch1.phase == TouchPhase.Began)
            {
                touchAuxPos = Camera.main.ScreenToWorldPoint(new Vector3(myTouch1.position.x, myTouch1.position.y, -Camera.main.transform.position.z));
                touchAuxPos.z = -0.001f;
                if (freeTouchMovement)
                {
                    if (isTouchingLeft(myTouch1.position.x / Screen.width))
                    {
                        mobileJoystick.SetActive(true);
                        mobileJoystick.transform.position = myTouch1.position;
                        mobileJoystickBackround.SetActive(true);
                        mobileJoystickBackround.transform.position = myTouch1.position;

                        mobileJoystick.GetComponent<Joystick>().setNewPos(myTouch1.position);
                        mobileJoystick.GetComponent<Joystick>().Start();
                    }
                    else
                    {
                        mobileChainJoystick.SetActive(true);
                        mobileChainJoystick.transform.position = myTouch1.position;
                        mobileChainJoystickBackround.SetActive(true);
                        mobileChainJoystickBackround.transform.position = myTouch1.position;

                        mobileChainJoystick.GetComponent<Joystick>().setNewPos(myTouch1.position);
                        mobileChainJoystick.GetComponent<Joystick>().Start();
                    }
                }
            }
            else if (myTouch1.phase == TouchPhase.Moved)
            {
                touchAuxPos = Camera.main.ScreenToWorldPoint(new Vector3(myTouch1.position.x, myTouch1.position.y, -Camera.main.transform.position.z));
                touchAuxPos.z = -0.001f;

                if (freeTouchMovement)
                {
                    if (isTouchingLeft(myTouch1.position.x / Screen.width))
                    {
                        mobileJoystick.GetComponent<Joystick>().setNewPos(myTouch1.position);
                    }
                    else {
                        if (chainManager.fingerReleased == true)
                        {
                            chainManager.removeAllChains();
                            chainManager.fingerReleased = false;
                        }
                        mobileChainJoystick.GetComponent<Joystick>().setNewPos(myTouch1.position);
                    }
                }
            }
            else if(myTouch1.phase == TouchPhase.Ended)
            {
                if (freeTouchMovement)
                {
                    if (isTouchingLeft(myTouch1.position.x / Screen.width))
                    {
                        mobileJoystick.SetActive(false);
                        mobileJoystickBackround.SetActive(false);

                        mobileJoystick.GetComponent<Joystick>().resetPos();
                    }
                    else
                    {
                        chainManager.fingerReleased = true;
                        mobileChainJoystick.SetActive(false);
                        mobileChainJoystickBackround.SetActive(false);

                        mobileChainJoystick.GetComponent<Joystick>().resetPos();
                    }
                }
            }
        }

        if (Input.touchCount > 1)
        {
            myTouch2 = Input.GetTouch(1);
            
            if (myTouch2.phase == TouchPhase.Began)
            {
                
                touchAuxPos = Camera.main.ScreenToWorldPoint(new Vector3(myTouch2.position.x, myTouch2.position.y, -Camera.main.transform.position.z));
                touchAuxPos.z = -0.001f;
                if (freeTouchMovement)
                {
                    if (isTouchingLeft(myTouch2.position.x / Screen.width))
                    {
                        mobileJoystick.SetActive(true);
                        mobileJoystick.transform.position = myTouch2.position;
                        mobileJoystickBackround.SetActive(true);
                        mobileJoystickBackround.transform.position = myTouch2.position;

                        mobileJoystick.GetComponent<Joystick>().setNewPos(myTouch2.position);
                        mobileJoystick.GetComponent<Joystick>().Start();
                    }
                    else
                    {
                        mobileChainJoystick.SetActive(true);
                        mobileChainJoystick.transform.position = myTouch2.position;
                        mobileChainJoystickBackround.SetActive(true);
                        mobileChainJoystickBackround.transform.position = myTouch2.position;

                        mobileChainJoystick.GetComponent<Joystick>().setNewPos(myTouch2.position);
                        mobileChainJoystick.GetComponent<Joystick>().Start();
                    }
                }
            }
            else if (myTouch2.phase == TouchPhase.Moved)
            {
                touchAuxPos = Camera.main.ScreenToWorldPoint(new Vector3(myTouch2.position.x, myTouch2.position.y, -Camera.main.transform.position.z));
                touchAuxPos.z = -0.001f;
                if (freeTouchMovement)
                {
                    if (isTouchingLeft(myTouch2.position.x / Screen.width))
                    {
                        mobileJoystick.GetComponent<Joystick>().setNewPos(myTouch2.position);
                    }
                    else
                    {
                        if (chainManager.fingerReleased == true)
                        {
                            chainManager.removeAllChains();
                            chainManager.fingerReleased = false;
                        }
                        mobileChainJoystick.GetComponent<Joystick>().setNewPos(myTouch2.position);
                    }
                }
            }
            else if (myTouch2.phase == TouchPhase.Ended)
            {
                if (freeTouchMovement)
                {
                    if (isTouchingLeft(myTouch2.position.x / Screen.width))
                    {
                        mobileJoystick.SetActive(false);
                        mobileJoystickBackround.SetActive(false);

                        mobileJoystick.GetComponent<Joystick>().resetPos();
                    }
                    else
                    {
                        chainManager.fingerReleased = true;
                        mobileChainJoystick.SetActive(false);
                        mobileChainJoystickBackround.SetActive(false);

                        mobileChainJoystick.GetComponent<Joystick>().resetPos();
                    }
                }
            }
        }
    }

    bool isTouchingLeft(float x) {
        return x <=0.5;
    }
}
