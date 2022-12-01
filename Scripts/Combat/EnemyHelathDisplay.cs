using System;
using UnityEngine;
using UnityEngine.UI;
using RPG.Attributes;

namespace RPG.Combat
{

    public class EnemyHelathDisplay : MonoBehaviour
    {
        Fighter target;
        Text text;

        void Awake()
        {
            target = GameObject.FindWithTag("Player").GetComponent<Fighter>();
            text=GetComponent<Text>();

        }

        void Update()
        {            
            if (target.GetTarget() == null) 
            {
                text.text = "N/A";
            }

            Health health = target.GetTarget();
            if (health == null) { return; }

            text.text = String.Format("Enemy: {0:0}/{1:0}", health.returnHPpoints(),health.returnMaxHP());

        }
    }
}
