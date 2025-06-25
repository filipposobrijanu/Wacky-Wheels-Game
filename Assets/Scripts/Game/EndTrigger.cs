using UnityEngine;
using UnityEngine.SceneManagement;


public class EndTrigger : MonoBehaviour
{
    public AudioManager audioManager;

    public PlayerMovement playerMovement;

    public Animator zoomCamera;

    public GameManager gameManager;

    public bool gameHasEnded = false;

    private void WinLevel()
    {
        // end of level play sfx + anim
        audioManager.PlaySFX(audioManager.winSFX);
        playerMovement.enabled = false;
        audioManager.CarSource.Pause();
        gameManager.CompleteLevel();
        zoomCamera.SetTrigger("WinCamera");
        gameHasEnded = true;
        gameManager.particlesSmoke1.enableEmission = false;
        gameManager.particlesSmoke2.enableEmission = false;
        UnlockNewLevel();
    }
    void UnlockNewLevel()
    {
        if(SceneManager.GetActiveScene().buildIndex >= PlayerPrefs.GetInt("ReachedIndex"))
        {

            PlayerPrefs.SetInt("ReachedIndex", SceneManager.GetActiveScene().buildIndex + 1);
            PlayerPrefs.SetInt("UnlockedLevel", PlayerPrefs.GetInt("ReachedIndex",1));
            PlayerPrefs.Save();
        }

    }
    private void OnTriggerEnter(Collider other)
    {
        Invoke("WinLevel", 0);

    }


}
