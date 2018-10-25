using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AnnouncementController : MonoBehaviour {

    float startTime;
    public float animationTime;
    public float startFadeTime;
    public GameObject Announcement;
    public TextMesh AnnouncementText;

    public string textInfo;

    public void Start () {
        startTime = Time.time;
        animationTime = 3.0f;
        startFadeTime = 2.0f;
        Announcement.SetActive(true);
        AnnouncementText.text = textInfo;
        Announcement.transform.localScale = new Vector3(0.0f, 0.0f, 0.0f);
    }

	void FixedUpdate() {
        if ((Time.time - startTime) / animationTime > 1)
        {
            this.enabled = false;
            Announcement.SetActive(false);
        }
        else
        {
            float x = (Time.time - startTime) / animationTime;
            Announcement.transform.localScale = new Vector3(x, x, x);
        }
    }
}
