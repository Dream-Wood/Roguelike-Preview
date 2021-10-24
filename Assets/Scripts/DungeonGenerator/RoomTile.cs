using System.Collections.Generic;
using UnityEngine;

namespace DungeonGenerator
{
    public class RoomTile : MonoBehaviour
    {
        [SerializeField] private float voxelSize = 0.1f;
        [SerializeField] private int tileSideVoxels = 8;

        [Range(1, 100)] public int Weight = 50;

        [SerializeField] private List<Direction> _directions = new List<Direction>() {Direction.Nope};

        private Room _room;
    
        public List<Direction> GetDir()
        {
            return _directions;
        }

        public void SetStartRoom()
        {
            _room = GetComponent<Room>();
            _room.SetStartRoom();
        }

        public bool IsDoorExist(Direction direction)
        {
            if (_directions[0] == Direction.Nope || _directions == null)
            {
                return false;
            }

            foreach (var dir in _directions)
            {
                if (dir == direction)
                {
                    return true;
                }
            }

            return false;
        }

        public float GetRoomSize()
        {
            return voxelSize;
        }

        public int GetRoomSide()
        {
            return tileSideVoxels;
        }
    }
}