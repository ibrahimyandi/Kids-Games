using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchRotate : MonoBehaviour
{
    public bool rotating = false;
    public float rotateDeg = 0;
    public float position;
    public AudioSource soundCorrectPosition;

    private void Update() {
        if (rotating)
        {
            if (position + 90f == transform.localEulerAngles.z)
            {
                rotating = false;
            }
            else
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(new Vector3(0f, 0f, rotateDeg)), 10f * Time.deltaTime);
            }
        }
    }
    private void OnMouseDown() {
        if (Mathf.Round(transform.localEulerAngles.z) == 0)
        {
            rotateDeg = 90f;
        }
        else if (Mathf.Round(transform.localEulerAngles.z) == 90)
        {
            rotateDeg = 180f;
        }
        else if (Mathf.Round(transform.localEulerAngles.z) == 180)
        {
            rotateDeg = -90f;
        }
        else if (Mathf.Round(transform.localEulerAngles.z) == 270)
        {
            rotateDeg = 360f;
            soundCorrectPosition.Play();
        }
        else if (Mathf.Round(transform.localEulerAngles.z) == 360)
        {
            rotateDeg = 90f;
        }
        rotating = true;
        position = Mathf.Round(transform.localEulerAngles.z);
        
    }
}
