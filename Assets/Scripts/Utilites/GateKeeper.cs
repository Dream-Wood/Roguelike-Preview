using System;
using System.Collections;
using System.Collections.Generic;
using DungeonGenerator;
using UnityEngine;

public class GateKeeper : MonoBehaviour
{
    [SerializeField] private Room currentRoom;
    [SerializeField] private Animator animator;
    [SerializeField] private Vector3 shift;
    private bool _gateOpen;
    private void OnEnable()
    {
        animator.Play("Open");
        currentRoom.RoomIsClear += IsClear;
    }

    private void OnDisable()
    {
        currentRoom.RoomIsClear -= IsClear;
    }

    private void IsClear(bool val)
    {
        if (val)
        {
            animator.Play("Open");
        }
        else
        {
            animator.Play("Close");
        }
    }
}
