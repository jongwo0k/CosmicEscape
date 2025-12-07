using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;
    public float damage = 1.0f;

    void Start()
    {
        Destroy(gameObject, 2.2f);
    }

    void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    /*
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Bullet")
        {
            other.gameObject.TryGetComponent<Boss>(out var boss);
            boss.TakeDamage(damage);
            Destroy(gameObject);
        }
        else if (other.gameObject.tag == "Ground")
        {
            Destroy(gameObject);
        }
    }
    */
}
