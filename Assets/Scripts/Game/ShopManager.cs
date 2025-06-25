using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopManager : MonoBehaviour
{
    public AudioManager audioManager;

    public MainMenuManager mainMenuManager;

    public GameObject[] ticks;

    public GameManager gameManager;
    public Animator animator1;
    public Animator animator2;
    public Animator animator3;

    public Button[] buttons;

    int coins = 0;

    public TextMeshProUGUI coinText;
    public TextMeshProUGUI gemText;

    int gems = 0;

    int coin;

    string coinCollected = "000000000000";

    public static int IntParseFast(string value)
    {
        int result = 0;
        for (int i = 0; i < value.Length; i++)
        {
            char letter = value[i];
            result = 10 * result + (letter - 48);
        }
        return result;

    }
    private void Start()
    {
        if (!PlayerPrefs.HasKey("title"))
        {
            PlayerPrefs.SetString("title", coinCollected);

        }
        if (!PlayerPrefs.HasKey("playerskin"))
        {
            PlayerPrefs.SetInt("playerskin", 0);

        }
        for (int i = 0; i < 4; i++)
        {
            if (PlayerPrefs.GetInt("playerskin") == i)
            {
                ticks[i].active = true;

            }
            else
            {
                ticks[i].active = false;

            }

        }
        string ee = PlayerPrefs.GetString("title");
        
        for(int i = 0; i < 11; i++)
        {
            if (i!=3 & i != 7 & i != 10) { 
                coin = IntParseFast(ee[i].ToString());
                coins += coin;
            }
            else{

                coin = IntParseFast(ee[i].ToString());
                gems += coin;
            }
        }

        coinText.text = coins.ToString();
        gemText.text = gems.ToString();

        if (coins >= 3)
        {
            buttons[1].interactable = true;
        }
        else
        {
            buttons[1].interactable = false;

        }
        if (coins >= 6)
        {
            buttons[2].interactable = true;
        }
        else
        {
            buttons[2].interactable = false;

        }
        if (gems >= 3)
        {
            buttons[3].interactable = true;
        }
        else
        {
            buttons[3].interactable = false;

        }
    }

    public void SkinSelection(int id)
    {
        PlayerPrefs.SetInt("playerskin", id);
        audioManager.PlaySFX(audioManager.buttonclick2SFX);
        for (int i = 0; i < 4; i++)
        {
            if (id == i)
            {
                ticks[i].active = true;

            }
            else
            {
                ticks[i].active = false;

            }

        }
    }
    public void LoadShopMenu()
    {
        audioManager.PlaySFX(audioManager.buttonclick1SFX);
        animator1.SetTrigger("MainMenu1Manager");
        animator2.SetTrigger("SettingsMenuManager");
        animator3.SetTrigger("CoinSPanel");

    }
    public void UnLoadShopMenu()
    {
        audioManager.PlaySFX(audioManager.buttonclick1SFX);
        animator1.SetTrigger("MainMenu2Manager");
        animator2.SetTrigger("SettingsMenuManager2");
        animator3.SetTrigger("CoinSPanel1");


    }
}
