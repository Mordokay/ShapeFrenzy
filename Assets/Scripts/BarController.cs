using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BarController : MonoBehaviour
{

    public float barDisplay;
    public Image frontImage;

    public string barText;
    public Text comboText;
    

    void FixedUpdate() {
        frontImage.fillAmount = barDisplay;
        comboText.text = barText;
    }
}
