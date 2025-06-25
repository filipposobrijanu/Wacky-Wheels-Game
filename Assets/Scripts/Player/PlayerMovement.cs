using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public Rigidbody rb;

    public float forwardForce = 2000f; // player forward force variable
    public float sideForce = 500f; // player side force variable

    // FixedUpdate giati xrhsimopoioume physics
    void FixedUpdate()
    {
        rb.AddForce(0, 0, forwardForce * Time.deltaTime); // move player forward

        if (Input.GetKey("d"))
        {
            rb.AddForce(sideForce * Time.deltaTime, 0, 0, ForceMode.VelocityChange);    // move player left

        };
        if (Input.GetKey("a"))
        {
            rb.AddForce(-sideForce * Time.deltaTime, 0, 0, ForceMode.VelocityChange);    // move player left

        };
        // endgame an pesei apo to map

        if (rb.position.y < -1f)
        {

            FindObjectOfType<GameManager>().EndGame();
        }

    }
}
