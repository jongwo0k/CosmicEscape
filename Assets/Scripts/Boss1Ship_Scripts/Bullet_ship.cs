using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_ship : MonoBehaviour
{

    public float shipDamage = 1.0f;
    public float speed;

    void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    /*
    public void OnTriggerEnter(Collider other)
    {
        // Bullet, Boss 자신 등과는 충돌X -> Layer or Tag
        if (other.CompareTag("Bullet_ship"))
        {
            return;
        }

        if (other.CompareTag("Player"))
        {
            other.gameObject.TryGetComponent<PlayerMove>(out var player);
            player.TakeDamage(shipDamage);
            Destroy(gameObject);
        }
    }
    */
}
