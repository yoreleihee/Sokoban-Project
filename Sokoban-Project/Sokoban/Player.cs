using System;
namespace Sokoban
{
	public class Player
	{
        public enum Direction
        {
            None,
            Left,
            Right,
            Up,
            Down
        }
        // 필드는 private으로 하는게 유리하다.
        private int _x = 1;
        private int _y = 1;
        private string _symbol = "☻";
        private Direction _moveDirection = Direction.None;
        private int _pushedBoxIndes = 0;


        //접근자(Getter), 설정자(Setter)

        //접근자
        public int GetX() => _x;
        public int GetY() => _y;
        public string GetSymbol() => _symbol;
        public Direction GetDirection() => _moveDirection;


        // 설정자
        public void SetX(int newX) => _x = newX;
        public void SetY(int newY) => _y = newY;

        // 이 클래스를 다루는 인터페이스가 된다.
        public void Move(ConsoleKey key)
        {
            if (key == ConsoleKey.LeftArrow)
            {
                _x = Math.Max(Game.MAP_MIN_X, _x - 1);
                _moveDirection = Direction.Left;
            }
            if (key == ConsoleKey.RightArrow)
            {
                _x = Math.Min(_x + 1, Game.MAP_MAX_X);
                _moveDirection = Direction.Right;
            }
            if (key == ConsoleKey.UpArrow)
            {
                _y = Math.Max(Game.MAP_MIN_Y, _y - 1);
                _moveDirection = Direction.Up;
            }
            if (key == ConsoleKey.DownArrow)
            {
                _y = Math.Min(_y + 1, Game.MAP_MAX_Y);
                _moveDirection = Direction.Down;
            }
        }
    }
}

