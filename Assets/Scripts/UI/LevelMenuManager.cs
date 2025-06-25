using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelMenuManager : MonoBehaviour
{
    public AudioManager audioManager;

    public MainMenuManager mainMenuManager;

    public Button[] buttons;

    public GameManager gameManager;
    public Animator animator1;
    public Animator animator2;

    public Button shopButton;

    public GameObject noteText;

    private void Awake()
    {
        //PlayerPrefs.DeleteAll();
        int unlockedLevel = PlayerPrefs.GetInt("UnlockedLevel", 1);
        for(int i = 0; i < buttons.Length; i++)
        {

            buttons[i].interactable = false;

        }
        for (int i = 0; i < unlockedLevel; i++)
        {
            buttons[i].interactable = true;
        }
    }
    public void OpenLevel()
    {
        mainMenuManager.loadFirstLevel();
        Invoke("MainMenuTransition", 1.2f);
        
       
    }
    public void MainMenuTransition()
    {
        SceneManager.LoadScene(EventSystem.current.currentSelectedGameObject.name);

    }

    public void LoadLevelMenu()
    {
        noteText.active = true;
        audioManager.PlaySFX(audioManager.buttonclick1SFX);
        animator1.SetTrigger("MainMenu1Manager");
        animator2.SetTrigger("SettingsMenuManager");
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            shopButton.interactable = false;

        }

    }
    public void UnLoadLevelMenu()
    {
        noteText.active = false;
        audioManager.PlaySFX(audioManager.buttonclick1SFX);
        animator1.SetTrigger("MainMenu2Manager");
        animator2.SetTrigger("SettingsMenuManager2");
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            shopButton.interactable = true;

        }
        

    }
}
