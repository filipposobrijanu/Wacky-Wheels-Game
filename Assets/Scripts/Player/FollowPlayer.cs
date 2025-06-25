using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Transform player;
    public Vector3 offset;
    void Update()
    {
        // camera follow player offset
        transform.position = player.position + offset;
    }
}
