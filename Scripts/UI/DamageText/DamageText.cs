using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace RPG.UI.DamageText
{
    public class DamageText : MonoBehaviour
    {
        [SerializeField] Text text;

        string textToDisplay;

        void Start()
        {
            text = GetComponent<Text>();
        }


        public void GetTextValue(float textPassIn)
        {
            text.text = String.Format("{0:0}",textPassIn);
        }
    }

}

