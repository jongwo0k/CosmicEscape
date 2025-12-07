using UnityEngine;

public class FireWallDamage : MonoBehaviour
{
    public float fireWallDamage = 5f;   // 초기 데미지
    public float damageInterval = 0.3f; // 도트 데미지

    private float lastHitTime;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var player = other.GetComponent<PlayerMove>();
            player.TakeDamage(fireWallDamage);
            lastHitTime = Time.time;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var player = other.GetComponent<PlayerMove>();
            if (Time.time - lastHitTime >= damageInterval)
            {
                player.TakeDamage(fireWallDamage);
                lastHitTime = Time.time;
            }
        }
    }
}
