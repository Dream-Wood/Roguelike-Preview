using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemStatsProvider : StatsDecorator
{
    private readonly IEnumerable<Artifacts> _itemKeys;
    private readonly ItemDataBase _dataBase;

    public ItemStatsProvider(IStatsProvider wrappedEntity, IEnumerable<Artifacts> itemKeys) : base(wrappedEntity)
    {
        _itemKeys = itemKeys;
        _dataBase = Resources.Load<ItemDataBase>("Items/ItemDataBase");
        if (_dataBase==null)
        {
            _dataBase = ItemDataBase.instance;
        }
    }

    protected override PlayerStats GetStatsInternal()
    {
        var stats = new PlayerStats();
        foreach (var item in _itemKeys)
        {
            stats += _dataBase.GetStats(_wrappedEntity,item);
        }

        return _wrappedEntity.GetStats() + stats;
    }
}
