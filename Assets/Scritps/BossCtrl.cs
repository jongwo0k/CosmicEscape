using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BossCtrl : MonoBehaviour
{
    public GameObject Blood;
    public GameObject player;
    public GameObject Shockwave;
    public GameObject ShockwavePos;
    public Animator _anim;
    public Slider HPSlider;
    Transform OriginalTransform;
    Vector3 OriginalPos;
    //public GameObject Blood_Big;

    public float MaxHP = 10;
    public float patternTime = 15f;
    public float rotateSpeed = 5f;
    public float HP;
    int activatedPattern = -1;
    bool isPatternCooldown = true;
    bool isPatternPlaying = false;
    bool isAttack = false;
    bool isShockwaveEx = false;
    bool isAlive = true;

    Rigidbody rb;

    float damage = 0;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        damage = player.GetComponent<PlayerMove>().Player_ATK;
        HP = MaxHP;
        OriginalPos = gameObject.transform.position;
        OriginalTransform = gameObject.transform;
        StartCoroutine(patternTimer());
    }

    // Update is called once per frame
    void Update()
    {
        damage = player.GetComponent<PlayerMove>().Player_ATK;
        HPSlider.value = HP / MaxHP;
        if (HP <= 0 && isAlive) BossDead();
        
        if(isAlive && player.GetComponent<PlayerMove>().isPlayerAlive)
        {
            if (!isPatternCooldown)
            {
                activatedPattern = (int)Random.Range(0, 2);
                Debug.Log(activatedPattern);
                isPatternCooldown = true;
            }

            if (activatedPattern == -1)
            {
                _anim.SetTrigger("Idle");
                IdlePattern();
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
        }
        

    }

    void BossDead()
    {
        isAlive = false;
        _anim.SetTrigger("Die");
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
        _anim.SetTrigger("Intimidate_2");
        isShockwaveEx = false;
        if(!isPatternPlaying) StartCoroutine(jumpAttack());
    }

    IEnumerator patternTimer()
    {
        yield return new WaitForSeconds(patternTime);
        isPatternCooldown = false;
        StartCoroutine(patternTimer());
    }

    IEnumerator patternWhileTimer(float time)
    {
        isPatternPlaying = true;
        
        yield return new WaitForSeconds(0.5f);
        isAttack = true;
        rotateSpeed = 1.5f;
        _anim.SetTrigger("Attack_1");
        _anim.SetTrigger("Idle");
        
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
        _anim.SetTrigger("Idle");
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

        if(player.GetComponent<PlayerMove>().isGround)
            player.GetComponent<PlayerMove>().PlayerHP -= 7;
        yield return new WaitForSeconds(1f);
        _anim.SetTrigger("Idle");
        activatedPattern = -1;
        isPatternPlaying = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        /*
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
        */
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Bullet")
        {
            Destroy(other.transform.parent.gameObject);
            GameObject _Blood = Instantiate(Blood, other.transform.position, other.gameObject.transform.rotation);
            Destroy(_Blood, 0.3f);
            HP -= damage;
        }
    }
}
