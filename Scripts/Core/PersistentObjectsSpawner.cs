using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    public class PersistentObjectsSpawner : MonoBehaviour
    {
        [SerializeField] GameObject persistentObjectPrefeb;

        static bool hasSpawned = false;
        private void Awake()
        {
            if (hasSpawned) { return; }


            SpawnPersistentObjects();
            hasSpawned = true;
        }

        private void SpawnPersistentObjects()
        {
            GameObject persistentObject = Instantiate(persistentObjectPrefeb);
            DontDestroyOnLoad(persistentObject);
        }
    }

}
