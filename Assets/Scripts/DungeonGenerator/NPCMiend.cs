using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class NPCMiend : MonoBehaviour
{
    [SerializeField] private TMP_Text textUI;
    [SerializeField] private GameObject canvas;
    [SerializeField] private string[] dialogs;
    private Camera camera;

    private void Start()
    {
        camera = Camera.main;
    }

    private void Update()
    {
        canvas.transform.LookAt(camera.transform);
    }

    public void ShowDialog()
    {
        StartCoroutine(nameof(Show));
    }

    IEnumerator Show()
    {
        while (this)
        {
            textUI.text = dialogs[Random.Range(0, dialogs.Length - 1)]; 
            textUI.gameObject.SetActive(true);
            yield return new WaitForSeconds(5f);
            
            textUI.gameObject.SetActive(false);
            yield return new WaitForSeconds(5f);
        }
    }
    
}
