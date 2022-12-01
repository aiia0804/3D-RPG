using System;
using UnityEngine;
using UnityEngine.UI;


namespace RPG.Stats
{
    public class LevelDisplay : MonoBehaviour
    {
        BasicStats basicStats;
        Text text;
        void Awake()
        {
            basicStats = GameObject.FindWithTag("Player").GetComponent<BasicStats>();
            text = GetComponent<Text>();

        }

        void Update()
        {
            text.text = String.Format("Level: {0:0}", basicStats.CalculateLevel());
        }
    }

}
