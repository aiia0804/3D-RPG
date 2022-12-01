using ImportPack.Inventories;
using UnityEngine;
using RPG.Stats;
using System.Collections.Generic;

namespace RPG.Inventory
{
    [CreateAssetMenu(menuName = ("RPG/INVENTORY/State EQUIPABLE ITEM"))]
    public class StateEquipableItem : EquipableItem, IModifierProvider
    {
        [SerializeField]
        Modifier[] additveModifiers;
        [SerializeField]
        Modifier[] percentageModifiers;


        [System.Serializable]
        struct Modifier
        {
            public Stat stat;
            public float value;
        }

        public IEnumerable<float> GetAdditiveModifier(Stat stat)
        {
            foreach (var modifier in additveModifiers)
            {
                if (modifier.stat == stat)
                {
                    yield return modifier.value;
                }
            }
        }

        public IEnumerable<float> GetPercintageModifier(Stat stat)
        {
            foreach (var modifier in percentageModifiers)
            {
                if (modifier.stat == stat)
                {
                    yield return modifier.value;
                }
            }
        }
    }
}
