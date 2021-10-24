using UnityEngine;

[CreateAssetMenu(fileName = "Enemy_Level_", menuName = "ScriptableObjects/EnemyGroup", order = 1)]
public class EnemyGroup : ScriptableObject
{
    public GameObject[] enemy;
    public GameObject[] bosses;
}