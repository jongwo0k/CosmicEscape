using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossHairRot : MonoBehaviour
{
    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float degree = Mathf.Atan2(gameObject.transform.position.x - player.transform.position.x, player.transform.position.z - gameObject.transform.position.z) * Mathf.Rad2Deg;
        float y_degree = Mathf.Atan2(player.transform.position.y - gameObject.transform.position.y, player.transform.position.z - gameObject.transform.position.z) * Mathf.Rad2Deg;
        gameObject.transform.eulerAngles = new Vector3(y_degree, -degree, 0);
    }
}
