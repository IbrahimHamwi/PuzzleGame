using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public static SFXManager instance;//singleton
    private void Awake()
    {
        instance = this;//set the instance to this script
    }

    public AudioSource gemSound, explosion, levelComplete, stoneBreak;//audio sources

    public void PlayGemBreak()
    {
        gemSound.Stop();
        gemSound.pitch = Random.Range(0.8f, 1.2f);
        gemSound.Play();
    }
    public void PlayExplode()
    {
        explosion.Stop();
        explosion.pitch = Random.Range(0.8f, 1.2f);
        explosion.Play();
    }
    public void StoneBreak()
    {
        stoneBreak.Stop();
        stoneBreak.pitch = Random.Range(0.8f, 1.2f);
        stoneBreak.Play();
    }
    public void PLayRoundOver()
    {
        levelComplete.Stop();
        levelComplete.pitch = Random.Range(0.8f, 1.2f);
        levelComplete.Play();
    }
}
