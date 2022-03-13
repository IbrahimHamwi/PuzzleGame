using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelectButton : MonoBehaviour
{
    public string levelToLoad;//levelToLoad is the name of the scene that the button will load
    public GameObject star1, star2, star3;//star1 is one star, star2 is two stars, star3 is three stars
    void Start()
    {
        star1.SetActive(false);
        star2.SetActive(false);
        star3.SetActive(false);

        if (PlayerPrefs.HasKey(levelToLoad + "_Star1"))
        {
            star1.SetActive(true);
        }
        if (PlayerPrefs.HasKey(levelToLoad + "_Star2"))
        {
            star2.SetActive(true);
        }
        if (PlayerPrefs.HasKey(levelToLoad + "_Star3"))
        {
            star3.SetActive(true);
        }
    }

    public void LoadLevel()
    {
        SceneManager.LoadScene(levelToLoad);//Loads the scene that the button will load
    }
}
