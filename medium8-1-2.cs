using System;
using System.Collections.Generic;

namespace Task
{
    class Program
    {
        public static void Main(string[] args)
        {
            GameObjectsPool pool = new GameObjectsPool();
            pool.Add(new GameObject(name: "1", isAlive: true, position: new Position(5, 5)));
            pool.Add(new GameObject(name: "2", isAlive: true, position: new Position(10, 10)));
            pool.Add(new GameObject(name: "3", isAlive: true, position: new Position(15, 15)));

            while(true)
            {
                pool.DestroyAllCollided();
                pool.MoveAllRandom();
                pool.ShowAllAlive();
            }
        }
    }

    public class GameObjectsPool
    {
        readonly List<GameObject> _pool;
        readonly Random random;

        public GameObjectsPool()
        {
            _pool = new List<GameObject>();
            random = new Random();
        }

        public void Add(GameObject gameObject)
        {
            _pool.Add(gameObject);                
        }

        public void DestroyAllCollided()
        {
            for(int i = 0; i < _pool.Count; i++)
            {
                for (int k = 0; k < _pool.Count; k++)
                {
                    if(i == k)
                        continue;
                    
                    GameObject current = _pool[i];
                    GameObject other = _pool[k];
                    
                    if(current.IsCollidedWith(other)) 
                        current.Destroy();
                }
            }
        }

        public void MoveAllRandom()
        {
            for(int i = 0; i < _pool.Count; i++)
                _pool[i].MoveOn(random.Next(-1, 1), random.Next(-1, 1));
        }

        public void ShowAllAlive()
        {
            for(int i = 0; i < _pool.Count; i++)
            {
                GameObject gameObject = _pool[i];
                if(gameObject.IsAlive)
                    gameObject.Show();
            }
        }
    }

    public class GameObject
    {
        public Position Position { get; private set; }
        public bool IsAlive { get; private set; }
        string Name { get; set; }

        public GameObject(string name, bool isAlive, Position position)
        {
            Name = name;
            IsAlive = isAlive;
            Position = position;
        }
        
        public void Destroy()
        {
            IsAlive = false;
        }

        public void MoveOn(int x, int y)
        {
            int newX = Position.X + x;
            int newY = Position.Y + y;
            
            Position.Set(newX, newY);
            Position.Normalize();
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

    public class Position
    {
        public int X => _x.Value;
        public int Y => _y.Value;
        
        Coordinate _x;
        Coordinate _y;

        public Position(int x, int y)
        {
            _x = new Coordinate(x); 
            _y = new Coordinate(y);
        }

        public void Set(int x, int y)
        {
            _x.Value = x;
            _y.Value = y;
        }

        public void Normalize()
        {
            _x.Value = _x.Value > 0 ? _x.Value : 0;
            _y.Value = _y.Value > 0 ? _y.Value : 0;
        }

        public bool EqualsTo(Position position)
        {
            return X == position.X && Y == position.Y;
        }
    }

    public class Coordinate
    {
        public int Value { get; set; }

        public Coordinate(int coordinate)
        {
            Value = coordinate;
        }
    }
}