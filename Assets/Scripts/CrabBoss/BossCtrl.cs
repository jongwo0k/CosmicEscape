using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BossCtrl : Boss
{
    public GameObject Blood;
    // public GameObject Player;
    public GameObject Shockwave;
    public GameObject ShockwavePos;
    public PlayerMove playerMove;

    Transform OriginalTransform;
    Vector3 OriginalPos;
    //public GameObject Blood_Big;

    public float MaxHP = 10;
    public float patternTime = 15f;
    public float rotateSpeed = 5f;
    // public float HP;

    int activatedPattern = -1;
    // bool isPatternCooldown = true;
    bool isPatternPlaying = false;
    bool isAttack = false;
    bool isShockwaveEx = false;

    // float damage = 0;

    protected override void Awake()
    {
        base.maxHP = MaxHP;
        base.Awake();

        OriginalPos = gameObject.transform.position;
        OriginalTransform = gameObject.transform;
        // damage = player.GetComponent<PlayerMove>().Player_ATK;
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override IEnumerator AttackRoutine()
    {
        float patternCooldown = 0f;
        while (!isDead && playerMove != null && playerMove.isPlayerAlive)
        {
            if (activatedPattern == -1)
            {
                anim.SetTrigger("Idle");
                IdlePattern();

                patternCooldown += Time.deltaTime;

                if (patternCooldown >= patternTime)
                {
                    activatedPattern = (int)Random.Range(0, 2);
                    patternCooldown = 0f; // 타이머 초기화
                }
            }

            if (activatedPattern == 0)
            {
                LegAttackPattern();
            }

            if (activatedPattern == 1 && !isPatternPlaying)
            {
                Debug.Log("JumpPattern");
                JumpAttackPattern();
            }

            yield return null;
        }
    }

    void IdlePattern()
    {
        gameObject.transform.rotation = Quaternion.Slerp(gameObject.transform.rotation, Quaternion.Euler(0, 180f, 0), rotateSpeed * Time.deltaTime);
        gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, Mathf.Lerp(gameObject.transform.position.z, OriginalPos.z, 3f * Time.deltaTime));
    }

    void LegAttackPattern()
    {
        float RotY = Mathf.Atan2(player.transform.position.x - gameObject.transform.position.x, player.transform.position.z - gameObject.transform.position.z) * Mathf.Rad2Deg - 15f;
        float GetFrontCoord = Mathf.Sqrt(Mathf.Pow((player.transform.position.z - gameObject.transform.position.z), 2) + Mathf.Pow(player.transform.position.x - gameObject.transform.position.x, 2)) - (gameObject.transform.position.z - player.transform.position.z);
        
        if(!isPatternPlaying)
            StartCoroutine(patternWhileTimer(1.3f));

        if (!isAttack)
        {
            gameObject.transform.rotation = Quaternion.Slerp(gameObject.transform.rotation, Quaternion.Euler(0, RotY, 0), rotateSpeed * Time.deltaTime);
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, Mathf.Lerp(gameObject.transform.position.z, OriginalPos.z - Mathf.Abs(Mathf.Sin(RotY) * GetFrontCoord), 3f * Time.deltaTime));
        }
    }

    void JumpAttackPattern()
    {
        anim.SetTrigger("Intimidate_2");
        isShockwaveEx = false;
        if(!isPatternPlaying) StartCoroutine(jumpAttack());
    }

    IEnumerator patternWhileTimer(float time)
    {
        isPatternPlaying = true;
        
        yield return new WaitForSeconds(0.5f);
        isAttack = true;
        rotateSpeed = 1.5f;
        anim.SetTrigger("Attack_1");
        anim.SetTrigger("Idle");
        
        yield return new WaitForSeconds(time);
        rotateSpeed = 5f;
        isAttack = false;
        isPatternPlaying = false;
        activatedPattern = -1;
    }

    IEnumerator jumpAttack()
    {
        isPatternPlaying = true;
        yield return new WaitForSeconds(1.8f);
        anim.SetTrigger("Idle");
        rb.AddForce(Vector3.up * 100f, ForceMode.Impulse);
        yield return new WaitForSeconds(0.7f);
        rb.AddForce(Vector3.up * -170f, ForceMode.Impulse);
        yield return new WaitForSeconds(0.85f);

        if (!isShockwaveEx)
        {
            GameObject SW = Instantiate(Shockwave, ShockwavePos.transform.position, ShockwavePos.transform.rotation);
            Destroy(SW, 1f);
            isShockwaveEx = true;
        }

        if (playerMove != null && playerMove.isGround)
        {
            playerMove.TakeDamage(7);
        }

        yield return new WaitForSeconds(1f);
        anim.SetTrigger("Idle");
        activatedPattern = -1;
        isPatternPlaying = false;
    }

    /*
    private void OnCollisionEnter(Collision collision)
    {
        
        if (collision.gameObject.tag == "Bullet")
        {
            Destroy(collision.transform.parent.gameObject);
            GameObject _Blood = Instantiate(Blood, collision.transform.position, collision.gameObject.transform.rotation);
            Destroy(_Blood, 0.3f);
            HP -= damage;
        }

        if(collision.gameObject.tag == "Missile")
        {
            Destroy(collision.transform.parent.gameObject);
            GameObject _Blood = Instantiate(Blood, collision.transform.position, collision.gameObject.transform.rotation);
            Destroy(_Blood, 0.3f);
            HP -= 30;
        }
        
    }
    */
    protected override void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Bullet")
        {
            Destroy(other.transform.parent.gameObject);
            GameObject _Blood = Instantiate(Blood, other.transform.position, other.gameObject.transform.rotation);
            Destroy(_Blood, 0.3f);
            damage = playerMove.Player_ATK;
            base.TakeDamage(damage);
        }
    }
}
