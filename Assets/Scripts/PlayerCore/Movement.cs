using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    public delegate void MoveAction(Vector2 val);

    public event MoveAction Move;

    public delegate void AttackAction(bool val);

    public event AttackAction Attack;
    
    public delegate void Pause();

    public event Pause pause;

    [SerializeField] private Player _player;

    private float _speed = 1;
    private Vector3 _direction;
    private Vector2 _move;
    private Camera _cameraMain;
    private CharacterController _controller;
    private float _gravity = 9.8f;

    void OnEnable()
    {
        _cameraMain = Camera.main;
        _controller = GetComponent<CharacterController>();

        _player.changeStats += OnChangeStats;
    }

    void OnDisable()
    {
        _player.changeStats -= OnChangeStats;
    }

    public void OnMovement(InputAction.CallbackContext value)
    {
        _move = value.ReadValue<Vector2>();
        Move?.Invoke(_move);
        _direction = new Vector3(_move.x, 0f, _move.y).normalized;
    }

    public void OnAttack(InputAction.CallbackContext value)
    {
        Attack?.Invoke(value.ReadValueAsButton());
    }

    public void OnUseHeal(InputAction.CallbackContext value)
    {
        if (value.canceled)
        {
            _player.UseHealPotion();
        }
    }
    
    public void OnPause(InputAction.CallbackContext value)
    {
        if (value.canceled)
        {
            pause?.Invoke();
        }
    }

    private void OnChangeStats(PlayerStats stats)
    {
        _speed = stats.Speed;
    }

    private void FixedUpdate()
    {
        if (_direction.magnitude >= 0.005f)
        {
            _controller.Move(_direction * _speed * Time.deltaTime * 5);
            Quaternion targetRotation = Quaternion.LookRotation(_direction);
            Quaternion playerRotation = Quaternion.Slerp(transform.rotation, targetRotation, 15 * Time.deltaTime);
            transform.rotation = playerRotation;
        }
    }

    private void LateUpdate()
    {
        RaycastHit hit;
        Vector3 org = new Vector3(transform.position.x, transform.position.y + .05f, transform.position.z);
        if (!Physics.Raycast(org, -transform.up, out hit, .1f))
        {
            _controller.Move(-transform.up * _gravity * Time.deltaTime * 2);
        }
    }
}