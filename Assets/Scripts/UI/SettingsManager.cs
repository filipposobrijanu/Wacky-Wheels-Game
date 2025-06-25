using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;


public class SettingsManager : MonoBehaviour
{
    public TMP_Dropdown dropdown;

    public AudioManager audioManager;

    public GameManager gameManager;

    public Slider masterSlider, musicSlider;

    public AudioMixer audioMixer;

    public Animator animator1;
    public Animator animator2;

    public Button shopButton;

    public Toggle fullscreenToggle;

    public void Start()
    {
        //PlayerPrefs.DeleteAll();
        LoadSavedSettings();
    }

    //load settings menu
    public void LoadSettings()
    {
        gameManager.infoAnimator.ResetTrigger("InfoButton");
        gameManager.infoAnimator.SetTrigger("InfoButton2");
        gameManager.optionMenuOn = true;
        audioManager.PlaySFX(audioManager.buttonclick1SFX);
        animator1.SetTrigger("MainMenu1Manager");
        animator1.SetTrigger("FromPauseToSettings");
        animator2.SetTrigger("SettingsMenuManager");
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            shopButton.interactable = false;

        }
    }
    //unload settings menu
    public void UnLoadSettings()
    {
        gameManager.infoAnimator.SetTrigger("InfoButton");
        gameManager.infoAnimator.ResetTrigger("InfoButton2");
        gameManager.optionMenuOn = false;
        audioManager.PlaySFX(audioManager.buttonclick1SFX);
        animator1.SetTrigger("MainMenu2Manager");
        animator1.SetTrigger("FromPauseToSettingsReverse");
        animator2.SetTrigger("SettingsMenuManager2");
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            shopButton.interactable = true;

        }

    }
    // load settings
    public void LoadSavedSettings()
    {


        if (PlayerPrefs.HasKey("qualitytext"))
        {
            QualitySettings.SetQualityLevel(PlayerPrefs.GetInt("quality"));
            dropdown.value = PlayerPrefs.GetInt("qualitytext");

        }
        else
        {
            ChangeGraphicsQuality();
        }

        if (PlayerPrefs.HasKey("mastervolume")) {

            LoadVolume();
        }
        else
        {
            ChangeMasterVolume();
        }

        if (PlayerPrefs.HasKey("musicvolume"))
        {

            LoadMusic();
        }
        else
        {
            ChangeMusicVolume();
        }
        if (PlayerPrefs.HasKey("fullscreenvalue"))
        {
            if (PlayerPrefs.GetInt("fullscreenvalue") == 1)
            {
                Screen.fullScreen = true;
                fullscreenToggle.isOn = true;
            }
            else
            {
                fullscreenToggle.isOn = false;
                Screen.fullScreen = false;

            }
        }
        else
        {
            SetFullscreen(true);

        }


    }
    //change graphics
    public void ChangeGraphicsQuality()
    {

        QualitySettings.SetQualityLevel(dropdown.value);
        PlayerPrefs.SetInt("quality", dropdown.value);
        PlayerPrefs.SetInt("qualitytext", dropdown.value);
        dropdown.value = PlayerPrefs.GetInt("qualitytext");
        audioManager.PlaySFX(audioManager.buttonclick1SFX);
    }

    //change master volume
    public void ChangeMasterVolume()
    {

        float volume1 = masterSlider.value;
        audioMixer.SetFloat("MasterParam", volume1);
        PlayerPrefs.SetFloat("mastervolume", volume1);

    }
    public void LoadVolume()
    {
        masterSlider.value = PlayerPrefs.GetFloat("mastervolume");

        ChangeMasterVolume();

    }
    //change music volume
    public void ChangeMusicVolume()
    {

        float volume1 = musicSlider.value;
        audioMixer.SetFloat("MusicParam", volume1);
        PlayerPrefs.SetFloat("musicvolume", volume1);

    }
    public void LoadMusic()
    {
        musicSlider.value = PlayerPrefs.GetFloat("musicvolume");

        ChangeMusicVolume();

    }
    //change fullscreen
    public void SetFullscreen(bool isFullscreen)
    {
        if(isFullscreen == true)
        {
            PlayerPrefs.SetInt("fullscreenvalue", 1);

        }
        else
        {
            PlayerPrefs.SetInt("fullscreenvalue", 0);
        }
        audioManager.PlaySFX(audioManager.buttonclick1SFX);
        if(PlayerPrefs.GetInt("fullscreenvalue") == 1) { 

            Screen.fullScreen = true;
        }
        else
        {
            Screen.fullScreen = false;

        }

    }

}
