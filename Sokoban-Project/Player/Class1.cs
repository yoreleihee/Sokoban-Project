namespace Player;
internal class Player
{
    // 기능 => 메소드 => Player 타입을 다루는 인터페이스(Interface)
    enum Direction
    {
        None,
        Left,
        Right,
        Up,
        Down
    }

    private int playerX = 0;
    private int playerY = 0;

    private Direction playerMoveDirection = Direction.None;
    private int _pushedBoxIndes = 0;

    int pushedBoxIndex = 0;

    public void Move()
    {

    }
}

