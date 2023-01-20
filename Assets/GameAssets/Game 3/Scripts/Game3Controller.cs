using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Game3Controller : MonoBehaviour
{
    public RuntimeAnimatorController firstAnim;
    public RuntimeAnimatorController secondAnim;
    public RuntimeAnimatorController firstLevelAlertAnim;
    public RuntimeAnimatorController secondLevelAlertAnim;
    public RuntimeAnimatorController wrongAnswerAnim;
    public RuntimeAnimatorController correctAnswerAnim;
    public AudioSource soundBtnClick;
    public AudioSource soundCorrectImage;
    public AudioSource soundWrongNumber;
    public AudioClip threeStar;
    public AudioClip twoStar;
    public AudioClip oneStar;
    public AudioClip zeroStar;
    public GameObject nextButton;
    public GameObject gamePanel;
    public GameObject starsObj;
    public Sprite starOn;
    public Sprite starOff;
    public Text scoreText;
    public Text timerText;
    public GameObject gameObj;
    public Image[] answerImg;
    public Sprite[] answerSprite;
    public Image questionImg;
    public static int questionIndex;
    string actionSpriteName;
    public GameObject sound;
    public Sprite soundOn;
    public Sprite soundOff;
    private float time;
    private bool stopTimer = false;
    private int globalScore = 0;
    private int wrongAnswer = 0;
    public GameObject levelAlert;
    public int[] levelDesign = new int[5];

    void Start()
    {
        gameObj.GetComponent<Animator>().runtimeAnimatorController = firstAnim;
        RandomQuestion();
        SetSprite();
    }

    private void Update() {
        if (SoundController.music == true)
        {
            sound.GetComponent<Image>().sprite = soundOn;
        }
        else{
            sound.GetComponent<Image>().sprite = soundOff;
        }
        if (stopTimer == false)
        {
            time += Time.deltaTime;
            timerText.text = Mathf.Round(time).ToString();
        }
    }

    public void NextButton(){
        answerImg[questionIndex].GetComponent<Animator>().runtimeAnimatorController = null;
        answerImg[questionIndex].GetComponent<Button>().interactable = false;
        gameObj.GetComponent<Animator>().runtimeAnimatorController = secondAnim;
        StartCoroutine(StartAnimation());
        wrongAnswer = 0;
    }

    public IEnumerator StartAnimation(){
        yield return new WaitForSeconds(0.7f);
        if (globalScore == 5)
        {
            int localStar = 0;
            if (time <= 15)
            {
                levelAlert.GetComponent<AudioSource>().clip = threeStar;
                LevelController.level3Star = 3;
                localStar = 3;
            }
            else if(time <= 20)
            {
                levelAlert.GetComponent<AudioSource>().clip = twoStar;
                localStar = 2;
                if (LevelController.level3Star < 2)
                {
                    LevelController.level3Star = 2;
                }
            }
            else if(time <= 30){
                levelAlert.GetComponent<AudioSource>().clip = oneStar;
                localStar = 1;
                if (LevelController.level3Star < 1)
                {
                    LevelController.level3Star = 1;
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
            if (LevelController.level3Star == 0)
            {
                nextButton.GetComponent<Button>().interactable = false;
            }
            else
            {
                nextButton.GetComponent<Button>().interactable = true;
            }
            levelAlert.SetActive(true);
            stopTimer = true;
            levelAlert.GetComponent<AudioSource>().Play();
            levelAlert.GetComponent<Animator>().runtimeAnimatorController = firstLevelAlertAnim;
            gamePanel.SetActive(false);
            Save.saveData();
        }
        questionIndex = levelDesign[globalScore];

        gameObj.GetComponent<Animator>().runtimeAnimatorController = firstAnim;
        questionImg.GetComponent<Image>().sprite = answerSprite[questionIndex];
        for (int i = 0; i < 10; i++)
        {
            answerImg[i].GetComponent<Button>().interactable = true;
        }
    }
    
    public IEnumerator WaitNextQuestion(){
        yield return new WaitForSeconds(1f);
        NextButton();
    }
    public void RandomQuestion(){
        levelDesign = new int[5];
        int i = 0;
        while (i < levelDesign.Length) 
        {
            int sayi = Random.Range(0, 10);
            if(System.Array.IndexOf(levelDesign, sayi) != -1)
                continue;
            levelDesign[i] = sayi;
            i++;
        }
    }

    public void SetSprite()
    {
        questionIndex = levelDesign[globalScore];
        questionImg.GetComponent<Image>().sprite = answerSprite[questionIndex];
        for (int i = 0; i < 10; i++)
        {
            answerImg[i].GetComponent<Image>().sprite = answerSprite[i];
        }
    }
    
    public void AnswerSpriteAction(Image img){
        actionSpriteName = img.sprite.name;
        string questionName = questionImg.sprite.name;
        foreach (var item in answerImg)
        {
            item.GetComponent<Animator>().runtimeAnimatorController = null;
        }
        if (questionName.Equals(actionSpriteName))
        {
            if (wrongAnswer < 2)
            {
                globalScore++;
                soundCorrectImage.Play();
                scoreText.text = globalScore.ToString();
            }
            StartCoroutine(WaitNextQuestion());
            answerImg[questionIndex].GetComponent<Animator>().runtimeAnimatorController = correctAnswerAnim;
            answerImg[questionIndex].GetComponent<Button>().interactable = false;
        }
        else
        {
            soundWrongNumber.Play();
            img.GetComponent<Animator>().runtimeAnimatorController = wrongAnswerAnim;
            wrongAnswer++;
        }
    }
    public IEnumerator waitAlert(){
        yield return new WaitForSeconds(1f);
        levelAlert.SetActive(false);
    }
    public IEnumerator waitGamePanel(){
        yield return new WaitForSeconds(0.7f);
        gamePanel.SetActive(true);
    }
    public void RestartGame(){
        soundBtnClick.Play();
        globalScore = 0;
        time = 0;
        wrongAnswer = 0;
        scoreText.text = globalScore.ToString();
        levelAlert.GetComponent<Animator>().runtimeAnimatorController = secondLevelAlertAnim;
        timerText.text = time.ToString();
        stopTimer = false;
        gamePanel.transform.position = new Vector2(1920f, 0f);
        StartCoroutine(waitGamePanel());
        StartCoroutine(waitAlert());
        StartCoroutine(StartAnimation());
        RandomQuestion();
    }
    public void GoNextGame(){
        soundBtnClick.Play();
        Initiate.Fade("Game4", Color.black, 1.5f);
    }

    public void GoGameMenu(){
        soundBtnClick.Play();
        Initiate.Fade("Game Menu", Color.black, 1f);
    }

    public void Sound(){
        soundBtnClick.Play();
        SoundController.music = !SoundController.music;
    }
}
