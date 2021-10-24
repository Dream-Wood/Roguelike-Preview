using System;
using UnityEngine;

public class DamageDetector : MonoBehaviour
{
    [SerializeField] private GameObject source;
    [SerializeField] private float damageMultiply = 1;
    [SerializeField] private string tag;

    private IDamageTaker _damageTaker;
    private float _cooldown;

    private void OnEnable()
    {
        _damageTaker = source == null ? GetComponent<IDamageTaker>() : source.GetComponent<IDamageTaker>();
    }

    private void LateUpdate()
    {
        if (_cooldown > 0)
        {
            _cooldown -= Time.deltaTime;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (_cooldown <= 0)
        {
            if (other.gameObject.TryGetComponent(out ContactDamage damageInfo) && other.gameObject.CompareTag(tag))
            {
                _damageTaker.TakeDamage(damageInfo.GetDamage() * damageMultiply);
                _cooldown += .5f;
            }
        }
    }
}
