using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Game5Controller : MonoBehaviour
{
    public RuntimeAnimatorController firstLevelAlertAnim;
    public RuntimeAnimatorController secondLevelAlertAnim;
    public AudioSource soundBtnClick;
    public GameObject sound;
    public Sprite soundOn;
    public Sprite soundOff;
    private float time;
    private bool stopTimer = false;
    public Text timerText;
    public Text scoreTxt;
    public GameObject levelAlert;
    public AudioClip threeStar;
    public AudioClip twoStar;
    public AudioClip oneStar;
    public AudioClip zeroStar;
    public GameObject starsObj;
    public Sprite starOn;
    public Sprite starOff;
    public GameObject nextButton;
    public GameObject bananaPrefab;
    float elapsed = 0f;
    private void Update() {
        
        if (stopTimer == false)
        {
            elapsed += Time.deltaTime;
            if (elapsed >= 1f) {
                elapsed = elapsed % 0.5f;
                createBanana();
            }
            time += Time.deltaTime;
            timerText.text = Mathf.Round(time).ToString();
        }
        if (SoundController.music == true)
        {
            sound.GetComponent<Image>().sprite = soundOn;
        }
        else{
            sound.GetComponent<Image>().sprite = soundOff;
        }
        
    }

    private void createBanana(){
        if (scoreTxt.text == "30")
        {
            int localStar = 0;

            if (time <= 40)
            {
                LevelController.level5Star = 3;
                levelAlert.GetComponent<AudioSource>().clip = threeStar;
                localStar = 3;
            }
            else if(time <= 50)
            {
                levelAlert.GetComponent<AudioSource>().clip = twoStar;
                localStar = 2;
                if (LevelController.level5Star < 2)
                {
                    LevelController.level5Star = 2;
                }
            }
            else if(time <= 60){
                localStar = 1;
                levelAlert.GetComponent<AudioSource>().clip = oneStar;
                if (LevelController.level5Star < 1)
                {
                    LevelController.level5Star = 1;
                }
            }
            else
            {
                localStar = 0;
                levelAlert.GetComponent<AudioSource>().clip = zeroStar;
            }
            for (int i = 0; i < 3; i++)
            {
                starsObj.transform.GetChild(i).GetComponent<Image>().sprite = starOff;
            }
            for (int i = 0; i < localStar; i++)
            {
                starsObj.transform.GetChild(i).GetComponent<Image>().sprite = starOn;
            }
            if (LevelController.level5Star == 0)
            {
                nextButton.GetComponent<Button>().interactable = false;
            }
            else
            {
                nextButton.GetComponent<Button>().interactable = true;
            }
            Save.saveData();
            levelAlert.SetActive(true);
            stopTimer = true;
            levelAlert.GetComponent<AudioSource>().Play();
            levelAlert.GetComponent<Animator>().runtimeAnimatorController = firstLevelAlertAnim;
        }
        else
        {
            
        GameObject gameObj = Instantiate(bananaPrefab, bananaPrefab.transform.parent);
        gameObj.AddComponent<Banana>();
        }
    }
    public void RestartGame(){
        soundBtnClick.Play();
        levelAlert.GetComponent<Animator>().runtimeAnimatorController = secondLevelAlertAnim;
        scoreTxt.text = "0";
        GameObject.Find("bucket").GetComponent<Bucket>().score = 0;
        time = 0;
        stopTimer = false;
    }
    public void GoNextGame(){
        soundBtnClick.Play();
        Initiate.Fade("Game5", Color.black, 1.5f);
    }
    public void GoGameMenu(){
        Initiate.Fade("Game Menu", Color.black, 1f);
        soundBtnClick.Play();
    }
    
    public void Sound(){
        SoundController.music = !SoundController.music;
        soundBtnClick.Play();
    }
    
}
