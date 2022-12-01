using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ImportPack.Inventories;

namespace RPG.Inventory
{
    [CreateAssetMenu(menuName = ("RPG/Inventory/Drop Library"))]
    public class DropLibrary : ScriptableObject
    {
        [SerializeField]
        DropConfig[] potentialDrops;
        [SerializeField] float[] dropChancePercentage;
        [SerializeField] int[] minDrops;
        [SerializeField] int[] maxDrops;

        [System.Serializable]
        class DropConfig
        {
            public InventoryItem item;
            public float[] relativeChance; // Percetange
            public int[] minNumber;  // minDrop
            public int[] maxNumber;     //maxDrop
            public int GetRandomNumber(int level)  //the qty of each drop
            {
                if (!item.IsStackable())
                {
                    return 1;
                }
                int min = GetByLevel(minNumber, level);
                int max = GetByLevel(maxNumber, level);
                return Random.Range(min, max + 1);
            }

        }

        public struct Dropped
        {
            public InventoryItem item;
            public int number;
        }

        public IEnumerable<Dropped> GetRandomDrops(int level)
        {
            if (!ShouldRandomDrop(level))
            {
                yield break;
            }
            for (int i = 0; i < GetRandomNumberOfDrops(level); i++)
            {
                yield return GetRandomDrop(level);
            }

        }

        private int GetRandomNumberOfDrops(int level)  // maximum how many ietm drop
        {
            int min = GetByLevel(minDrops, level);
            int max = GetByLevel(maxDrops, level);
            return Random.Range(min, max);
        }

        private bool ShouldRandomDrop(int level)
        {
            float randomDrop = Random.Range(0, 100);
            Debug.Log("Dice" + randomDrop);
            Debug.Log("Chance" + dropChancePercentage);
            if (randomDrop > GetByLevel(dropChancePercentage, level))
            {
                return false;
            }
            else return true;

        }

        Dropped GetRandomDrop(int level)
        {
            var drop = SelectRandomItem(level);
            var result = new Dropped();
            result.item = drop.item;
            result.number = drop.GetRandomNumber(level);
            return result;
        }

        DropConfig SelectRandomItem(int level)
        {
            float totalChance = GetTotalChance(level);
            float randomRoll = Random.Range(0, totalChance);
            float chanceToptal = 0;
            foreach (var drop in potentialDrops)
            {
                chanceToptal += GetByLevel(drop.relativeChance, level);
                if (chanceToptal > randomRoll)
                {
                    return drop;
                }
            }
            return null;
        }

        private float GetTotalChance(int level)
        {
            float total = 0;
            foreach (var drop in potentialDrops)
            {
                total += GetByLevel(drop.relativeChance, level);
            }
            return total;
        }

        static T GetByLevel<T>(T[] values, int level)
        {
            if (values.Length == 0)
            {
                return default;
            }
            if (level > values.Length)   // 如果等級比矩陣還高很多， 返回矩陣最大的值減1
            {
                return values[values.Length - 1];
            }
            if (level <= 0)
            {
                return default;
            }
            return values[level - 1];  //沒有以上情況，返回等級減1 的VALUE值
        }
    }
}