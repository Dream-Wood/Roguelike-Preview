using UnityEngine;
using Random = UnityEngine.Random;

namespace DungeonGenerator
{
    public class Room : MonoBehaviour
    {
        public delegate void RoomClear(bool val);

        public event RoomClear RoomIsClear;

        [SerializeField] private bool isStartRoom;
        [SerializeField] private bool isSpecial;
        [SerializeField] private bool isBoss;
        [SerializeField] private EnemyGroup enemyGroup;
        [SerializeField] private GameObject light;
        [SerializeField] private float radiusActive = 60;
        [SerializeField] private GameObject pedestal;
        [SerializeField] private GameObject portal;

        private int _countEnemy;
        private Transform _playerTransform;

        private void OnEnable()
        {
            _playerTransform = GameObject.FindWithTag("Player").transform;
        }

        private void Initialize()
        {
            if (isBoss)
            {
                GameObject tmp = Instantiate(enemyGroup.bosses[Random.Range(0, enemyGroup.bosses.Length)],
                    transform.position + Vector3.up, Quaternion.identity);
                tmp.GetComponent<IArtificialIntelligenceInit>().Init(this);
                FindObjectOfType<GUIInformation>().ShowTextEvent($"You found your death: {tmp.name}");
                _countEnemy = 1;
                return;
            }
        
            _countEnemy = Random.Range(1, 5);
            for (int i = 0; i < _countEnemy; i++)
            {
                Vector3 pos = new Vector3(Random.Range(-5, 5), 1, Random.Range(-5, 5));
                GameObject tmp = Instantiate(enemyGroup.enemy[Random.Range(0, enemyGroup.enemy.Length)],
                    transform.position + pos, Quaternion.identity);
                tmp.GetComponent<IArtificialIntelligenceInit>().Init(this);
            }
        }

        public void SetStartRoom()
        {
            isStartRoom = true;
        }

        private void LateUpdate()
        {
            if (Vector3.SqrMagnitude(transform.position - _playerTransform.position) > radiusActive)
            {
                light.SetActive(false);
            }
            else
            {
                light.SetActive(true);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<Player>())
            {
                if (isSpecial)
                { 
                    Instantiate(pedestal, transform.position + Vector3.up * 10, Quaternion.identity);
                    Destroy(GetComponent<BoxCollider>());
                    return;
                }
                if (isStartRoom)
                {
                    RoomIsClear?.Invoke(true);
                    other.GetComponent<Player>().TestEvent();
                    Destroy(GetComponent<BoxCollider>());
                }
                else
                {
                    RoomIsClear?.Invoke(false);
                    Initialize();
                }
            }
        }

        public void EnemyDeath()
        {
            _countEnemy--;
            if (_countEnemy == 0)
            {
                if (isBoss)
                {
                    Instantiate(pedestal, transform.position + Vector3.up * 10, Quaternion.identity);
                    portal.SetActive(true);
                }
                RoomIsClear?.Invoke(true);
                Destroy(GetComponent<BoxCollider>());
            }
        }
    }
}