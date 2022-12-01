using ImportPack.Inventories;
using UnityEngine;
using RPG.Stats;
using System.Collections.Generic;

namespace RPG.Inventory
{
    public class StateEquipment : Equipment, IModifierProvider
    {
        public IEnumerable<float> GetAdditiveModifier(Stat stat)
        {
            foreach (var slot in GetAllPopulatedSlots())
            {
                var item = GetItemInSlot(slot) as IModifierProvider;
                if (item == null) continue;
                foreach (float modifier in item.GetAdditiveModifier(stat))
                {
                    yield return modifier;
                }
            }
        }

        public IEnumerable<float> GetPercintageModifier(Stat stat)
        {
            foreach (var slot in GetAllPopulatedSlots())
            {
                var item = GetItemInSlot(slot) as IModifierProvider;
                if (item == null) continue;
                foreach (float modifier in item.GetPercintageModifier(stat))
                {
                    yield return modifier;
                }
            }
        }
    }
}
