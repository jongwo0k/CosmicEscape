using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class PlayerMove : MonoBehaviour
{
    Transform tr;

    public Animator animator;
    public Transform FirePos;
    public GameObject Bullet;
    public GameObject Rifle;
    public GameObject MuzzleFlash;
    public GameObject Missile;
    public GameObject MissilePos_1;
    public GameObject MissilePos_2;

    public GameObject Target_1;
    public GameObject Target_2;
    public GameObject Target_3;
    public GameObject Target_4;

    public GameObject ReinforceScreen;
    public Button HPButton;
    public Button SpeedButton;

    public Slider HPSlider;
    public Slider XPSlider;

    GameObject MainTarget;

    public float normal_speed = 5f;
    public float roll_speed = 10f;
    public float speed = 5f;
    public float ATK_speed = 1f;
    public float ATK_accuracy = 1f;
    public float rolling_time = 0.3f;
    public float rolling_cooldown = 3f;
    public float Player_MaxHP = 20f;
    public float Player_ATK = 3f;
    public int missile_round = 3;

    bool isATK = true;
    bool isRoll = false;
    bool isMissileArmed = true;
    bool isReinforceTime = false;
    public bool isGround = true;
    public bool isPlayerAlive = true;
    public float PlayerHP;
    public float inGameTime = 0;

    // Start is called before the first frame update
    void Awake()
    {
        PlayerHP = Player_MaxHP;
        speed = normal_speed;
        MainTarget = Target_1;
        float degree = Mathf.Atan2(gameObject.transform.position.x, MainTarget.transform.position.z) * Mathf.Rad2Deg;
        float y_degree = Mathf.Atan2(MainTarget.transform.position.y - gameObject.transform.position.y, MainTarget.transform.position.z) * Mathf.Rad2Deg;
        FirePos.eulerAngles = new Vector3(-y_degree + Random.Range(-5f / ATK_accuracy, 5f / ATK_accuracy), -degree + Random.Range(-5f / ATK_accuracy, 5f / ATK_accuracy), 0);
    }

    void Start()
    {
        HPSlider.value = PlayerHP / Player_MaxHP;
        tr = GetComponent<Transform>();
        CrossHairCtrl();
        StartCoroutine(Fire());
        StartCoroutine(ReinforceTimer());
        StartCoroutine(GameTimer());
    }

    // Update is called once per frame
    void Update()
    {
        // HPSlider.value = PlayerHP / Player_MaxHP;

        if(PlayerHP <= 0 && isPlayerAlive)
        {
            isPlayerAlive = false;
            isATK = false;
            animator.SetTrigger("Death");
            GameManager.Instance.GameIsOver();
        }

        if (isPlayerAlive)
        {
            XPSlider.value = inGameTime / 30f;
            if (!isRoll)
            {
                if (Input.GetKeyDown(KeyCode.Alpha1))
                {
                    MainTarget = Target_1;
                    CrossHairCtrl();
                }
                if (Input.GetKeyDown(KeyCode.Alpha2))
                {
                    MainTarget = Target_2;
                    CrossHairCtrl();
                }
                if (Input.GetKeyDown(KeyCode.Alpha3))
                {
                    MainTarget = Target_3;
                    CrossHairCtrl();
                }
                if (Input.GetKeyDown(KeyCode.Alpha4))
                {
                    MainTarget = Target_4;
                    CrossHairCtrl();
                }

                if ((Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)) && Input.GetKeyDown(KeyCode.LeftShift) && isGround)
                {
                    animator.SetTrigger("Roll");
                    StartCoroutine(Roll());
                }

                if (Input.GetKeyDown(KeyCode.T) && isMissileArmed && missile_round > 0)
                {
                    StartCoroutine(Missile_Fire());
                }
            }

            if (Input.GetKeyDown(KeyCode.V))
            {
                if (isATK) isATK = false;
                else isATK = true;
            }

            if (Input.GetKeyDown(KeyCode.Space) && isGround)
            {
                isGround = false;
                gameObject.GetComponent<Rigidbody>().AddForce(Vector3.up * 5f, ForceMode.Impulse);
            }

            if (inGameTime >= 30)
            {
                PlayerReinforce();
            }

            float x = Input.GetAxis("Horizontal");
            Vector3 dir = new Vector3(x, 0, 0);
            tr.Translate(dir * speed * Time.deltaTime);

            if (MainTarget != null)
            {
                float degree = Mathf.Atan2(FirePos.transform.position.x - MainTarget.transform.position.x, MainTarget.transform.position.z - FirePos.transform.position.z) * Mathf.Rad2Deg;
                float y_degree = Mathf.Atan2(MainTarget.transform.position.y - FirePos.transform.position.y, MainTarget.transform.position.z - FirePos.transform.position.z) * Mathf.Rad2Deg;
                FirePos.eulerAngles = new Vector3(-y_degree + Random.Range(-5f / ATK_accuracy, 5f / ATK_accuracy), -degree + Random.Range(-5f / ATK_accuracy, 5f / ATK_accuracy), 0);
                Rifle.transform.eulerAngles = new Vector3(-y_degree, -degree - 90f, 0);
            }
            
            /*
            float ms1_degree = Mathf.Atan2(MissilePos_1.transform.position.x - MainTarget.transform.position.x, MainTarget.transform.position.z - MissilePos_1.transform.position.z) * Mathf.Rad2Deg;
            float ms1_y_degree = Mathf.Atan2(MissilePos_1.transform.position.y - MainTarget.transform.position.y, MainTarget.transform.position.z - MissilePos_1.transform.position.z) * Mathf.Rad2Deg;
            float ms2_degree = Mathf.Atan2(MissilePos_2.transform.position.x - MainTarget.transform.position.x, MainTarget.transform.position.z - MissilePos_2.transform.position.z) * Mathf.Rad2Deg;
            float ms2_y_degree = Mathf.Atan2(MissilePos_2.transform.position.y - MainTarget.transform.position.y, MainTarget.transform.position.z - MissilePos_2.transform.position.z) * Mathf.Rad2Deg;

            MissilePos_1.transform.eulerAngles = new Vector3(ms1_y_degree, -ms1_degree, 0);
            MissilePos_2.transform.eulerAngles = new Vector3(ms2_y_degree, -ms2_degree, 0);
            */
        }

    }

    public void TakeDamage(float damage)
    {
        if (!isPlayerAlive) return;
        if (isRoll) return;

        PlayerHP -= damage;
        HPSlider.value = PlayerHP / Player_MaxHP;
    }

    void CrossHairCtrl()
    {
        Target_1.SetActive(false);
        Target_2.SetActive(false);
        Target_3.SetActive(false);
        Target_4.SetActive(false);

        MainTarget.SetActive(true);
    }

    void PlayerReinforce()
    {
        ReinforceScreen.SetActive(true);
        Time.timeScale = 0;
    }

    public void ButtonATKReinforce()
    {
        inGameTime = 0;
        isReinforceTime = false;
        Player_ATK += 1;
        Time.timeScale = 1;
        ReinforceScreen.SetActive(false);
        StartCoroutine(ReinforceTimer());
    }
    public void ButtonHPRecovery()
    {
        inGameTime = 0;
        isReinforceTime = false;
        if (PlayerHP <= 10)
            PlayerHP += 10;
        else
            PlayerHP = 20;
        Time.timeScale = 1;
        ReinforceScreen.SetActive(false);
        StartCoroutine(ReinforceTimer());
    }
    public void ButtonSpeedReinforce()
    {
        inGameTime = 0;
        isReinforceTime = false;
        speed += 1;
        roll_speed += 2;
        Time.timeScale = 1;
        ReinforceScreen?.SetActive(false);
        StartCoroutine(ReinforceTimer());
    }
    IEnumerator Missile_Fire()
    {
        isMissileArmed = false;
        missile_round--;
        Instantiate(Missile, MissilePos_1.transform.position, MissilePos_1.transform.rotation);
        Instantiate(Missile, MissilePos_2.transform.position, MissilePos_2.transform.rotation);
        yield return new WaitForSeconds((float)10 / ATK_speed);
        isMissileArmed = true;
    }

    IEnumerator Fire()
    {
        if (!isRoll && isATK)
        {
            Instantiate(Bullet, FirePos.position, FirePos.rotation);
            StartCoroutine(Muzzle());
        }
        yield return new WaitForSeconds((float)1 / ATK_speed);
        StartCoroutine(Fire());
    }

    IEnumerator Muzzle()
    {
        MuzzleFlash.SetActive(true);
        yield return new WaitForSeconds((float)0.2 / ATK_speed);
        MuzzleFlash.SetActive(false);
    }

    IEnumerator Roll()
    {
        isRoll = true;
        speed = roll_speed;
        yield return new WaitForSeconds(rolling_time);
        speed = normal_speed;
        yield return new WaitForSeconds(rolling_cooldown - rolling_time);
        isRoll = false;
    }

    IEnumerator ReinforceTimer()
    {
        yield return new WaitForSeconds(30f);
        isReinforceTime = true;
    }
    IEnumerator GameTimer()
    {
        if (inGameTime == 30)
            inGameTime = 0;
        else
        {
            inGameTime += 1;
            yield return new WaitForSeconds(1f);
        }
        StartCoroutine(GameTimer());
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Ground")
        {
            isGround = true;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Spike")
        {
            TakeDamage(2);
            Debug.Log(PlayerHP);
        }
    }
}