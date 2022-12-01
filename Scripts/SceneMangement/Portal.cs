using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;
using RPG.Control;

namespace RPG.SceneMagement
{

    public class Portal : MonoBehaviour
    {
        enum DestinationIdentifier
        {
            A, B, C, D, E
        }

        [SerializeField] int sceneToLoad = -1;
        [SerializeField] Transform SpawnPoint;
        [SerializeField] DestinationIdentifier destination;
        [SerializeField] float FadeInTime = 1f;
        [SerializeField] float FadeOutTime = 1f;
        [SerializeField] float FadeWaitTime = .5f;



        Fader fader;
        bool isTransiting = true;
        private void Start()
        {
            fader = FindObjectOfType<Fader>();
        }

        private void OnTriggerEnter(Collider other)
        {


            if (other.gameObject.tag == "Player")
            {
                StartCoroutine(Transition());
            }
        }

        public bool GetStatusOfTransition()
        {
            return isTransiting;
        }



        private IEnumerator Transition()
        {
            if (sceneToLoad < 0)
            {
                Debug.LogError("Scene hsasn't setted yet");
                yield break;
            }

            DisableController();

            DontDestroyOnLoad(gameObject);

            yield return fader.FadeOut(FadeOutTime);

            SavingWrapper savingWrapper = FindObjectOfType<SavingWrapper>();
            savingWrapper.Save();
            yield return SceneManager.LoadSceneAsync(sceneToLoad);

            DisableController();
            savingWrapper.Load();

            Portal OtherPortal = GetOtherPortal();
            UpdatePlayer(OtherPortal);

            yield return new WaitForSeconds(FadeWaitTime);
            fader.FadeIn(FadeInTime);

            savingWrapper.Save();

            EnableController();
            Destroy(gameObject);
        }

        private void UpdatePlayer(Portal otherPortal)
        {

            GameObject player = GameObject.FindWithTag("Player");
            player.GetComponent<NavMeshAgent>().enabled = false;
            player.transform.position = otherPortal.SpawnPoint.transform.position;
            player.transform.rotation = otherPortal.SpawnPoint.transform.rotation;
            player.GetComponent<NavMeshAgent>().enabled = true;

        }

        private Portal GetOtherPortal()
        {
            Portal[] otherPortal = FindObjectsOfType<Portal>();

            foreach (Portal portal in otherPortal)
            {
                if (portal == this) continue;
                if (portal.destination != this.destination) continue;
                return portal;
            }
            return null;
        }

        private void DisableController()
        {
            GameObject player = GameObject.FindWithTag("Player");
            player.GetComponent<PlayerController>().enabled = false;
        }

        private void EnableController()
        {
            GameObject player = GameObject.FindWithTag("Player");
            player.GetComponent<PlayerController>().enabled = true;
        }
    }
}
