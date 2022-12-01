using UnityEngine;
using UnityEngine.UI;
using System;

namespace RPG.Attributes
{
    public class HealthDisplay : MonoBehaviour

    {
        Health health;
        Text text;
        void Awake()
        {
            health = GameObject.FindWithTag("Player").GetComponent<Health>();
            text = GetComponent<Text>();
        }

        void Update()
        {
            text.text = String.Format("Player: {0:0}/{1:0}", health.returnHPpoints(), health.returnMaxHP());
        }
    }

}
