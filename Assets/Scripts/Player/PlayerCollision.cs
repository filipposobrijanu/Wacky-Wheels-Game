using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    public PlayerCollision playerCollision;

    [SerializeField] private Animator coinAnimator;

    [SerializeField] private Animator coinAnimatorUI;

    public PlayerMovement playerMovement;

    public bool skullBool = false;

    public AudioManager audioManager;

    public GameObject coin;

    public CoinCollectingScore coinScript;

    public SkullCollectingScore skullScript;

    public StressReceiver shakeCollide;

    public EndTrigger endTrigger;

    public bool checkRotation = false;

    public ParticleSystem particlesSmoke1;
    public ParticleSystem particlesSmoke2;
    public ParticleSystem speedLines;
    public ParticleSystem crashParticles;

    public GameObject player;
    void FixedUpdate()
    {
        // 90 degree explosion
        if (player.transform.rotation.eulerAngles.z < -85)
        {

            if (checkRotation == false)
            {
                endTrigger.gameHasEnded = true;
                skullScript.UpCoins(skullBool);
                skullBool = true;
                checkRotation = true;
                crashParticles.Play();
                speedLines.enableEmission = false;
                audioManager.PlaySFX(audioManager.wallHit);
                particlesSmoke1.enableEmission = false;
                particlesSmoke2.enableEmission = false;

                playerMovement.enabled = false;
                FindObjectOfType<GameManager>().EndGame();
                audioManager.CarSource.Pause();
                shakeCollide.InduceStress(0.4f);
                playerCollision.enabled = false;
            }
        }
        // 90 degree explosion
        if (player.transform.rotation.eulerAngles.z > 85)
        {
            if (player.transform.rotation.eulerAngles.z < 105)
            {
                if (checkRotation == false)
                {
                    endTrigger.gameHasEnded = true;
                    skullScript.UpCoins(skullBool);
                    skullBool = true;
                    checkRotation = true;
                    crashParticles.Play();
                    speedLines.enableEmission = false;
                    audioManager.PlaySFX(audioManager.wallHit);
                    particlesSmoke1.enableEmission = false;
                    particlesSmoke2.enableEmission = false;

                    playerMovement.enabled = false;
                    FindObjectOfType<GameManager>().EndGame();
                    audioManager.CarSource.Pause();
                    shakeCollide.InduceStress(0.4f);
                    playerCollision.enabled = false;
                }
            }
        }

    }
    // collision with obstacle
    void OnCollisionEnter(Collision collisionInfo)
    {
        if (collisionInfo.collider.tag == "Block")
        {
            if(endTrigger.gameHasEnded == false) {

                endTrigger.gameHasEnded = true;
                skullScript.UpCoins(skullBool);
                skullBool = true;
                crashParticles.Play();
                speedLines.enableEmission = false;
                audioManager.PlaySFX(audioManager.wallHit);
                particlesSmoke1.enableEmission = false;
                particlesSmoke2.enableEmission = false;

                playerMovement.enabled = false;
                FindObjectOfType<GameManager>().EndGame();
                audioManager.CarSource.Pause();
                shakeCollide.InduceStress(0.4f);
                playerCollision.enabled = false;
            }

        }
 
    }
    // collision with coin
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Coin")
        {
            audioManager.PlaySFX(audioManager.coinCollect);
            coinAnimator.SetTrigger("coin");
            coinAnimatorUI.SetTrigger("CoinUp");
            coinScript.UpCoins();
            //coin.SetActive(false);
            playerCollision.enabled = false;


        }
        // collision with sides
        if (collision.gameObject.tag == "BlockSide")
        {
            if (endTrigger.gameHasEnded == false)
            {
                endTrigger.gameHasEnded = true;
                skullScript.UpCoins(skullBool);
                skullBool = true;
                crashParticles.Play();
                audioManager.PlaySFX(audioManager.wallHit);
                speedLines.enableEmission = false;
                particlesSmoke1.enableEmission = false;
                particlesSmoke2.enableEmission = false;

                playerMovement.enabled = false;
                FindObjectOfType<GameManager>().EndGame();
                audioManager.CarSource.Pause();
                shakeCollide.InduceStress(0.4f);
                playerCollision.enabled = false;
            }
        }

    }
}
