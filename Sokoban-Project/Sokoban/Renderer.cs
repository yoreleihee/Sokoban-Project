using System;
namespace Sokoban
{
	/// <summary>
	/// 콘솔 환경에 그려주는 걸 도와주는 클래스
	/// </summary>
	internal class Renderer //한 프로젝트 안에서만 쓸거기 때문에 internal
	{
		public void Render(int x, int y, string symbol)
		{
			Console.SetCursorPosition(x, y);
			Console.Write(symbol);
		}
	}
}

