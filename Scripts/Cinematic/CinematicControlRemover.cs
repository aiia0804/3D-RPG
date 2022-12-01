using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Playables;
using RPG.Core;
using RPG.Control;

namespace RPG.Cinematic
{
    public class CinematicControlRemover : MonoBehaviour
    {
        private void OnEnable()
        {
            GetComponent<PlayableDirector>().played += DisableController;
            GetComponent<PlayableDirector>().stopped += EnableController;
        }

        private void OnDisable()
        {
            GetComponent<PlayableDirector>().played -= DisableController;
            GetComponent<PlayableDirector>().stopped -= EnableController;
        }

        private void DisableController(PlayableDirector d)
        {
            GameObject player = GameObject.FindWithTag("Player");
            player.GetComponent<ActionScheduler>().CancelCurrentAction();
            player.GetComponent<PlayerController>().enabled = false;
        }

        private void EnableController(PlayableDirector d)
        {
            GameObject player = GameObject.FindWithTag("Player");
            player.GetComponent<PlayerController>().enabled = true;


        }


    }
}
