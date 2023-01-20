using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Game2Controller : MonoBehaviour
{
    public RuntimeAnimatorController firstAnim;
    public RuntimeAnimatorController secondAnim;
    public RuntimeAnimatorController firstLevelAlertAnim;
    public RuntimeAnimatorController secondLevelAlertAnim;
    public GameObject nextButton;
    public AudioSource soundBtnClick;
    public AudioSource soundCorrectImage;
    public AudioClip threeStar;
    public AudioClip twoStar;
    public AudioClip oneStar;
    public AudioClip zeroStar;
    public GameObject starsObj;
    public Sprite starOn;
    public Sprite starOff;
    public GameObject sound;
    public Sprite soundOn;
    public Sprite soundOff;
    private int globalScore = 0;
    public Text scoreText;
    public Text timerText;
    public GameObject levelAlert;
    public GameObject gamePanel;
    private bool stopTimer = false; 
    private float time = 0; 
    [SerializeField]
    private List<Sprite> pictures; 
    
    [SerializeField]
    private List<GameObject> gamePicture;
    private int[] levelDesign = new int[3];
    private int level = 0;
    private bool imageControl = false;

    private void Start() {
        createImageArr();
        StartCoroutine(gamePanelFirstAnim());
    }

    private void Update() {
        if (stopTimer == false)
        {
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
        controlPicture();
    }

    private void controlPicture(){
        if (globalScore == 3)
        {
            int localStar = 0;
            if (time <= 55)
            {
                levelAlert.GetComponent<AudioSource>().clip = threeStar;
                LevelController.level2Star = 3;
                localStar = 3;
            }
            else if(time <= 60)
            {
                levelAlert.GetComponent<AudioSource>().clip = twoStar;
                localStar = 2;
                if (LevelController.level2Star < 2)
                {
                    LevelController.level2Star = 2;
                }
            }
            else if(time <= 70){
                levelAlert.GetComponent<AudioSource>().clip = oneStar;
                localStar = 1;
                if (LevelController.level2Star < 1)
                {
                    LevelController.level2Star = 1;
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
            if (LevelController.level2Star == 0)
            {
                nextButton.GetComponent<Button>().interactable = false;
            }
            else
            {
                nextButton.GetComponent<Button>().interactable = true;
            }
            gamePanel.SetActive(false);
            levelAlert.GetComponent<AudioSource>().Play();
            levelAlert.GetComponent<Animator>().runtimeAnimatorController = firstLevelAlertAnim;
            stopTimer = true;
            Save.saveData();
        }
        else
        {
            if (imageControl == false)
            {
                if ((Mathf.Round(gamePicture[0].transform.localEulerAngles.z) <= 30f && Mathf.Round(gamePicture[0].transform.localEulerAngles.z) >= -30f) &&
                (Mathf.Round(gamePicture[1].transform.localEulerAngles.z) <= 30f && Mathf.Round(gamePicture[0].transform.localEulerAngles.z) >= -30f) &&
                (Mathf.Round(gamePicture[2].transform.localEulerAngles.z) <= 30f && Mathf.Round(gamePicture[0].transform.localEulerAngles.z) >= -30f) &&
                (Mathf.Round(gamePicture[3].transform.localEulerAngles.z) <= 30f && Mathf.Round(gamePicture[0].transform.localEulerAngles.z) >= -30f) &&
                (Mathf.Round(gamePicture[4].transform.localEulerAngles.z) <= 30f && Mathf.Round(gamePicture[0].transform.localEulerAngles.z) >= -30f) &&
                (Mathf.Round(gamePicture[5].transform.localEulerAngles.z) <= 30f && Mathf.Round(gamePicture[0].transform.localEulerAngles.z) >= -30f) &&
                (Mathf.Round(gamePicture[6].transform.localEulerAngles.z) <= 30f && Mathf.Round(gamePicture[0].transform.localEulerAngles.z) >= -30f) &&
                (Mathf.Round(gamePicture[7].transform.localEulerAngles.z) <= 30f && Mathf.Round(gamePicture[0].transform.localEulerAngles.z) >= -30f) &&
                (Mathf.Round(gamePicture[8].transform.localEulerAngles.z) <= 30f && Mathf.Round(gamePicture[0].transform.localEulerAngles.z) >= -30f)
                )
                {
                    soundCorrectImage.Play();
                    imageControl = true;
                    globalScore++;
                    scoreText.text = globalScore.ToString();
                    StartCoroutine(gamePanelSecondAnim());
                }
            }
        }
    }
    private void createImageArr(){
        int i = 0;
        while (i < levelDesign.Length) 
        {
            int sayi = Random.Range(0, 12);
            if(System.Array.IndexOf(levelDesign, sayi) != -1)
                continue;
            levelDesign[i] = sayi;
            i++;
        }
    }
    public IEnumerator gamePanelFirstAnim(){
        RefreshGame();
        imageControl = false;
        yield return new WaitForSeconds(0.5f);
        gamePanel.GetComponent<Animator>().runtimeAnimatorController = firstAnim;
    }
    public IEnumerator gamePanelSecondAnim(){
        gamePanel.GetComponent<Animator>().runtimeAnimatorController = secondAnim;
        yield return new WaitForSeconds(1f);
        StartCoroutine(gamePanelFirstAnim());
    }
    private void RefreshGame(){
        int i = 0;
        foreach (var gp in gamePicture)
        {
            gp.GetComponent<SpriteRenderer>().sprite = pictures[i + levelDesign[level] * 9];
            gp.GetComponent<TouchRotate>().rotateDeg = 0;
            gp.GetComponent<TouchRotate>().position = 0;
            gp.GetComponent<TouchRotate>().rotating = false;
            int randomRotate = Random.Range(0, 3);
            if (randomRotate == 0)
            {
                gp.transform.Rotate(0f, 0f, 90f);
            }
            else if (randomRotate == 1)
            {
                gp.transform.Rotate(0f, 0f, 180f);
            }
            else if (randomRotate == 2)
            {
                gp.transform.Rotate(0f, 0f, 270f);
            }
            i++;
        }
        level++;
    }

    public void RestartGame(){
        soundBtnClick.Play();
        globalScore = 0;
        time = 0;
        level = 0;
        scoreText.text = globalScore.ToString();
        levelAlert.GetComponent<Animator>().runtimeAnimatorController = secondLevelAlertAnim;
        timerText.text = time.ToString();
        stopTimer = false;
        System.Array.Clear(levelDesign, 0, levelDesign.Length);
        createImageArr();
        gamePanel.transform.position = new Vector2(-1920f, 0f);
        gamePanel.GetComponent<Animator>().runtimeAnimatorController = null;

        StartCoroutine(gamePanelFirstAnim());

        gamePanel.SetActive(true);
    }

    public void GoNextGame(){
        Initiate.Fade("Game3", Color.black, 1.5f);
        soundBtnClick.Play();
    }

    public void GoGameMenu(){
        Initiate.Fade("Game Menu", Color.black, 2f);
        soundBtnClick.Play();
    }

    public void Sound(){
        SoundController.music = !SoundController.music;
        soundBtnClick.Play();
    }
}
