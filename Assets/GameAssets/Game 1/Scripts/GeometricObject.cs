using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeometricObject : MonoBehaviour
{
    public bool draggable = true;
    public bool animationMove = true;
    public int index;
    public Vector2 firstPosition;
    public Vector2 targetPos;

    private void Start() {
        //transform.position = new Vector3(0f, 0f, 0f);
        targetPos = firstPosition;
    }

    private void Update() {
        MoveAnimation(gameObject.transform.position, targetPos);
    }

    public void MoveAnimation(Vector2 obj, Vector2 targetPos){
        if (animationMove == false)
        {
            gameObject.transform.position = Vector2.MoveTowards(obj, targetPos, Time.deltaTime * 20f);
        }
    }
}
