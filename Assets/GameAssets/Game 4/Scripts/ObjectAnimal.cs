using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectAnimal : MonoBehaviour
{
    public bool draggable = true;
    public int index;
    public Vector2 firstPosition;
    public bool move = false;
    public Vector3 targetPos;

    private void Awake() {
        
    }
    private void Start() {
        targetPos = transform.position;
    }

    private void Update() {
        if (gameObject.transform.position != targetPos && this.GetComponent<Animator>().runtimeAnimatorController == null)
        {
            if (move == true)
            {
                MoveAnimation(gameObject.transform.position, targetPos);
            }
        }
    }

    public void MoveAnimation(Vector2 obj, Vector2 targetPos){
        gameObject.transform.position = Vector2.MoveTowards(obj, targetPos, Time.deltaTime * 20f);
    }
}
