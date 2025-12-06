using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{

    public float speed = 10f;
    public float rotSpeed = 2f;
    public Transform character;
    Transform CameraOriginalPos;

    float YtoGo;
    float RotXtoGo;
    // Start is called before the first frame update
    void Start()
    {
        CameraOriginalPos = transform;
        YtoGo = CameraOriginalPos.position.y;
        RotXtoGo = 20f;
    }

    // Update is called once per frame
    void Update()
    {
        float LerpX = Mathf.Lerp(transform.position.x, character.position.x, speed * Time.deltaTime);
        float LerpY = Mathf.Lerp(transform.position.y, YtoGo, speed * Time.deltaTime);
        transform.position = new Vector3(LerpX, LerpY, transform.position.z);
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(RotXtoGo, transform.rotation.y, transform.rotation.z), Time.deltaTime * rotSpeed);
        if (Input.GetKeyDown(KeyCode.Alpha4) || Input.GetKeyDown(KeyCode.Alpha2))
        {
            YtoGo = 2.5f;
            RotXtoGo = -6.6f;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Alpha3))
        {
            YtoGo = 4.25f;
            RotXtoGo = 20f;
        }
    }
}
