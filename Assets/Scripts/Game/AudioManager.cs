using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] public AudioSource musicSource;
    [SerializeField] public AudioSource SFXSource;
    [SerializeField] public AudioSource CarSource;

    public AudioClip background;
    public AudioClip wallHit;
    public AudioClip carSFX;
    public AudioClip winSFX;
    public AudioClip coinCollect;
    public AudioClip buttonclick1SFX;
    public AudioClip buttonclick2SFX;

    private void Start()
    {
        //PlayerPrefs.DeleteAll();
        // play bg music + car music
        musicSource.clip = background;
        musicSource.Play();
        CarSource.clip = carSFX;
        
    }
    public void PlaySFX(AudioClip clip)
    {
        // play a sfx one time
        SFXSource.PlayOneShot(clip);

    }

}
