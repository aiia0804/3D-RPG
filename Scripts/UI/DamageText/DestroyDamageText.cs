using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.UI.DamageText
{
    public class DestroyDamageText : MonoBehaviour
    {
        [SerializeField] GameObject toDestroy = null;

        public void DestroyTarget()
        {
            Destroy(toDestroy);
        }

    }
}



