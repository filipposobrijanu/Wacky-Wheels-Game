using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TipsManager : MonoBehaviour
{
    public Animator animator1,animator2;

    public AudioManager audioManager;

    public GameManager gameManager;


    public void ShowTips()
    {
        animator1.SetTrigger("SettingsMenuManager");
        animator1.ResetTrigger("SettingsMenuManager2");
        animator2.SetTrigger("FromPauseToSettings");
        gameManager.tipMenuOn = true;
        audioManager.PlaySFX(audioManager.buttonclick1SFX);

    }
    public void UnShowTips()
    {
        gameManager.tipMenuOn = false;
        animator1.ResetTrigger("SettingsMenuManager");
        animator1.SetTrigger("SettingsMenuManager2");
        animator2.SetTrigger("FromPauseToSettingsReverse");
        audioManager.PlaySFX(audioManager.buttonclick1SFX);

    }
}
