using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class WaveBarController : MonoBehaviour
{

    public float waveBarInitialValue;
    public Image frontImage;
    public Image backImage;

    public Text comboText;
    public float lifeTime;

    void Start() {
        lifeTime = 0.0f;
    }

    public void disable() {
        frontImage.enabled = false;
        backImage.enabled = false;
        comboText.enabled = false;
    }

    public void enable() {
        frontImage.enabled = true;
        backImage.enabled = true;
        comboText.enabled = true;
    }

    void FixedUpdate()
    {
        lifeTime += Time.deltaTime;
        frontImage.fillAmount = (waveBarInitialValue - lifeTime) / waveBarInitialValue;
    }
}
