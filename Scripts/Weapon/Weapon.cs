using UnityEngine;
using UnityEngine.Events;


namespace RPG.Combat
{
    public class Weapon : MonoBehaviour
    {
        [SerializeField] UnityEvent onHitVFX;

        public void onHit()
        {

            onHitVFX.Invoke();

        }
    }
}
