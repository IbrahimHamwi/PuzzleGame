using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public TMP_Text scoreText, timerText, winScore, winText;
    public GameObject winstars1, winstars2, winstars3;//winstars is one star, winstars2 is two stars, winstars3 is three stars
    public GameObject roundOverScreen;


    void Start()
    {
        winstars1.SetActive(false);
        winstars2.SetActive(false);
        winstars3.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
