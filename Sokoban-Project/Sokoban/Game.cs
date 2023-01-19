using System;
namespace Sokoban
{
	/// <summary>
	/// 소코반 게임과 관련된 데이터 관리
	/// </summary>
	public class Game
	{
        public const int MAP_MIN_X = 1;
        public const int MAP_MAX_X = 19;
        public const int MAP_MIN_Y = 1;
        public const int MAP_MAX_Y = 9;

        public void Initialize()
        {
            // 초기 세팅
            Console.ResetColor();
            Console.CursorVisible = false;
            Console.Title = "Huiji_Sokoban";
            // 맥 backgroundColor 실화냐
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Clear();
        }
    }
}

