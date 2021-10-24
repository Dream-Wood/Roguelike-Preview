using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleCameraTraking : MonoBehaviour
{
    [SerializeField] private Vector3 followOffset;
    [SerializeField] private float followSpeed = 5f;
    private Transform _player;
    private Vector3 _targetPos;

    private void Start()
    {
        _player = FindObjectOfType<Player>().transform;
    }
    private void Update()
    {
        _targetPos = Vector3.Lerp(transform.position , _player.position + followOffset, Time.deltaTime * followSpeed);
        transform.position = _targetPos;
    }
}
