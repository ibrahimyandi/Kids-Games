using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Game4Controller : MonoBehaviour
{
    public RuntimeAnimatorController firstAnim;
    public RuntimeAnimatorController secondAnim;
    public RuntimeAnimatorController firstLevelAlertAnim;
    public RuntimeAnimatorController secondLevelAlertAnim;
    public Sprite starOn;
    public Sprite starOff;
    public AudioClip correctAnswerSound;
    public AudioClip wrongAnswerSound;
    public AudioSource soundBtnClick;
    public AudioSource soundLevelAlert;
    public AudioSource soundAnswer;
    public GameObject gameObj;
    public GameObject cloneObj;
    public LayerMask draggableMask;
    public LayerMask dropableMask;
    public GameObject nextButton;
    GameObject selectedObject;
    public bool isDragging;
    public Vector2 offset;
    public List<Sprite> airAnimal;
    public List<Sprite> landAnimal;
    public List<Sprite> seaAnimal;
    public GameObject sound;
    public Sprite soundOn;
    public Sprite soundOff;
    public Text scoreText;
    public Text timerText;
    public GameObject levelAlert;
    public AudioClip threeStar;
    public AudioClip twoStar;
    public AudioClip oneStar;
    public AudioClip zeroStar;
    public GameObject starsObj;
    private int wrongAnswer = 0;
    private int globalScore = 0;
    private float time;
    private bool stopTimer = false;
    public int level = 0;
    private void Start() {
        RestartGame();
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
        if (gameObj)
        {
            if (gameObj.GetComponent<Animator>().runtimeAnimatorController != null){
                if(gameObj.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime > 1){
                    gameObj.GetComponent<Animator>().runtimeAnimatorController = null;
                    gameObj.transform.position = new Vector2(0, 0);
                    gameObj.GetComponent<ObjectAnimal>().move = true;
                    gameObj.GetComponent<ObjectAnimal>().targetPos = gameObj.transform.position;

                }
            }
        }
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity, draggableMask);
            if (hit.collider != null)
            {
                if (hit.collider.gameObject.GetComponent<ObjectAnimal>().draggable == true)
                {
                    selectedObject = hit.collider.gameObject;
                    offset = hit.collider.gameObject.transform.position - ray.origin;
                    isDragging = true;
                }
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
            if (selectedObject)
            {
                CheckForDrop();
            }
        }
        if (isDragging)
        {
            Vector2 pos = mousePos();
            selectedObject.transform.position = pos + offset;
        }
    }

    public Vector2 mousePos(){
        return Camera.main.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
    }

    public IEnumerator destroyAnimal(){
        gameObj.GetComponent<Animator>().runtimeAnimatorController = secondAnim;
        yield return new WaitForSeconds(0.3f);
        Destroy(gameObj);

        if (globalScore == 5)
        {
            int localStar = 0;
            if (time <= 10)
            {
                LevelController.level4Star = 3;
                levelAlert.GetComponent<AudioSource>().clip = threeStar;
                localStar = 3;
            }
            else if(time <= 15)
            {
                levelAlert.GetComponent<AudioSource>().clip = twoStar;
                localStar = 2;
                if (LevelController.level4Star < 2)
                {
                    LevelController.level4Star = 2;
                }
            }
            else if(time <= 25){
                localStar = 1;
                levelAlert.GetComponent<AudioSource>().clip = oneStar;
                if (LevelController.level4Star < 1)
                {
                    LevelController.level4Star = 1;
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
            if (LevelController.level4Star == 0)
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
            gameObj.transform.position = new Vector2(-20f, 0f);
        }
        else{
            nextAnimal();
        }
    }

    public void CheckForDrop(){
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction , Mathf.Infinity, dropableMask);
        if (hit.collider != null)
        {
            if (selectedObject.GetComponent<ObjectAnimal>().index == hit.collider.GetComponent<Region>().index)
            {
                selectedObject.GetComponent<ObjectAnimal>().draggable = false;
                selectedObject.GetComponent<ObjectAnimal>().targetPos = hit.collider.transform.position;
                if (wrongAnswer < 2)
                {
                    globalScore++;
                }
                wrongAnswer = 0;
                scoreText.text = globalScore.ToString();
                soundAnswer.clip = correctAnswerSound;
                soundAnswer.Play();
                StartCoroutine(destroyAnimal());
            }
            else
            {
                selectedObject.GetComponent<ObjectAnimal>().targetPos = selectedObject.GetComponent<ObjectAnimal>().firstPosition;
                wrongAnswer++;
                soundAnswer.clip = wrongAnswerSound;
                soundAnswer.Play();
            }
        }
        else
        {
            selectedObject.GetComponent<ObjectAnimal>().targetPos = selectedObject.GetComponent<ObjectAnimal>().firstPosition;
            wrongAnswer++;
            soundAnswer.clip = wrongAnswerSound;
            soundAnswer.Play();
        }
        selectedObject = null;
    }
    public IEnumerator animalFirstAnim(){
        gameObj.GetComponent<Animator>().runtimeAnimatorController = firstAnim;
        yield return new WaitForSeconds(1f);
        gameObj.GetComponent<Animator>().runtimeAnimatorController = null;
    }
    public void nextAnimal(){
        if (level > 4)
        {
            level = 0;
        }
        gameObj = GameObject.Instantiate(cloneObj, cloneObj.transform.parent);
        gameObj.AddComponent<ObjectAnimal>();
        int randomRegion = Random.Range(0,3);
        gameObj.GetComponent<ObjectAnimal>().index = randomRegion;
        StartCoroutine(animalFirstAnim());

        gameObj.GetComponent<ObjectAnimal>().targetPos = new Vector2(0, 0);
        //StartCoroutine(animalFirstAnim());
        if (randomRegion == 0)
        {
            gameObj.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = airAnimal[level];
        }
        else if (randomRegion == 1)
        {
            gameObj.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = landAnimal[level];
        }
        else
        {
            gameObj.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = seaAnimal[level];
        }
        level++;
    }
    public void RestartGame(){
        level = 0;
        if (globalScore != 0)
        {
            soundBtnClick.Play();
            levelAlert.GetComponent<Animator>().runtimeAnimatorController = secondLevelAlertAnim;
        }
        globalScore = 0;
        time = 0;
        scoreText.text = globalScore.ToString();
        
        timerText.text = time.ToString();
        stopTimer = false;
        nextAnimal();
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
