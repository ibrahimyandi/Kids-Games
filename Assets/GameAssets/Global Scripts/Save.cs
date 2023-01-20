using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Save : MonoBehaviour
{
    public static void getData(){
        LevelController.level1Star = PlayerPrefs.GetInt("level1");
        LevelController.level2Star = PlayerPrefs.GetInt("level2");
        LevelController.level3Star = PlayerPrefs.GetInt("level3");
        LevelController.level4Star = PlayerPrefs.GetInt("level4");
        LevelController.level5Star = PlayerPrefs.GetInt("level5");
        LevelController.level6Star = PlayerPrefs.GetInt("level6");
    }
    public static void saveData(){
        PlayerPrefs.SetInt("level1", LevelController.level1Star);
        PlayerPrefs.SetInt("level2", LevelController.level2Star);
        PlayerPrefs.SetInt("level3", LevelController.level3Star);
        PlayerPrefs.SetInt("level4", LevelController.level4Star);
        PlayerPrefs.SetInt("level5", LevelController.level5Star);
        PlayerPrefs.SetInt("level6", LevelController.level6Star);
        PlayerPrefs.Save();
    }

    public static void resetGame(){
        PlayerPrefs.DeleteAll();
        LevelController.level1Star = 0;
        LevelController.level2Star = 0;
        LevelController.level3Star = 0;
        LevelController.level4Star = 0;
        LevelController.level5Star = 0;
        LevelController.level6Star = 0;
    }
}