using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public enum SIDE { Left, Mid, Right };
public class Bucket : MonoBehaviour
{
    public AudioSource correct;
    public AudioSource swipeSound;
    public SIDE m_Side = SIDE.Mid;
    public bool SwipeLeft;
    public bool SwipeRight;
    public Text scoreTxt;
    public bool moving = false;
    public int score = 0;
    Vector3 targetPosition;

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.tag == "banana")
        {
            correct.Play();
            score++;
            scoreTxt.text = score.ToString();
            Destroy(other.gameObject);
        }
    }

    private void Start() {
        targetPosition = transform.position;
    }

    private void Update() {
        SwipeLeft = Input.GetKeyDown(KeyCode.A);
        SwipeRight = Input.GetKeyDown(KeyCode.D);
        if (SwipeRight)
        {
            swipeSound.Play();
            if (m_Side == SIDE.Mid)
            {
                m_Side = SIDE.Right;
            }
            else if (m_Side == SIDE.Left)
            {
                m_Side = SIDE.Mid;
            }
        }
        if (SwipeLeft)
        {
            swipeSound.Play();
            if (m_Side == SIDE.Mid)
            {
                m_Side = SIDE.Left;
            }
            else if (m_Side == SIDE.Right)
            {
                m_Side = SIDE.Mid;
            }
        }
        if (Input.touchCount > 0)
        {
            Touch swipe = Input.GetTouch(0);
            if (!moving)
            {
                if (swipe.deltaPosition.x > 30f)
                {
                    swipeSound.Play();
                    if (m_Side == SIDE.Mid)
                    {
                        m_Side = SIDE.Right;
                    }
                    else if (m_Side == SIDE.Left)
                    {
                        m_Side = SIDE.Mid;
                    }
                }
                if (swipe.deltaPosition.x < -30f)
                {
                    swipeSound.Play();
                    if (m_Side == SIDE.Mid)
                    {
                        m_Side = SIDE.Left;
                    }
                    else if (m_Side == SIDE.Right)
                    {
                        m_Side = SIDE.Mid;
                    }
                }
            }
            moving = true;
        }
       
        if (m_Side == SIDE.Left)
        {
            targetPosition = new Vector3(-10f, -4f, 0f);
        }
        else if(m_Side == SIDE.Mid)
        {
            targetPosition = new Vector3(0f, -4f, 0f);
        }
        else if(m_Side == SIDE.Right){
            targetPosition = new Vector3(10f, -4f, 0f);
        }
        moveAnimation(transform.position, targetPosition);
    }
    
    private void moveAnimation(Vector3 currentPos, Vector3 targetPos){
        if (transform.position == targetPos)
        {
            moving = false;
        }
        else
        {
            moving = true;
            transform.position = Vector3.Lerp(currentPos, targetPos, 20 * Time.deltaTime);
        }
    }
}
