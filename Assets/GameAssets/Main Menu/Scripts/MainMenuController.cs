using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    public RuntimeAnimatorController modalBeforeAnimation;
    public RuntimeAnimatorController modalAfterAnimation;
    public GameObject soundBtnClick;
    public GameObject settingsModal;
    [SerializeField]
    GameObject playButton;
    public string scene;
    public Color loadToColor = Color.black;
    private bool clickPlay = false;
    public GameObject sound;
    public Sprite soundOn;
    public Sprite soundOff;
 
    Vector3 pos;
    public Text question;
    public Text answer;
    private int number;
    public Button reset;

    private void Start() {
        int sayi1 = Random.Range(0,10);
        int sayi2 = Random.Range(0,10);
        number = sayi1 + sayi2;
        question.text = sayi1 + " + " + sayi2 + " = ";
        clickPlay = false;
        pos = playButton.transform.position;

        if (PlayerPrefs.HasKey("level1"))
        {
            Save.getData();
        }
    }
    private void Update() {
        if (SoundController.music == true)
        {
            sound.GetComponent<Image>().sprite = soundOn;
        }
        else{
            sound.GetComponent<Image>().sprite = soundOff;
        }
        if (clickPlay)
        {
            if (pos.y > 1000)
            {
                clickPlay = false;
            }
            pos += Vector3.up * Time.deltaTime * 2;
            playButton.transform.position = pos + transform.right * Mathf.Sin(Time.time * 10f) * 0.4f;
        }
    }
    public void textboxControl(){
        if (answer.text == number.ToString())
        {
            reset.interactable = true;
        }
    }

    public void OpenGameMenu(){
        pos = playButton.transform.position;
        clickPlay = true;
        Initiate.Fade(scene, loadToColor, 1f);
        soundBtnClick.GetComponent<AudioSource>().Play();
    }

    public void Sound(){
        SoundController.music = !SoundController.music;
        soundBtnClick.GetComponent<AudioSource>().Play();
    }

    public void openSettingsModal(){
        settingsModal.GetComponent<Animator>().runtimeAnimatorController = modalBeforeAnimation;
        soundBtnClick.GetComponent<AudioSource>().Play();
    }

    public void closeSettingsModal(){
        settingsModal.GetComponent<Animator>().runtimeAnimatorController = modalAfterAnimation;
        soundBtnClick.GetComponent<AudioSource>().Play();
    }

    public void ResetGame(){
        Save.resetGame();
        soundBtnClick.GetComponent<AudioSource>().Play();
        closeSettingsModal();
    }
}
