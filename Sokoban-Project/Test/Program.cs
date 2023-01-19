using System;

namespace Sokoban
{
    enum Direction
    {
        None,
        Left,
        Right,
        Up,
        Down
    }

    class Player
    {
        private int _x; //수정하거나 접근할 수 있는 데이터를 저장
        private int _y;
        private string _icon;
        private Direction _moveDirection;

        // 생성자 문법
        // ㄴ반환 타입이 없다.
        // ㄴ메소드 이름은 타입과 같게
        // 매개변수가 0개인 생서자를 기본 생성자(Default Construct)
        // 기본 생성자는 프로그래머가 생성자를 작성하지 않은 경우 컴파일러가 알아서 생성한다.

        // 내가 필요한 데이터를 초기화해준다.
        public Player()
        {
            _x = 1;
            _y = 1;
            _icon = "☻";
            _moveDirection = Direction.None;
        }

        //접근자(Getter) & 설정자(Setter)
        //접근자 : 데이터에 접근하는 메소드
        //설정자 : 데이터를 설정하는 메소드
        public int GetX() => _x;
        public int GetY() => _y;
        //public string GetIcon() => _icon; 수정할 필요가 없으므로?
        public Direction GetDirection() => _moveDirection;

        public void SetX(int newX) => _x = newX;
        public void SetY(int newY) => _y = newY;

        /// <summary>
        /// 플레이어를 그린다.
        /// </summary>
        public void Render()
        {
            Console.SetCursorPosition(_x, _y);
            Console.Write(_icon);
        }
        /// <summary>
        /// 플레이어를 움직인다.
        /// </summary>
        /// <param name="key">사용자가 입력한 키</param>
        public void Move(ConsoleKey key)
        {
            if (key == ConsoleKey.LeftArrow)
            {
                _x = Math.Max(1, _x - 1);
                _moveDirection = Direction.Left;
            }
            if (key == ConsoleKey.RightArrow)
            {
                _x = Math.Min(_x + 1, 19);
                _moveDirection = Direction.Right;
            }
            if (key == ConsoleKey.UpArrow)
            {
                _y = Math.Max(1, _y - 1);
                _moveDirection = Direction.Up;
            }
            if (key == ConsoleKey.DownArrow)
            {
                _y = Math.Min(_y + 1, 9);
                _moveDirection = Direction.Down;
            }
        }
    }

    class Wall
    {
        //data

        private int _x;
        private int _y;
        private string _icon;

        //기본 생성자
        public Wall()
        {
            _icon = "⎕";
            _x = 5;
            _y = 5;
        }

        public int GetX() => _x;
        public int GetY() => _y;

        //기능 여기서 Render한다.
        public void Render()
        {
            Console.SetCursorPosition(_x, _y);
            Console.Write(_icon);
        }
    }

    class Box
    {
        private int _x;
        private int _y;
        private string _icon;

        public Box()
        {
            _x = 1;
            _y = 1;
            _icon = "✦";
        }

        public int GetX() => _x;
        public int GetY() => _y;

        public void SetX(int newX) => _x = newX;
        public void SetY(int newY) => _y = newY;

        public void Render(int x, int y)
        {
            _x = x; // _x에 매개변수로 받아온 인자의 값을 다시 넣어준다.
            _y = y; // _y에 매개변수로 받아온 인자의 값을 다시 넣어준다.
            Console.SetCursorPosition(_x, _y);
            Console.Write(_icon);
        }
    }

    class Goal
    {
        private int _x;
        private int _y;
        private string _iconGoal;
        private string _iconGoalIn;

        public Goal()
        {
            _x = 8;
            _y = 8;
            _iconGoal = "✪";
            _iconGoalIn = "♥︎";
        }

        public int GetX() => _x;
        public int GetY() => _y;

        public void Render()
        {
            Console.SetCursorPosition(_x, _y);
            Console.Write(_iconGoal);
        }
    }

