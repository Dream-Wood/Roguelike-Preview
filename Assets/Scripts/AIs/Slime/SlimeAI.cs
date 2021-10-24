using System;
using System.Collections;
using System.Collections.Generic;
using DungeonGenerator;
using UnityEngine;

public class SlimeAI : Entity, IArtificialIntelligenceInit
{
    [SerializeField] private SkinnedMeshRenderer smr;
    private Transform _playerTransform;
    private GameObject _burningSkin;
    private Vector3 _modify;
    private Room _owner;

    public void Init(Room owner)
    {
        _owner = owner;
        if (GameObject.FindWithTag("Player"))
        {
            _playerTransform = GameObject.FindWithTag("Player").transform;
        }
        _burningSkin = Resources.Load<GameObject>("VFX/BurningSkin");
    }
    
    void Update()
    {
        _modify = _playerTransform.position + Vector3.up;
        transform.LookAt(_modify);
    }

    public override void TakeDamage(Damage dmg)
    {
        base.TakeDamage(dmg);

        if (dmg.Fire)
        {
            GameObject effect = Instantiate(_burningSkin, transform.parent);
            ParticleSystem ps = effect.GetComponent<ParticleSystem>();
            Destroy(effect, dmg.BurningTime);
            var shape = ps.shape;
            shape.shapeType = ParticleSystemShapeType.SkinnedMeshRenderer;
            shape.skinnedMeshRenderer = smr;
            Destroy(effect, dmg.BurningTime);
        }

        if (hp - dmg.Value < 0)
        {
            _owner.EnemyDeath();
            Destroy(gameObject);
        }
    }
}
