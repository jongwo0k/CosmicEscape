using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundSpawn : MonoBehaviour
{
    public GameObject ground;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(spawn());   
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator spawn()
    {
        Instantiate(ground, gameObject.transform.position, gameObject.transform.rotation);
        yield return new WaitForSeconds(4f);
        StartCoroutine(spawn());
    }
}
