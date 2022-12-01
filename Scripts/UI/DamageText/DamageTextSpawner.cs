using UnityEngine;


namespace RPG.UI.DamageText
{
    public class DamageTextSpawner: MonoBehaviour
    {
        [SerializeField] DamageText damageTextPrefb;

  

        public void SpawnDamageText(float damageAmount) // call by health in Inspector
        {
            DamageText instance= Instantiate(damageTextPrefb, transform);
            instance.GetComponent<DamageText>().GetTextValue(damageAmount);
        }

    }
}
