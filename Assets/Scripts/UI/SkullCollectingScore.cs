using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkullCollectingScore : MonoBehaviour
{
    public TextMeshProUGUI scoreText;


    // start set death ui
    private void Start() {


        //PlayerPrefs.SetInt("deaths", 0);

        if (!PlayerPrefs.HasKey("deaths"))
        {
            PlayerPrefs.SetInt("deaths", 0);

        }

        scoreText.text = PlayerPrefs.GetInt("deaths").ToString();

    }
    // update death ui when coin is collected
    public void UpCoins(bool skullBool)
    {
        if (!skullBool) { 

            PlayerPrefs.SetInt("deaths", PlayerPrefs.GetInt("deaths") + 1);
            scoreText.text = PlayerPrefs.GetInt("deaths").ToString();
        }
        PlayerPrefs.Save();

    }


}
