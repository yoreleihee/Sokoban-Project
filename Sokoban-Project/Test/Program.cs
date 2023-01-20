

using System;
using Sokoban_Huiji;

namespace Sokoban_Huiji; //namespace가 같아야 class를 쓸 수 있다.


class Program
{
    static void Main(string[] args)
    {
        // 초기 세팅
        Console.ResetColor();                               // 컬러를 초기화한다.
        Console.CursorVisible = false;                      // 커서를 숨긴다.
        Console.Title = "My Sokoban";                       // 타이틀을 설정한다.
        Console.BackgroundColor = ConsoleColor.DarkBlue;    // 배경색을 설정한다.
        Console.ForegroundColor = ConsoleColor.Gray;        // 글꼴색을 설정한다.
        Console.Clear();                                    // 콘솔 창에 출력된 내용을 모두 지운다.

        // 기호 상수 정의
        const int MIN_X = 0;
        const int MAX_X = 15;
        const int MIN_Y = 0;
        const int MAX_Y = 20;

        // 플레이어 생성
        Player player = new Player()
        {
            X = 0,
            Y = 0,
            PlayerDirection = Direction.None,
            PushedBoxIndex = 0
        };

        // 박스 생성
        Box[] boxes = new Box[]
        {
            new Box { X = 10, Y = 2, IsOnGoal = false},
            new Box { X = 13, Y = 3, IsOnGoal = false},
        };


        // 벽 생성
        Wall[] walls = new Wall[]
        {
            new Wall { X = 7, Y = 7},
            new Wall { X = 11, Y = 5}
        };

        // 골 생성
        Goal[] goals = new Goal[]
        {
            new Goal { X = 10, Y = 10},
            new Goal { X = 3, Y = 6}
        };


        // 게임 루프
        while (true)
        {
            // ======================= Render ==========================
            // 이전 프레임을 지운다
            Console.Clear();

            // 플레이어를 그린다
            RenderObject(player.X, player.Y, "☻");

            // 골을 그린다
            int goalCount = goals.Length;
            for (int i = 0; i < goalCount; ++i)
            {
                RenderObject(goals[i].X, goals[i].Y, "✪");
            }

            // 박스를 그린다
            int boxCount = boxes.Length;
            for (int i = 0; i < boxCount; ++i)
            {
                string boxIcon = boxes[i].IsOnGoal ? "♥︎" : "✩";
                RenderObject(boxes[i].X, boxes[i].Y, boxIcon);
            }

            // 벽을 그린다
            int wallCount = walls.Length;
            for (int i = 0; i < wallCount; ++i)
            {
                RenderObject(walls[i].X, walls[i].Y, "⎕");
            }

            // ======================= ProcessInput =======================
            // 유저로부터 입력을 받는다 
            ConsoleKeyInfo keyInfo = Console.ReadKey();
            ConsoleKey key = keyInfo.Key;   // 실제 키는 ConsoleKeyInfo에 Key에 있다 

            // ======================= Update =======================
            MovePlayer(key, ref player.X, ref player.Y, ref player.PlayerDirection);

            // 플레이어와 벽의 충돌 처리
            for (int i = 0; i < wallCount; ++i)
            {
                if (false == IsCollided(player.X, player.Y, walls[i].X, walls[i].Y))
                {
                    continue;
                }

                OnCollision(() =>
                {
                    // target에서 반대로 움직이는 코드
                    PushOut(player.PlayerDirection, ref player.X, ref player.Y, in walls[i].X, in walls[i].Y);
                });
            }

            // 박스 업데이트
            for (int i = 0; i < boxCount; ++i)
            {
                if (false == IsCollided(player.X, player.Y, boxes[i].X, boxes[i].Y))
                {
                    continue;
                }

                OnCollision(() =>
                {
                    MoveBox(player.PlayerDirection, ref boxes[i].X, ref boxes[i].Y,
                            player.X, player.Y);
                });


                // 어떤 박스를 밀었는지 저장해야 한다 
                player.PushedBoxIndex = i;

                break;
            }

            // 박스끼리의 충돌 처리
            for (int i = 0; i < boxCount; ++i)
            {
                // 같은 박스라면 처리할 필요가 없다 
                if (player.PushedBoxIndex == i)
                {
                    continue;
                }

                if (false == IsCollided(boxes[player.PushedBoxIndex].X, boxes[player.PushedBoxIndex].Y,
                                    boxes[i].X, boxes[i].Y))
                {
                    continue;
                }

                OnCollision(() =>
                {
                    PushOut(player.PlayerDirection, ref boxes[player.PushedBoxIndex].X, ref boxes[player.PushedBoxIndex].Y,
                            boxes[i].X, boxes[i].Y); //in은 인자로 줄때 안써줘도 됨. out은 써줘야함.

                    PushOut(player.PlayerDirection, ref player.X, ref player.Y,
                            boxes[player.PushedBoxIndex].X, boxes[player.PushedBoxIndex].Y);
                });
            }

            // 박스와 벽의 충돌 처리
            for (int i = 0; i < wallCount; ++i)
            {
                if (false == IsCollided(boxes[player.PushedBoxIndex].X, boxes[player.PushedBoxIndex].Y,
                                    walls[i].X, walls[i].Y))
                {
                    continue;
                }

                OnCollision(() =>
                {
                    PushOut(player.PlayerDirection, ref boxes[player.PushedBoxIndex].X, ref boxes[player.PushedBoxIndex].Y,
                            walls[i].X, walls[i].Y);
                    PushOut(player.PlayerDirection, ref player.X, ref player.Y,
                            boxes[player.PushedBoxIndex].X, boxes[player.PushedBoxIndex].Y);
                });
                //OnCollision(player.PlayerDirection,
                //    ref boxes[player.PushedBoxIndex].X, ref boxes[player.PushedBoxIndex].Y,
                //    in walls[i].X, in walls[i].Y);
                //OnCollision(player.PlayerDirection,
                //    ref player.X, ref player.Y,
                //    in boxes[player.PushedBoxIndex].X, in boxes[player.PushedBoxIndex].Y);
                break;
            }


            int boxOnGoalCount = CountBoxOnGoal(boxes, goals);

            if (boxOnGoalCount == goalCount)
            {
                break;
            }
        }

        Console.Clear();
        Console.WriteLine("축하합니다. 게임을 클리어하셨습니다.");

        // 게임이 끝났으니 콘솔 세팅을 다시 정상화한다.
        Console.ResetColor();


        // 오브젝트를 그린다
        void RenderObject(int x, int y, string icon)
        {
            Console.SetCursorPosition(x, y);
            Console.Write(icon);
        }

        // 골 위에 박스가 몇 개 있는지 센다
        // Box 클래스의 배열 boxes를 매개변수로 받으면 Box 클래스 안의 데이터가 다 들어간다.
        // box의 x좌표에 접근하고 싶다면 인자나 매개변수에서 접근하는게 아니라 함수안에서 접근할 수 있다.
        int CountBoxOnGoal(Box[] boxes, Goal[] goals)
        {
            int boxCount = boxes.Length;
            int goalCount = goals.Length;

            int result = 0;
            for (int boxId = 0; boxId < boxCount; ++boxId)
            {
                boxes[boxId].IsOnGoal = false;

                for (int goalId = 0; goalId < goalCount; ++goalId)
                {
                    if (IsCollided(boxes[boxId].X, boxes[boxId].Y,
                                    goals[goalId].X, goals[goalId].Y))
                    {
                        ++result;

                        boxes[boxId].IsOnGoal = true;

                        break;
                    }
                }
            }

            return result;
        }

        // target 근처로 이동시킨다 
        void MoveToLeftOfTarget(out int x, in int target) => x = Math.Max(MIN_X, target - 1);
        void MoveToRightOfTarget(out int x, in int target) => x = Math.Min(target + 1, MAX_X);
        void MoveToUpOfTarget(out int y, in int target) => y = Math.Max(MIN_Y, target - 1);
        void MoveToDownOfTarget(out int y, in int target) => y = Math.Min(target + 1, MAX_Y);

        // 플레이어를 움직인다
        // 여기도 마찬가지로 Player player로 바꿔주자.
        void MovePlayer(ConsoleKey key, ref int x, ref int y, ref Direction moveDirection)
        {
            if (key == ConsoleKey.LeftArrow)
            {
                MoveToLeftOfTarget(out x, in x);
                moveDirection = Direction.Left;
            }

            if (key == ConsoleKey.RightArrow)
            {
                MoveToRightOfTarget(out x, in x);
                moveDirection = Direction.Right;
            }

            if (key == ConsoleKey.UpArrow)
            {
                MoveToUpOfTarget(out y, in y);
                moveDirection = Direction.Up;
            }

            if (key == ConsoleKey.DownArrow)
            {
                MoveToDownOfTarget(out y, in y);
                moveDirection = Direction.Down;
            }
        }

        // 충돌을 처리한다 => player가 박스를 미는 상황일 수도 있고, 움직이는 박스가 벽이나 다른 상자를 만났을때도 있다.
        void OnCollision(Action action)
        {
            action();
        }

        //충돌을 처리한다.
        //움직이는 물체의 좌표를 바꿔준다.
        //충돌한 물체는 가만히 둔다.
        void PushOut(Direction playerMoveDirection, ref int objX, ref int objY,
                    in int collidedObjX, in int collidedObjY)
        {
            switch (playerMoveDirection)
            {
                //플레이어가 왼쪽으로 오고 있을때 충돌한 물체를 만났다면
                case Direction.Left:
                    //충돌한 물체를 Target으로 두고 움직이는 물체를 오른쪽에 둔다.
                    MoveToRightOfTarget(out objX, in collidedObjX);

                    break;
                //플레이어가 오른쪽으로 오고 있을때 충돌한 물체를 만났다면
                case Direction.Right:
                    MoveToLeftOfTarget(out objX, in collidedObjX);

                    break;
                case Direction.Up:
                    MoveToDownOfTarget(out objY, in collidedObjY);

                    break;
                case Direction.Down:
                    MoveToUpOfTarget(out objY, in collidedObjY);

                    break;
            }
        }

        //player가 box를 밀때 => player 방향으로 box가 한칸 더 밀려야한다.
        //위의 상황과 반대
        //Class Box는 이미 참조타입이라서 ref쓸 필요 없다 Box box 이케쓰면 됨. 함수 안에서 boxes의 좌표에 접근하게 하자.
        void MoveBox(Direction playerMoveDirection, ref int boxX, ref int boxY, in int playerX, in int playerY)
        {
            switch (playerMoveDirection)
            {
                case Direction.Left:
                    MoveToLeftOfTarget(out boxX, in playerX);
                    break;

                case Direction.Right:
                    MoveToRightOfTarget(out boxX, in playerX);
                    break;

                case Direction.Up:
                    MoveToUpOfTarget(out boxY, in playerY);
                    break;

                case Direction.Down:
                    MoveToDownOfTarget(out boxY, in playerY);
                    break;
            }
        }


        // 에러 메시지와 함께 애플리케이션을 종료한다 
        void ExitWithError(string errorMessage)
        {
            Console.Clear();
            Console.WriteLine(errorMessage);
            Environment.Exit(1);
        }

        // 충돌했는지 검사한다 
        bool IsCollided(int x1, int y1, int x2, int y2)
        {
            if (x1 == x2 && y1 == y2)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}