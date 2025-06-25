using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    bool gameHasEnded = false;

    public int LevelSelect;

    public Animator timerCountAnim;

    [SerializeField] private Animator mainTrans;

    public PlayerMovement playerMovement;
    public PlayerCollision playerCollision;

    public float restartDelay = 1f;
    public float delayTrans = 1.1f;

    [SerializeField] private Animator pauseMenuAnimator;

    public GameObject completeLevelUI;

    public AudioManager audioManager;

    public StressReceiver stressReceiver;

    public ParticleSystem particlesSmoke1;
    public ParticleSystem particlesSmoke2;
    public ParticleSystem crashParticles;

    public bool optionMenuOn = false;

    public bool tipMenuOn = false;

    public bool endMenuOn = false;

    public Button optionsButton, resButton, xButton;

    private bool checkCarSFXStartPause = false;

    public float cooldownEscButton = 0;

    float prevSpeed;

    public ParticleSystem speedLines;

    private bool escapeButtonBool = false;

    public Animator infoAnimator;

    // open url to my twitter
    public void OpenX()
    {
        Application.OpenURL("https://x.com/MonkassDev");

    }
    // open url to my yt
    public void OpenYT()
    {
        Application.OpenURL("https://www.youtube.com/channel/UCNhFUWHoaB7TqYn_QCgCDUQ");

    }
    void Start()
    {
        endMenuOn = false;
        crashParticles.Stop();
        audioManager.CarSource.Pause();
        playerMovement.enabled = false;
        speedLines.enableEmission = false;
        particlesSmoke1.enableEmission = false;
        particlesSmoke2.enableEmission = false;

        Invoke("ChangeTimeAfterCountdown", 2.5f);
    }
    // load next level
    public void LoadNextLevel()
    {

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        audioManager.PlaySFX(audioManager.buttonclick1SFX);


    }
    // game quit
    public void Quit()
    {
        Application.Quit();
        audioManager.PlaySFX(audioManager.buttonclick1SFX);

    }
    // load main menu
    public void LoadMainMenu()
    {

        optionsButton.interactable = false;
        xButton.interactable = false;
        resButton.interactable = false;
        mainTrans.SetTrigger("MainMenuTransition");
        audioManager.PlaySFX(audioManager.buttonclick1SFX);
        playerMovement.enabled = false;
        playerCollision.enabled = false;
        speedLines.enableEmission = false;
        particlesSmoke1.enableEmission = false;
        particlesSmoke2.enableEmission = false;
        audioManager.CarSource.Pause();
        Time.timeScale = 1;
        Invoke(nameof(MainMenuTransition1), delayTrans);

    }
    // transition with logo
    public void MainMenuTransition1()
    {
        
        SceneManager.LoadScene(0);

    }
    // after starting countdown shii
    void ChangeTimeAfterCountdown()
    {
        speedLines.enableEmission = true;
        playerMovement.enabled = true;
        particlesSmoke1.enableEmission = true;
        particlesSmoke2.enableEmission = true;
        audioManager.CarSource.Play();
        checkCarSFXStartPause = true;

    }

    private void Update()
    {

        // escape key things
        if (Input.GetKey(KeyCode.Escape) & optionMenuOn == false & tipMenuOn == false & endMenuOn == false)
        {
            EscapeButton();

        };

        cooldownEscButton -= Time.unscaledDeltaTime;

    }

    public void EscapeButton()
    {
        if (cooldownEscButton <= 0)
        {
            cooldownEscButton = 0.25f;
            if (escapeButtonBool == false)
            {
                Pause();
            }
            else
            {
                UnPause();
            }
        }

    }
    public void CompleteLevel()
    {
        completeLevelUI.SetActive(true);
        endMenuOn = true;

    }
    // end go to restart
    public void EndGame()
    {
        if(gameHasEnded == false)
        {
            gameHasEnded = true;
            Invoke("Restart", restartDelay);

        }

    }
    // restart scene
    void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }
    // pause game to pause menu
    public void Pause()
    {
        escapeButtonBool = true;
        prevSpeed = timerCountAnim.speed;
        timerCountAnim.speed = 0;
        pauseMenuAnimator.SetTrigger("PauseMenu");
        infoAnimator.SetTrigger("InfoButton");
        infoAnimator.ResetTrigger("InfoButton2");
        pauseMenuAnimator.ResetTrigger("UnPauseMenu");
        Time.timeScale = 0;
        audioManager.PlaySFX(audioManager.buttonclick1SFX);
        audioManager.CarSource.Pause();
        speedLines.enableEmission = false;

    }

    // pause menu to pause game
    public void UnPause()
    {
        escapeButtonBool = false;
        timerCountAnim.speed = prevSpeed;
        Time.timeScale = 1;
        audioManager.PlaySFX(audioManager.buttonclick1SFX);
        pauseMenuAnimator.ResetTrigger("PauseMenu");
        infoAnimator.ResetTrigger("InfoButton");
        infoAnimator.SetTrigger("InfoButton2");
        pauseMenuAnimator.SetTrigger("UnPauseMenu");
        if(checkCarSFXStartPause == true) {
            audioManager.CarSource.Play();
            speedLines.enableEmission = true;
        }
    }


}
