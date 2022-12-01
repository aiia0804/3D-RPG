using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Attributes
{
  public class HealthBar : MonoBehaviour
    {
        [SerializeField] Health displayTarget;
        [SerializeField] RectTransform foreground;
        [SerializeField] Canvas canvas;



        void Update()
        {
            float HPBarValue = displayTarget.returnHealthPercentage() / 100;


            if (Mathf.Approximately(HPBarValue,0)||(Mathf.Approximately(HPBarValue,1)))
            {
                
                canvas.GetComponent<Canvas>().enabled = false;
                return;     
            }

            canvas.GetComponent<Canvas>().enabled = true;
           
            foreground.localScale = new Vector3(HPBarValue, 1, 1);
            
        }

    }

}
