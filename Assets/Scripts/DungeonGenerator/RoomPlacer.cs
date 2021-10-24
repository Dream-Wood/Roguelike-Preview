using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DungeonGenerator
{
    public class RoomPlacer : MonoBehaviour
    {
        [SerializeField] private List<RoomTile> roomTilesPrefabs;
        [SerializeField] private List<RoomTile> startRooms;
        [SerializeField] private List<RoomTile> endRooms;
        [SerializeField] private List<RoomTile> edge;
        [SerializeField] private List<RoomTile> specialRooms;
        [SerializeField] private Vector2Int mapSize = new Vector2Int(10, 10);

        private RoomTile[,] _spawnedRooms;

        private readonly Queue<Vector2Int> _recalculatePossibleRoomQueue = new Queue<Vector2Int>();
        private List<RoomTile>[,] _possibleRooms;

        private Vector2Int _specialPos;
        private Vector2Int _endPos;

        private float _timer = 5;

        private void Start()
        {
            _spawnedRooms = new RoomTile[mapSize.x, mapSize.y];
            Generate();
        }

        private void Generate()
        {
            RoomTile spec = GetRandomTile(specialRooms);
            RoomTile boss = GetRandomTile(endRooms);

            _possibleRooms = new List<RoomTile>[mapSize.x, mapSize.y];
            int maxAttempts = 10;
            int attempts = 0;
            while (attempts++ < maxAttempts)
            {
                for (int x = 0; x < mapSize.x; x++)
                for (int y = 0; y < mapSize.y; y++)
                {
                    if (x <= 1 || y <= 1 || x >= mapSize.x - 2 || y >= mapSize.y - 2)
                    {
                        _possibleRooms[x, y] = new List<RoomTile>(edge);
                        continue;
                    }

                    _possibleRooms[x, y] = new List<RoomTile>(roomTilesPrefabs);
                }


                RoomTile startRoom = GetRandomTile(startRooms);
                _possibleRooms[mapSize.x / 2, mapSize.y / 2] = new List<RoomTile> {startRoom};
                _specialPos = new Vector2Int(Random.Range(2, mapSize.x - 2), Random.Range(2, mapSize.y / 2));
                _possibleRooms[_specialPos.x, _specialPos.y] = new List<RoomTile> {spec};
                _endPos = new Vector2Int(Random.Range(2, mapSize.x - 2), Random.Range(mapSize.y / 2 + 1, mapSize.y - 2));
                _possibleRooms[_endPos.x, _endPos.y] = new List<RoomTile> {boss};

                _recalculatePossibleRoomQueue.Clear();
                EnqueueNeighboursToRecalculate(new Vector2Int(mapSize.x / 2, mapSize.y / 2));

                bool success = GenerateAllPossibleRooms();
                if (success) break;
            }

            if (_possibleRooms[_endPos.x, _endPos.y][0] != boss || _possibleRooms[_specialPos.x, _specialPos.y][0] != spec)
            {
                Generate();
                return;
            }

            PlaceAllRoom();
        }

        private bool GenerateAllPossibleRooms()
        {
            int maxIteration = mapSize.x * mapSize.y;
            int iterations = 0;
            int backtracks = 0;

            while (iterations++ < maxIteration)
            {
                int maxInnerIterations = 500;
                int innerIterations = 0;

                while (_recalculatePossibleRoomQueue.Count > 0 && innerIterations++ < maxInnerIterations)
                {
                    Vector2Int position = _recalculatePossibleRoomQueue.Dequeue();

                    if (position.x == 0 || position.y == 0 ||
                        position.x == mapSize.x - 1 || position.y == mapSize.y - 1) continue;

                    List<RoomTile> possibleRoomsHere = _possibleRooms[position.x, position.y];

                    int countRemoved = possibleRoomsHere.RemoveAll(t => !IsRoomPossibleHere(t, position));

                    if (countRemoved > 0) EnqueueNeighboursToRecalculate(position);

                    // possibleRoomHere is Exist = false
                    if (possibleRoomsHere.Count == 0)
                    {
                        possibleRoomsHere.AddRange(roomTilesPrefabs);
                        _possibleRooms[position.x + 1, position.y] = new List<RoomTile>(roomTilesPrefabs);
                        _possibleRooms[position.x - 1, position.y] = new List<RoomTile>(roomTilesPrefabs);
                        _possibleRooms[position.x, position.y + 1] = new List<RoomTile>(roomTilesPrefabs);
                        _possibleRooms[position.x, position.y - 1] = new List<RoomTile>(roomTilesPrefabs);

                        EnqueueNeighboursToRecalculate(position);

                        backtracks++;
                    }
                }

                if (innerIterations == maxInnerIterations) break;

                List<RoomTile> maxCountRooms = _possibleRooms[1, 1];
                Vector2Int maxCountRoomPosition = new Vector2Int(1, 1);

                for (int x = 1; x < mapSize.x - 1; x++)
                for (int y = 1; y < mapSize.x - 1; y++)
                {
                    if (_possibleRooms[x, y].Count > maxCountRooms.Count)
                    {
                        maxCountRooms = _possibleRooms[x, y];
                        maxCountRoomPosition = new Vector2Int(x, y);
                    }
                }

                if (maxCountRooms.Count == 1)
                {
                    return true;
                }

                RoomTile roomToCollapse = GetRandomTile(maxCountRooms);
                _possibleRooms[maxCountRoomPosition.x, maxCountRoomPosition.y] = new List<RoomTile> {roomToCollapse};
                EnqueueNeighboursToRecalculate(maxCountRoomPosition);
            }

            Debug.LogError("Failed, run out of iterations");
            return false;
        }

        private bool IsRoomPossibleHere(RoomTile room, Vector2Int position)
        {
            bool isAllRightImpossible = _possibleRooms[position.x + 1, position.y]
                .All(rightRoom => !CanAppendTile(room, rightRoom, Direction.Right));
            if (isAllRightImpossible) return false;

            bool isAllLeftImpossible = _possibleRooms[position.x - 1, position.y]
                .All(leftRoom => !CanAppendTile(room, leftRoom, Direction.Left));
            if (isAllLeftImpossible) return false;

            bool isAllUpImpossible = _possibleRooms[position.x, position.y + 1]
                .All(upRoom => !CanAppendTile(room, upRoom, Direction.Up));
            if (isAllUpImpossible) return false;

            bool isAllDownImpossible = _possibleRooms[position.x, position.y - 1]
                .All(downRoom => !CanAppendTile(room, downRoom, Direction.Down));
            if (isAllDownImpossible) return false;

            return true;
        }

        private bool CanAppendTile(RoomTile existingRoom, RoomTile roomToAppend, Direction direction)
        {
            if (existingRoom == null) return true;

            int score = 0;

            if (direction == Direction.Right)
            {
                if (existingRoom.IsDoorExist(Direction.Right)) score++;
                if (roomToAppend.IsDoorExist(Direction.Left)) score++;
                return score is 0 or 2;
            }
            else if (direction == Direction.Left)
            {
                if (existingRoom.IsDoorExist(Direction.Left)) score++;
                if (roomToAppend.IsDoorExist(Direction.Right)) score++;
                return score is 0 or 2;
            }
            else if (direction == Direction.Up)
            {
                if (existingRoom.IsDoorExist(Direction.Up)) score++;
                if (roomToAppend.IsDoorExist(Direction.Down)) score++;
                return score is 0 or 2;
            }
            else if (direction == Direction.Down)
            {
                if (existingRoom.IsDoorExist(Direction.Down)) score++;
                if (roomToAppend.IsDoorExist(Direction.Up)) score++;
                return score is 0 or 2;
            }
            else
            {
                throw new ArgumentException("Wrong direction value, should be Vector3.left/right/up/down",
                    nameof(direction));
            }
        }

        private void PlaceAllRoom()
        {
            for (int x = 1; x < mapSize.x - 1; x++)
            for (int y = 1; y < mapSize.y - 1; y++)
            {
                PlaceRoom(x, y);
            }
        }

        private void EnqueueNeighboursToRecalculate(Vector2Int position)
        {
            _recalculatePossibleRoomQueue.Enqueue(new Vector2Int(position.x + 1, position.y));
            _recalculatePossibleRoomQueue.Enqueue(new Vector2Int(position.x - 1, position.y));
            _recalculatePossibleRoomQueue.Enqueue(new Vector2Int(position.x, position.y + 1));
            _recalculatePossibleRoomQueue.Enqueue(new Vector2Int(position.x, position.y - 1));
        }

        private void PlaceRoom(int x, int y)
        {
            if (_possibleRooms[x, y].Count == 0) return;

            RoomTile selectedTile = GetRandomTile(_possibleRooms[x, y]);
            Vector3 position = selectedTile.GetRoomSize() * selectedTile.GetRoomSide() * new Vector3(x, 0, y);
            _spawnedRooms[x, y] = Instantiate(selectedTile, position, selectedTile.transform.rotation);
            if (x == mapSize.x / 2 && y == mapSize.y / 2)
            {
                _spawnedRooms[x, y].SetStartRoom();
            }
        }

        private RoomTile GetRandomTile(List<RoomTile> availableTiles)
        {
            List<float> chances = new List<float>();
            for (int i = 0; i < availableTiles.Count; i++)
            {
                chances.Add(availableTiles[i].Weight);
            }

            float value = Random.Range(0, chances.Sum());
            float sum = 0;

            for (int i = 0; i < chances.Count; i++)
            {
                sum += chances[i];
                if (value < sum)
                {
                    return availableTiles[i];
                }
            }

            return availableTiles[availableTiles.Count - 1];
        }
    }
}