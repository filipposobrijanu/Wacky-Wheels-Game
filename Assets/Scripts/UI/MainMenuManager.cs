using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{

    [SerializeField] private Animator mainTrans;

    public AudioManager audioManager;

    public GameManager gameManager;

    public Button optionsButton,exitButton;

    // load first level;
    public void loadFirstLevel()
    {
        mainTrans.SetTrigger("MainMenuTransition");
        optionsButton.interactable = false;
        exitButton.interactable = false;
        audioManager.PlaySFX(audioManager.buttonclick1SFX);
        

    }
}
