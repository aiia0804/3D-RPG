﻿using UnityEngine;
using UnityEngine.UI;
using System;

namespace RPG.Stats

{
    public class ExperienceDisplay:MonoBehaviour
    {
        Experience experience;
        Text text;
        void Awake()
        {
            experience = GameObject.FindWithTag("Player").GetComponent<Experience>();
            text = GetComponent<Text>();

        }

        void Update()
        {
            text.text = String.Format("EXP: {0:0}", experience.ReturnExperiencePoints());
        }


    }
}
