using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Game1Controller : MonoBehaviour
{
    public RuntimeAnimatorController firstAnim;
    public RuntimeAnimatorController secondAnim;
    public RuntimeAnimatorController firstLevelAlertAnim;
    public RuntimeAnimatorController secondLevelAlertAnim;
    public AudioSource soundBtnClick;
    public AudioSource soundLevelAlert;
    public AudioSource soundAnswer;
    public AudioClip threeStar;
    public AudioClip twoStar;
    public AudioClip oneStar;
    public AudioClip zeroStar;
    public AudioClip correctAnswerSound;
    public AudioClip wrongAnswerSound;
    public AudioClip soundBtnClickClip;
    public GameObject nextButton;
    public Sprite starOn;
    public Sprite starOff;
    public GameObject starsObj;
    public GameObject sound;
    public Text scoreText;
    public Text timerText;
    public GameObject gameObj;
    public List<Sprite> slots;
    public List<Sprite> shape;
    public List<GeometricObject> objects;
    public List<GeometricObject> objectZones;
    public GameObject levelAlert;
    public Sprite soundOn;
    public Sprite soundOff;
    public LayerMask draggableMask;
    public LayerMask dropableMask;
    GameObject selectedObject;
    private bool isDragging;
    public Vector2 offset;
    private int complateObj = 0;
    private int wrongAnswer = 0;
    private int globalScore = 0;
    private float time;
    private bool stopTimer = false;

    private void Start() {
        StartCoroutine(StartAnimation());
        isDragging = false;
    }
    
    void Update()
    {
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

        if (complateObj == 3)
        {
            gameObj.GetComponent<Animator>().runtimeAnimatorController = secondAnim;

            foreach (GeometricObject obj in objects)
            {
                obj.transform.position = new Vector2(obj.GetComponent<GeometricObject>().targetPos.x, obj.GetComponent<GeometricObject>().targetPos.y);
            }
            
            if (wrongAnswer < 2)
            {
                globalScore++;
            }
            complateObj = 0;
            wrongAnswer = 0;
            scoreText.text = globalScore.ToString();
            StartCoroutine(StartAnimation());
            
        }
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity, draggableMask);
            if (hit.collider != null)
            {
                if (hit.collider.gameObject.GetComponent<GeometricObject>().draggable == true)
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
    public IEnumerator StartAnimation(){
        foreach (GeometricObject obj in objects)
        {
            obj.GetComponent<GeometricObject>().animationMove = true;
        }
        foreach (GeometricObject obj in objectZones)
        {
            obj.GetComponent<GeometricObject>().animationMove = true;
        }
        yield return new WaitForSeconds(1f);
        foreach (GeometricObject obj in objects)
        {
            obj.transform.position = new Vector2(obj.transform.position.x, -3f);
            obj.targetPos = obj.firstPosition;
            obj.GetComponent<GeometricObject>().draggable = true;
        }
        foreach (GeometricObject obj in objectZones)
        {
            obj.targetPos = obj.firstPosition;
        }
        if (globalScore == 5)
        {
            int localStar = 0;
            if (time <= 20)
            {
                LevelController.level1Star = 3;
                levelAlert.GetComponent<AudioSource>().clip = threeStar;
                localStar = 3;
            }
            else if(time <= 25)
            {
                levelAlert.GetComponent<AudioSource>().clip = twoStar;
                localStar = 2;
                if (LevelController.level1Star < 2)
                {
                    LevelController.level1Star = 2;
                }
            }
            else if(time <= 30){
                localStar = 1;
                levelAlert.GetComponent<AudioSource>().clip = oneStar;
                if (LevelController.level1Star < 1)
                {
                    LevelController.level1Star = 1;
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
            if (LevelController.level1Star == 0)
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
            refreshGame();
            gameObj.GetComponent<Animator>().runtimeAnimatorController = firstAnim;
        }
    }
    public Vector2 mousePos(){
        return Camera.main.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
    }

    public void CheckForDrop(){
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction , Mathf.Infinity, dropableMask);
        if (hit.collider != null)
        {
            if (selectedObject.GetComponent<GeometricObject>().index == hit.collider.GetComponent<GeometricObject>().index)
            {
                selectedObject.GetComponent<GeometricObject>().draggable = false;
                selectedObject.GetComponent<GeometricObject>().targetPos = hit.collider.transform.position;
                complateObj++;
                soundAnswer.clip = correctAnswerSound;
                soundAnswer.Play();
            }
            else
            {
                wrongAnswer++;
                soundAnswer.clip = wrongAnswerSound;
                soundAnswer.Play();
                selectedObject.GetComponent<GeometricObject>().targetPos = selectedObject.GetComponent<GeometricObject>().firstPosition;
            }
        }
        else
        {
            wrongAnswer++;
            soundAnswer.clip = wrongAnswerSound;
            soundAnswer.Play();
            selectedObject.GetComponent<GeometricObject>().targetPos = selectedObject.GetComponent<GeometricObject>().firstPosition;
        }
        selectedObject = null;
    }

    public void refreshGame()
    {
        int[] shapeIndex = new int[objects.Count];
       
        shapeIndex[0] = Random.Range(0, slots.Count);
        do
        {
            shapeIndex[1] = Random.Range(0, slots.Count);
        } while (shapeIndex[0] == shapeIndex[1]);

        do
        {
            shapeIndex[2] = Random.Range(0, slots.Count);
        } while (shapeIndex[0] == shapeIndex[2] || shapeIndex[1] == shapeIndex[2]);

       
        for (int i = 0; i < objectZones.Count; i++)
        {
            objectZones[i].transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = slots[shapeIndex[i]];
        }
        
        
        int[] indexArr = {0, 1, 2};
        int index;
        for (int i = 0; i < 3; i++)
        {
            do
            {
                index = Random.Range(0, indexArr.Length);
            } while (!indexArr.Contains(indexArr[index]));
            objects[i].GetComponent<GeometricObject>().index = indexArr[index];
            objects[i].transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = shape[shapeIndex[objects[i].GetComponent<GeometricObject>().index]];
            indexArr = indexArr.Where(val => val != indexArr[index]).ToArray();
        }
        foreach (GeometricObject obj in objects)
        {
            obj.GetComponent<GeometricObject>().animationMove = false;
        }
        foreach (GeometricObject obj in objectZones)
        {
            obj.GetComponent<GeometricObject>().animationMove = false;
        }
    }
    public IEnumerator waitAlert(){
        yield return new WaitForSeconds(1f);
        levelAlert.SetActive(false);
    }
    public void GoGameMenu(){
        Initiate.Fade("Game Menu", Color.black, 1.5f);
        soundBtnClick.Play();
    }
    public void RestartGame(){
        soundBtnClick.Play();
        globalScore = 0;
        time = 0;
        scoreText.text = globalScore.ToString();
        levelAlert.GetComponent<Animator>().runtimeAnimatorController = secondLevelAlertAnim;
        timerText.text = time.ToString();
        stopTimer = false;
        StartCoroutine(waitAlert());
        StartCoroutine(StartAnimation());
    }
    public void GoNextGame(){
        soundBtnClick.Play();
        Initiate.Fade("Game2", Color.black, 1.5f);
    }
    public void Sound(){
        soundBtnClick.Play();
        SoundController.music = !SoundController.music;
    }
}
