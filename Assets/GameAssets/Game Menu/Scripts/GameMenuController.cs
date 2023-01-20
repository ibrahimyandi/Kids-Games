using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMenuController : MonoBehaviour
{
    public Color loadToColor = Color.black;
    public GameObject soundBtnClick;
    public GameObject sound;
    public Sprite soundOn;
    public Sprite soundOff;

    private void Update() {
        if (SoundController.music == true)
        {
            sound.GetComponent<Image>().sprite = soundOn;
        }
        else{
            sound.GetComponent<Image>().sprite = soundOff;
        }
    }

    public void GoMainMenu(){
        Initiate.Fade("Main Menu", loadToColor, 1f);
        soundBtnClick.GetComponent<AudioSource>().Play();
    }
    public void OpenGame1(){
        Initiate.Fade("Game1", loadToColor, 1f);
        soundBtnClick.GetComponent<AudioSource>().Play();
    }
    public void OpenGame2(){
        Initiate.Fade("Game2", loadToColor, 1f);
        soundBtnClick.GetComponent<AudioSource>().Play();
    }
    public void OpenGame3(){
        Initiate.Fade("Game3", loadToColor, 1f);
        soundBtnClick.GetComponent<AudioSource>().Play();
    }
    public void OpenGame4(){
        Initiate.Fade("Game4", loadToColor, 1f);
        soundBtnClick.GetComponent<AudioSource>().Play();
    }
    public void OpenGame5(){
        Initiate.Fade("Game5", loadToColor, 1f);
        soundBtnClick.GetComponent<AudioSource>().Play();
    }
    public void OpenGame6(){
        Initiate.Fade("Game6", loadToColor, 1f);
        soundBtnClick.GetComponent<AudioSource>().Play();
    }

    public void Sound(){
        SoundController.music = !SoundController.music;
        soundBtnClick.GetComponent<AudioSource>().Play();
    }
}
