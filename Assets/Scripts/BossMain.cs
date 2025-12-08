using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public abstract class Boss : MonoBehaviour
{
    [Header("Status")]
    [SerializeField] protected float maxHP = 100; // 임의
    protected float currentHP;
    public float damage = 0;

    [SerializeField] protected Slider hpSlider;
    protected bool isDead = false;

    [Header("Component")]
    protected Animator anim;
    protected Collider col;
    protected Rigidbody rb;

    [SerializeField] protected Transform player; // Player 현재 위치


    // 코루틴 공통 관리
    protected Coroutine attackRoutine;

    protected virtual void OnEnable()
    {
        if (attackRoutine == null)
        {
            attackRoutine = StartCoroutine(AttackRoutine());
        }
    }

    protected virtual void OnDisable()
    {
        if (attackRoutine != null)
        {
            StopCoroutine(attackRoutine);
            attackRoutine = null;
        }
    }

    protected abstract IEnumerator AttackRoutine();
    

    protected virtual void Awake()
    {
        isDead = false;
        currentHP = maxHP;
        damage = player.GetComponent<PlayerMove>().Player_ATK;

        anim = GetComponent<Animator>();
        col = GetComponent<Collider>();
        rb = GetComponent<Rigidbody>();
    }

    protected virtual void Start()
    {
        UpdateUI();
    }

    // 피격 처리
    public virtual void TakeDamage(float damage)
    {
        if (isDead) return;

        currentHP -= damage;

        UpdateUI();

        if (currentHP <= 0)
        {
            Die();
        }
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            damage = player.GetComponent<PlayerMove>().Player_ATK;
            TakeDamage(damage);
            Destroy(other.gameObject);
        }
    }

    // UI 업데이트 (HP, ...)
    protected virtual void UpdateUI()
    {
        hpSlider.value = currentHP / maxHP;
    }

    // 사망 처리
    protected virtual void Die()
    {
        isDead = true;
        StopCoroutine(attackRoutine);
        attackRoutine = null;

        if (HasParameter("isDead"))
        {
            anim.SetBool("isDead", true);
        }

        if (HasParameter("Die"))
        {
            anim.SetTrigger("Die");
        }

        col.enabled = false;
        rb.isKinematic = true;

        StartCoroutine(DeathRoutine());
    }

    private bool HasParameter(string paramName)
    {
        if (anim == null) return false;

        foreach (AnimatorControllerParameter param in anim.parameters)
        {
            if (param.name == paramName) return true;
        }
        return false;
    }

    // Event에서
    public void DestroyBoss()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.StageClear();
        }
        Destroy(gameObject);
    }

    // Event없는 경우
    IEnumerator DeathRoutine()
    {
        yield return new WaitForSeconds(3.0f);
        DestroyBoss();
    }
}