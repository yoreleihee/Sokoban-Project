using System;
namespace Sokoban_Huiji
{
    public enum Direction
    {
        None,
        Left,
        Right,
        Up,
        Down
    }

    public class Player
    {
        public int X;
        public int Y;
        public Direction PlayerDirection;
        // pushedBoxIndex가 플레이어가 가져야할 데이터는 아니라는 교수님의 의견
        // 나중에 class로 충돌처리를 만들었을때 충돌처리가 가져야할 데이터
        public int PushedBoxIndex;
    }
}