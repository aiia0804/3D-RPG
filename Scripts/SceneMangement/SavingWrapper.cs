using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameDevTV.Saving;
using System;

namespace RPG.SceneMagement
{
    public class SavingWrapper : MonoBehaviour
    {
        private void Awake()
        {
            StartCoroutine(LoadLastScene());

        }
      
        public IEnumerator LoadLastScene()
        {
            
            yield return GetComponent<SavingSystem>().LoadLastScene(defaultSaveFile);
            Fader fader = FindObjectOfType<Fader>();
            fader.FadeOutImmidately();
            yield return fader.FadeIn(1.5f);

        }
        const string defaultSaveFile = "save";
      void Update()
        {
            if (Input.GetKeyDown(KeyCode.F11))
            {
                Save();
            }
            if (Input.GetKeyDown(KeyCode.F12))
            {
                Load();
            }

        }

        public void Save()
        {
                GetComponent<SavingSystem>().Save(defaultSaveFile);        
        }

        public void Load()
        {
                GetComponent<SavingSystem>().Load(defaultSaveFile);
        }
    }
}