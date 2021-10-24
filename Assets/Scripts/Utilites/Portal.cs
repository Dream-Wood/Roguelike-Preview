using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Player>())
        {
            other.GetComponent<CharacterController>().enabled = false;
            other.transform.position = new Vector3(84, 1, 84);
            other.GetComponent<CharacterController>().enabled = true;
            SceneManager.LoadSceneAsync("Loop");
            FindObjectOfType<GUIInformation>().ShowLoadingScreen();
        }
    }
}