    class Sokoban
    {
        static void Main()
        {
            // 초기 세팅
            Console.ResetColor();
            Console.CursorVisible = false;
            Console.Title = "Huiji_Sokoban";
            // 맥 backgroundColor 실화냐
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Clear();

            // 기호 상수 정의
            const int WALL_COUNT = 4;
            const int BOX_COUNT = 3;
            const int GOAL_COUNT = 3;
            const int MAP_MIN_X = 1;
            const int MAP_MAX_X = 19;
            const int MAP_MIN_Y = 1;
            const int MAP_MAX_Y = 9;

            // 플레이어 위치를 저장하기 위한 변수
            Player player = new Player();

            // 벽 생성자
            Wall wall = new Wall();

            // 박스 생성자          
            Box box = new Box();


            Goal goal = new Goal();

            




            // 박스 초기 위치설정
            //int[] boxPositionX = { 5, 7, 15 };
            //int[] boxPositionY = { 3, 6, 8 };

            // wall 위치설정
            //int[] wallPositionX = { 3, 4, 7, 8 };
            //int[] wallPositionY = { 3, 3, 8, 8 };

            // goal 위치설정
            //int[] goalPositionX = { 18, 10, 5 };
            //int[] goalPositionY = { 3, 6, 8 };

            //플레이어 이동방향을 저장하기 위한 변수


            //밀고 있는 박스를 저장하기 위한 변수
            int pushedBoxId = 0;

            //박스가 들어간 골을 저장하기 위한 변수
            bool[] goalIn = new bool[GOAL_COUNT];



            while (true)
            {
                //Render------------------------------------------------------------
                Render();

                //Process Input-----------------------------------------------------
                ConsoleKey key = Console.ReadKey().Key;
                //Update------------------------------------------------------------

                Update(key);


                // 박스와 골의 처리
                int boxOnGoalcount = 0;

                for (int goalId = 0; goalId < GOAL_COUNT; ++goalId)
                {
                    goalIn[goalId] = false;

                    for (int boxId = 0; boxId < BOX_COUNT; ++boxId)
                    {


                        if (Iscollided(box.GetX(), box.GetY(), goal.GetX(), goal.GetY()))
                        {
                            goalIn[goalId] = true;
                            ++boxOnGoalcount;
                            break;
                        }
                    }
                    if (boxOnGoalcount == GOAL_COUNT)
                    {
                        Console.Clear();
                        Console.WriteLine("축하!!");
                        return;
                    }
                }
            }


            //프레임을 그린다.            
            void Render()
            {
                Console.Clear();

                // wall 출력
                //for (int wallCount = 0; wallCount < WALL_COUNT; ++wallCount)
                //{
                //    int wallX = wallPositionX[wallCount];
                //    int wallY = wallPositionY[wallCount];
                //    RenderObject(wallX, wallY, "⎕");
                //}

                wall.Render();

                for (int MapX = 0; MapX <= 20; ++MapX)
                {
                    RenderObject(MapX, 0, "⎕");
                    RenderObject(MapX, 10, "⎕");
                }
                for (int MapY = 1; MapY < 10; ++MapY)
                {
                    RenderObject(0, MapY, "⎕");
                    RenderObject(20, MapY, "⎕");
                }

                // box 출력
                //for (int boxCount = 0; boxCount < BOX_COUNT; ++boxCount)
                //{
                //    //int boxX = boxPositionX[boxCount];
                //    //int boxY = boxPositionY[boxCount];
                //    //RenderObject(boxX, boxY, "✦");
                //}
                
                

                // goal 출력
                //for (int goalId = 0; goalId < GOAL_COUNT; ++goalId)
                //{
                //    int goalX = goalPositionX[goalId];
                //    int goalY = goalPositionY[goalId];

                //    string goalSymbol = goalIn[goalId] ? "♥︎" : "✪";
                //    RenderObject(goalX, goalY, goalSymbol);
                //}
                goal.Render();

                // 플레이어 출력
                //RenderObject(playerX, playerY, "☻"); 밑의 구문으로 대
                player.Render();


            }

            // 오브젝트를 그린다.
            void RenderObject(int x, int y, string symbolString)
            {
                Console.SetCursorPosition(x, y);
                Console.Write(symbolString);
            }


            //Update
            void Update(ConsoleKey key)
            {
                //player 이동                
                //MovePlayer(key, ref playerX, ref playerY, ref playerDirection); 밑의 구문으로 대</param>
                player.Move(key);

                //player가 박스를 밀때
                for (int boxId = 0; boxId < BOX_COUNT; ++boxId)
                {
                    //int boxX = boxPositionX[boxId];
                    //int boxY = boxPositionY[boxId];

                    if (false == Iscollided(player.GetX(), player.GetY(), box.GetX(), box.GetY()))
                    {
                        continue;
                    }
                    switch (player.GetDirection())
                    {
                        case Direction.Left:
                            box.SetX(Math.Max(box.GetX() - 1, MAP_MIN_X));
                            player.SetX(box.GetX() + 1);
                            break;

                        case Direction.Right:
                            box.SetX(Math.Min(player.GetX() + 1, MAP_MAX_X));
                            player.SetX(box.GetX() - 1);
                            break;

                        case Direction.Up:
                            box.SetY(Math.Max(player.GetY() - 1, MAP_MIN_X));
                            player.SetY(box.GetY() + 1);
                            break;

                        case Direction.Down:
                            box.SetY(Math.Min(player.GetY() + 1, MAP_MAX_Y));
                            player.SetY(box.GetY() - 1);
                            break;

                        case Direction.None:
                            Console.Clear();
                            Console.WriteLine("[Error]플레이어 이동방향 데이터가 잘못 되었습니다.");
                            break;
                    }
                    //boxPositionX[boxId] = boxX;
                    //boxPositionY[boxId] = boxY;
                    pushedBoxId = boxId;
                    break;
                }

                //player가 벽에 부딪힐 때
                for (int wallId = 0; wallId < WALL_COUNT; ++wallId)
                {
                    //int wallX = wallPositionX[wallId];
                    //int wallY = wallPositionY[wallId];
                    if (false == Iscollided(player.GetX(), player.GetY(), wall.GetX(), wall.GetY()))
                    {
                        continue;
                    }
                    switch (player.GetDirection())
                    {
                        case Direction.Left:
                            player.SetX(wall.GetX() + 1);
                            break;

                        case Direction.Right:
                            player.SetX(wall.GetX() - 1);
                            break;

                        case Direction.Up:
                            player.SetY(wall.GetY() + 1);
                            break;

                        case Direction.Down:
                            player.SetY(wall.GetY() - 1);
                            break;

                        case Direction.None:
                            Console.Clear();
                            Console.WriteLine("플레이어 이동방향 데이터가 잘못 되었습니다.");
                            break;
                    }
                }
                //box가 벽에 부딪힐 때
                for (int wallId = 0; wallId < WALL_COUNT; ++wallId)
                {
                    //int wallX = wallPositionX[wallId];
                    //int wallY = wallPositionY[wallId];

                    if (false == Iscollided(box.GetX(), box.GetY(), wall.GetX(), wall.GetY()))
                    {
                        continue;
                    }

                    switch (player.GetDirection())
                    {
                        case Direction.Left:
                            box.SetX(wall.GetX() + 1);
                            player.SetX(box.GetX() + 1);
                            break;

                        case Direction.Right:
                            box.SetX(wall.GetX() - 1);
                            player.SetX(box.GetX() - 1);
                            break;

                        case Direction.Up:
                            box.SetY(wall.GetY() + 1);
                            player.SetY(box.GetY() + 1);
                            break;

                        case Direction.Down:
                            box.SetY(wall.GetY() - 1);
                            player.SetY(box.GetY() - 1);
                            break;

                        case Direction.None:
                            Console.Clear();
                            Console.WriteLine("[Error]플레이어 이동방향 데이터가 잘못 되었습니다");
                            return;
                    }
                    break;
                }

                // 박스랑 박스가 부딪힐 때
                //for (int CollidedBoxId = 0; CollidedBoxId < BOX_COUNT; ++CollidedBoxId)
                //{
                //    if (CollidedBoxId == pushedBoxId)
                //    {
                //        continue;
                //    }
                //    if (false == Iscollided(boxPositionX[pushedBoxId], boxPositionY[pushedBoxId], boxPositionX[CollidedBoxId], boxPositionY[CollidedBoxId]))
                //    {
                //        continue;
                //    }
                //    switch (player.GetDirection())
                //    {
                //        case Direction.Left:
                //            boxPositionX[pushedBoxId] = boxPositionX[CollidedBoxId] + 1;
                //            player.SetX(boxPositionX[pushedBoxId] + 1);
                //            break;

                //        case Direction.Right:
                //            boxPositionX[pushedBoxId] = boxPositionX[CollidedBoxId] - 1;
                //            player.SetX(boxPositionX[pushedBoxId] - 1);
                //            break;

                //        case Direction.Up:
                //            boxPositionY[pushedBoxId] = boxPositionX[CollidedBoxId] + 1;
                //            player.SetY(boxPositionY[pushedBoxId] + 1);
                //            break;

                //        case Direction.Down:
                //            boxPositionY[pushedBoxId] = boxPositionY[CollidedBoxId] - 1;
                //            player.SetY(boxPositionY[pushedBoxId] - 1);
                //            break;

                //        case Direction.None:
                //            Console.Clear();
                //            Console.WriteLine("[Error]플레이어 이동방향 데이터가 잘못 되었습니다");
                //            return;
                //    }
                //}
            }

            //두 물체가 충돌했는지 판별한다.
            bool Iscollided(int x1, int y1, int x2, int y2)
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
}