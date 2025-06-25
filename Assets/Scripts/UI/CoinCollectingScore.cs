using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CoinCollectingScore : MonoBehaviour
{
    public TextMeshProUGUI scoreText;

    public int CoinstoCollect;

    public GameManager gameManager;

    public GameObject coinObject;

    string coinsCollected;

    string coinCollected = "000000000000";

    // start set coin ui
    private void Start()
    {
        //PlayerPrefs.DeleteAll();


        if (!PlayerPrefs.HasKey("title"))
        {
            PlayerPrefs.SetString("title", coinCollected);

        }

        string ee = PlayerPrefs.GetString("title");

        coinsCollected = ee[gameManager.LevelSelect].ToString();

        if (coinsCollected == "1")
        {
            coinObject.active = false;

        }

        scoreText.text = coinsCollected + "/" + CoinstoCollect;

    }
    // update coin ui when coin is collected
    public void UpCoins()
    {

        string ee = PlayerPrefs.GetString("title");


        coinsCollected = ee[gameManager.LevelSelect].ToString();


        if (coinsCollected == "0")
        {
            ee = ee.Remove(gameManager.LevelSelect, 1).Insert(gameManager.LevelSelect, "1");

            PlayerPrefs.SetString("title", ee);

            ee = PlayerPrefs.GetString("title");

            coinsCollected = ee[gameManager.LevelSelect].ToString();

            scoreText.text = coinsCollected + "/" + CoinstoCollect;

        }

        PlayerPrefs.Save();

    }

}
