using ImportPack.Inventories;
using RPG.Stats;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Inventory
{
    public class RandomDropper : ItemDropper
    {
        [Tooltip("How far can the pickups be scattered from the droper")]
        [SerializeField] float scatterDistance = 1;
        [SerializeField] DropLibrary dropLibary;

        const int attempt = 30;

        public void RandomDrop()
        {
            var baseStats = GetComponent<BasicStats>();
            var drops = dropLibary.GetRandomDrops(baseStats.GetLevel());
            foreach (var drop in drops)
            {
                DropItem(drop.item, drop.number);
            }
        }

        protected override Vector3 GetDropLocation()
        {

            for (int i = 0; i < attempt; i++)
            {
                Vector3 randomPoint = transform.position + Random.insideUnitSphere * scatterDistance;
                NavMeshHit hit;
                if (NavMesh.SamplePosition(randomPoint, out hit, 0.1f, NavMesh.AllAreas))
                {
                    return hit.position;
                }
            }
            return transform.position;
        }
    }
}
