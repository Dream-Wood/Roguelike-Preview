using System;
using UnityEngine;

    public class PlayerAnimator : MonoBehaviour
    {
        [SerializeField] private Animator characterAnimator;
        [SerializeField] private Animator rigAnimator;
        [SerializeField] private Movement movement;

        private Player _player;

        private void OnEnable()
        {
            _player = FindObjectOfType<Player>();
            
            movement.Move += OnMove;
            movement.Attack += OnAttack;

            _player.changeStats += OnChangeStats;
        }

        private void OnDisable()
        {
            movement.Move -= OnMove;
            movement.Attack -= OnAttack;
            
            _player.changeStats -= OnChangeStats;
        }

        private void OnMove(Vector2 val)
        {
            characterAnimator.SetFloat("MoveY", val.y);
            characterAnimator.SetFloat("MoveX", val.x);
        }

        private void OnAttack(bool val)
        {
            rigAnimator.SetBool("Attack", val);
        }
        
        private void OnChangeStats(PlayerStats stats)
        {
            rigAnimator.SetFloat("AttackSpeed", stats.AttackSpeed);
        }
    }

