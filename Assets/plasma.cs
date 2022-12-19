using UnityEngine;

public class plasma : MonoBehaviour
{

    public GameObject explosion;
    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.collider.tag != "RPG")
        {
            GameObject effect = Instantiate(explosion, transform.position, Quaternion.identity);
            Destroy(effect, 0.5f);
            Destroy(gameObject);
        }
    }
}
