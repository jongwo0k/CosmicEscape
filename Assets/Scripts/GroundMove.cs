using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundMove : MonoBehaviour
{
    public float movespeed = 10.0f;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 30);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += new Vector3(0, 0, 1) * -movespeed * Time.deltaTime;
    }
}
