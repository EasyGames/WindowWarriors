using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour {

    public AudioClip firstTrack;
    public AudioClip secondTrack;
    public AudioSource audioSource;
    bool playMusic = true;
    bool muteMusic = true;
    bool colorToggle = false;
    Vector3 position;
    SpriteRenderer soundIcon;

	void Start () {

        soundIcon = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = firstTrack;
        audioSource.Play();
        position = new Vector3(Screen.width - Screen.width / 18, Screen.height - Screen.height / 18, 30);
        position = Camera.main.ScreenToWorldPoint(position);
        transform.position = position;

    }
	
    void OnMouseUp()
    {
        muteMusic = !muteMusic;

        if (colorToggle)
        {
            soundIcon.color = Color.red;
            colorToggle = false;
        }
        else
        {
            soundIcon.color = Color.white;
            colorToggle = true;
        }
    }

	// Update is called once per frame
	void FixedUpdate () {

        if (!audioSource.isPlaying)
        {
            audioSource.clip = secondTrack;
            audioSource.Play();
        }

        if (playMusic && !muteMusic)
        {
            audioSource.volume = 1.0f;
        }
        else
        {
            audioSource.volume = 0.0f;
        }
	
	}

    void OnApplicationFocus(bool focusStatus)
    {
        playMusic = focusStatus;
    }
}
