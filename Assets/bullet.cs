using UnityEngine;

public class bullet : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D col)
    {
        Destroy(gameObject);
    }
}
