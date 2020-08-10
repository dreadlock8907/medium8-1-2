using System;
using System.Collections.Generic;

namespace Task
{
    class Program
    {
        public static void Main(string[] args)
        {
            TrajectorySimulator simulator = new TrajectorySimulator();
            simulator.Add(new GameObject(name: "1", position: new Position(5, 5)));
            simulator.Add(new GameObject(name: "2", position: new Position(10, 10)));
            simulator.Add(new GameObject(name: "3", position: new Position(15, 15)));

            while(true)
            {
                simulator.DestroyAllCollided();
                simulator.MoveAllRandom();
                simulator.ShowAllAlive();
            }
        }
    }

    public class TrajectorySimulator
    {
        readonly List<GameObject> _pool;
        readonly Random _random;

        public TrajectorySimulator()
        {
            _pool = new List<GameObject>();
            _random = new Random();
        }

        public void Add(GameObject gameObject)
        {
            _pool.Add(gameObject);                
        }

        public void DestroyAllCollided()
        {
            List<GameObject> collided = FindAllCollided();
            for (int i = 0; i < collided.Count; i++)
                _pool.Remove(collided[i]);
        }

        private List<GameObject> FindAllCollided()
        {
            List<GameObject> collided = new List<GameObject>();
            for(int i = 0; i < _pool.Count; i++)
            {
                for (int k = 0; k < _pool.Count; k++)
                {
                    if(i == k)
                        continue;
                    
                    GameObject current = _pool[i];
                    GameObject other = _pool[k];
                    
                    if(collided.Contains(current))
                        continue;
                    
                    if(current.IsCollidedWith(other)) 
                        collided.Add(current);
                }
            }

            return collided;
        }

        public void MoveAllRandom()
        {
            for(int i = 0; i < _pool.Count; i++)
                _pool[i].MoveOn(_random.Next(-1, 1), _random.Next(-1, 1));
        }

        public void ShowAllAlive()
        {
            for(int i = 0; i < _pool.Count; i++)
                _pool[i].Show();
        }
    }

    public class GameObject
    {
        public Position Position { get; private set; }
        private string Name { get; set; }

        public GameObject(string name, Position position)
        {
            Name = name;
            Position = position;
        }

        public void MoveOn(int x, int y)
        {
            int newX = Position.X + x;
            int newY = Position.Y + y;

            if (newX < 0) newX = 0;
            if (newY < 0) newY = 0;
            
            Position = new Position(newX, newY);
        }
        
        public void Show()
        {
            Console.SetCursorPosition(Position.X, Position.Y);
            Console.Write(Name);
        }

        public bool IsCollidedWith(GameObject gameObject)
        {
            return Position.EqualsTo(gameObject.Position);
        }
    }

    public struct Position
    {
        public readonly int X;
        public readonly int Y;

        public Position(int x, int y)
        {
            X = x;
            Y = y;
        }

        public bool EqualsTo(Position position)
        {
            return X == position.X && Y == position.Y;
        }
    }
}