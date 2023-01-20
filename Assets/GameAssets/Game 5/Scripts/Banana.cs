using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Banana : MonoBehaviour
{
    public float targetPosX;
    void Start()
    {
        int random = Random.Range(0, 3);
        if (random == 0)
        {
            transform.position = new Vector3(-10f, 15f, 0f);
            targetPosX = -10f;
        }
        else if(random == 1)
        {
            transform.position = new Vector3(0f, 15f, 0f);
            targetPosX = 0f;
        }
        else
        {
            transform.position = new Vector3(10f, 15f, 0f);
            targetPosX = 10f;
        }
        targetPosX = transform.position.x;
    }
    void Update()
    {
        if (transform.position.y < -20f)
        {
            Destroy(gameObject);
        }
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(targetPosX, -50f, 0f), Time.deltaTime * 20f);
    }
}
