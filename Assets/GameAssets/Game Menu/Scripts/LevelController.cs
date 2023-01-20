using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelController : MonoBehaviour
{
    public Sprite star;
    public Button btnLevel1, btnLevel2, btnLevel3, btnLevel4, btnLevel5, btnLevel6;
    public static int level1Star, level2Star, level3Star, level4Star, level5Star, level6Star;

    private void Start() {
        Save.getData();
        for (int i = 2; i < level1Star+2; i++)
        {
            btnLevel1.transform.GetChild(i).GetComponent<Image>().sprite = star;
        }
        for (int i = 2; i < level2Star+2; i++)
        {
            btnLevel2.transform.GetChild(i).GetComponent<Image>().sprite = star;
        }
        for (int i = 2; i < level3Star+2; i++)
        {
            btnLevel3.transform.GetChild(i).GetComponent<Image>().sprite = star;
        }
        for (int i = 2; i < level4Star+2; i++)
        {
            btnLevel4.transform.GetChild(i).GetComponent<Image>().sprite = star;
        }
        for (int i = 2; i < level5Star+2; i++)
        {
            btnLevel5.transform.GetChild(i).GetComponent<Image>().sprite = star;
        }
        for (int i = 2; i < level6Star+2; i++)
        {
            btnLevel6.transform.GetChild(i).GetComponent<Image>().sprite = star;
        }
    }
    private void Update() {
        
        if (level1Star > 0)
        {
            btnLevel2.interactable = true;
            btnLevel2.transform.Find("Img_LevelLock").gameObject.SetActive(false);
            
        }
        if (level2Star > 0)
        {
            btnLevel3.interactable = true;
            btnLevel3.transform.Find("Img_LevelLock").gameObject.SetActive(false);

        }
        if (level3Star > 0)
        {
            btnLevel4.interactable = true;
            btnLevel4.transform.Find("Img_LevelLock").gameObject.SetActive(false);
        }
        if (level4Star > 0)
        {
            btnLevel5.interactable = true;
            btnLevel5.transform.Find("Img_LevelLock").gameObject.SetActive(false);
        }
        if (level5Star > 0)
        {
            //btnLevel6.interactable = true;
            //btnLevel6.transform.Find("Img_LevelLock").gameObject.SetActive(false);
        }
    }
}
