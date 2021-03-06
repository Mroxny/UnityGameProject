﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Window_MissionSelect : MonoBehaviour
{
    public Animator animator;
    public GameObject sceneMenager;


    private Camera cam;
    void OnEnable() {
        Initiate();
    }

    public void Initiate() {
        cam = Camera.main;
        gameObject.transform.Find("Canvas").GetComponent<Canvas>().worldCamera = cam;
        GameObject.Find("Player(Clone)").GetComponent<Player>().DisableHUD();
        if (sceneMenager == null) {
            sceneMenager = GameObject.Find("/SceneMenager");
        }
    }
    public void LoadLevel(int gameId) {
        PlayerPrefs.SetInt("GameId",gameId);
        PlayerPrefs.SetInt("ModeId", 1);
        sceneMenager.GetComponent<LevelLoader>().LoadLevel(2);
    }
    public void LoadMainMenu() {
        sceneMenager.GetComponent<LevelLoader>().LoadLevel(0);
    }
    public void Close() {
        animator.SetTrigger("close");
        StartCoroutine(DisableAfterTime(1.4f));
    }
    IEnumerator DisableAfterTime(float time) {
        yield return new WaitForSeconds(time);
        gameObject.SetActive(false);
        GameObject.Find("Player(Clone)").GetComponent<Player>().EnableHUD();
    }
}
