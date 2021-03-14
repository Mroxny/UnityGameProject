﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Advertisements;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;
using UnityEngine.Assertions.Must;
using UnityEngine.Events;

public class MainMenu_SM : MonoBehaviour {
   
    public GameObject mainMenu;
    public GameObject playerSelect;
    public GameObject settings;
    public GameObject single_ContinuePanel;
    public GameObject loadingScreen;
    int characterId;
    public GameObject[] objects;
    public AudioMixer audioMixer;
    public Animator animator;

    public event Action<int> OnColorChange;

    void Awake() {
#if UNITY_IOS
        Advertisement.Initialize("3835253", false);
#elif UNITY_ANDROID
        Advertisement.Initialize("3835252", false);
#endif
        //objects = GameObject.FindGameObjectsWithTag("colorchange");
        Application.targetFrameRate = 60;

    }


    // Start is called before the first frame update
    public void Start() {
        mainMenu.gameObject.SetActive(true);
        playerSelect.gameObject.SetActive(false);
        single_ContinuePanel.gameObject.SetActive(false);
        settings.gameObject.SetActive(false);
        loadingScreen.gameObject.SetActive(false);
        
    }

    public void NewGame() {
        Start();
        PlayerPrefs.DeleteKey("ModeId");
        PlayerPrefs.DeleteKey("GameId");
        PlayerPrefs.DeleteKey("CharacterId");
    }

        public void ContinueSingle() {
        GetComponent<LevelLoader>().LoadLevel(2);
    }

        public void LoadLevelSingle() {
        if (!PlayerPrefs.HasKey("GameId")) {
            PlayerPrefs.SetInt("CharacterId", characterId);
        }
        else {

        }
        mainMenu.gameObject.SetActive(false);
        playerSelect.gameObject.SetActive(false);
        single_ContinuePanel.gameObject.SetActive(false);
        settings.gameObject.SetActive(false);
        loadingScreen.gameObject.SetActive(true);
        PlayerPrefs.SetInt("ModeId", 1);
        GetComponent<LevelLoader>().LoadLevel(1);
    }
    public void LoadLevelMulti() {
        mainMenu.gameObject.SetActive(false);
        playerSelect.gameObject.SetActive(false);
        single_ContinuePanel.gameObject.SetActive(false);
        settings.gameObject.SetActive(false);
        loadingScreen.gameObject.SetActive(true);
        GetComponent<LevelLoader>().LoadLevel(2);
    }


    public void PlaySingle() {
        if (PlayerPrefs.HasKey("GameId")) {
            mainMenu.gameObject.SetActive(false);
            playerSelect.gameObject.SetActive(false);
            single_ContinuePanel.gameObject.SetActive(true);
            if (PlayerPrefs.GetInt("ModeId") == 1) {
                single_ContinuePanel.transform.Find("Continue").gameObject.SetActive(false);
            }
            settings.gameObject.SetActive(false);
        }
        else {
            playerSelect.transform.GetChild(1).transform.GetChild(1).gameObject.SetActive(false);
            playerSelect.transform.GetChild(2).transform.GetChild(1).gameObject.SetActive(false);
            playerSelect.transform.GetChild(3).transform.GetChild(1).gameObject.SetActive(false);
            playerSelect.transform.GetChild(4).transform.GetChild(1).gameObject.SetActive(false);
            playerSelect.transform.GetChild(5).gameObject.SetActive(false);           //GetComponent<Button>().interactable = false;
            mainMenu.gameObject.SetActive(false);
            playerSelect.gameObject.SetActive(true);
            animator.SetTrigger("PlayCharacterSelect");
            characterId = 0;
        }
    }
    public void PlayMulti() {
        LoadLevelMulti();
    }
    public void SetCharacter(int id) {
        FindObjectOfType<MainMenu_SM>().characterId = id;
        playerSelect.transform.GetChild(5).gameObject.SetActive(true); //GetComponent<Button>().interactable = true;
        switch (id) {
            case 1:
                playerSelect.transform.GetChild(1).transform.GetChild(1).gameObject.SetActive(true);
                playerSelect.transform.GetChild(2).transform.GetChild(1).gameObject.SetActive(false);
                playerSelect.transform.GetChild(3).transform.GetChild(1).gameObject.SetActive(false);
                playerSelect.transform.GetChild(4).transform.GetChild(1).gameObject.SetActive(false);
                break;
            case 2:
                playerSelect.transform.GetChild(1).transform.GetChild(1).gameObject.SetActive(false);
                playerSelect.transform.GetChild(2).transform.GetChild(1).gameObject.SetActive(true);
                playerSelect.transform.GetChild(3).transform.GetChild(1).gameObject.SetActive(false);
                playerSelect.transform.GetChild(4).transform.GetChild(1).gameObject.SetActive(false);
                break;
            case 3:
                playerSelect.transform.GetChild(1).transform.GetChild(1).gameObject.SetActive(false);
                playerSelect.transform.GetChild(2).transform.GetChild(1).gameObject.SetActive(false);
                playerSelect.transform.GetChild(3).transform.GetChild(1).gameObject.SetActive(true);
                playerSelect.transform.GetChild(4).transform.GetChild(1).gameObject.SetActive(false);
                break;
            case 4:
                playerSelect.transform.GetChild(1).transform.GetChild(1).gameObject.SetActive(false);
                playerSelect.transform.GetChild(2).transform.GetChild(1).gameObject.SetActive(false);
                playerSelect.transform.GetChild(3).transform.GetChild(1).gameObject.SetActive(false);
                playerSelect.transform.GetChild(4).transform.GetChild(1).gameObject.SetActive(true);
                break;
        }
        
    }

   

    public void SetVolume(float volume) {
        audioMixer.SetFloat("Volume", volume);
    }

    
    public void Setcolor(int color) {
        OnColorChange?.Invoke( color );
        PlayerPrefs.SetInt("ColorId",color);

    }

    

    public void Settings() {
        mainMenu.gameObject.SetActive(false);
        playerSelect.gameObject.SetActive(false);
        single_ContinuePanel.gameObject.SetActive(false);
        settings.gameObject.SetActive(true);
    }
    public void Quit() {
        Application.Quit();
    }
  
    
  
    void Update() {
        
;    }
}
