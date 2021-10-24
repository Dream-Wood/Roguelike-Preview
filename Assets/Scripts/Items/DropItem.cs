using UnityEngine;

namespace Items
{
    public class DropItem : MonoBehaviour
    {

        [SerializeField] private DropItemType type;

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<Player>())
            {
                other.GetComponent<Player>().AddItem(type);
                Destroy(gameObject);
            }
        }
    }
}
