using System.Drawing;

namespace _17;

public class ShapeEmitter
{
    private int index = 0;

    private Type[] shapes = new[]
    {
        typeof(Line),
        typeof(Plus),
        typeof(Corner),
        typeof(Bar),
        typeof(Square),
    };

    public Shape GetNextShape()
    {
        if (index == shapes.Length)
            index = 0;

        return Activator.CreateInstance(shapes[index++]) as Shape;
    }
}


enum Movement
{
    None,
    Left,
    Right,
    Fall
}

public abstract class Shape
{
    public Point[] Bits { get; protected set; }

    private Movement lastMovement = Movement.None;

    public void PositionAbove(int y)
    {
        for (int i = 0; i < Bits.Length; i++)
        {
            Bits[i].X += 2; // move two paces right (left most x = 3) 
            Bits[i].Y += (y + 3); // 3 above top shape
        }
    }

    public void ApplyJet(char jet)
    {
        switch (jet)
        {
            case '<':
                MoveLeft();
                break;
            case '>':
                MoveRight();
                break;
            default:
                throw new NotImplementedException();
        }
    }

    private void MoveLeft()
    {
        lastMovement = Movement.None;

        if (Bits.Any(bit => bit.X == 1))
            return;

        for (int i = 0; i < Bits.Length; i++)
        {
            Bits[i].X -= 1;
        }

        lastMovement = Movement.Left;
    }

    private void UndoMoveLeft() => MoveRight();

    private void MoveRight()
    {
        lastMovement = Movement.None;

        if (Bits.Any(bit => bit.X == 7))
            return;

        for (int i = 0; i < Bits.Length; i++)
        {
            Bits[i].X += 1;
        }

        lastMovement = Movement.Right;
    }

    private void UndoMoveRight() => MoveLeft();

    public void Fall()
    {
        for (int i = 0; i < Bits.Length; i++)
        {
            Bits[i].Y -= 1;
        }

        lastMovement = Movement.Fall;
    }

    private void UndoFall()
    {
        for (int i = 0; i < Bits.Length; i++)
        {
            Bits[i].Y += 1;
        }
    }

    public void UndoLastMovement()
    {
        Action<Shape> compensatingMove = lastMovement switch
        {
            Movement.Left => shape => shape.UndoMoveLeft(),
            Movement.Right => shape => shape.UndoMoveRight(),
            Movement.Fall => shape => shape.UndoFall(),
            Movement.None => shape => { }
        };
        compensatingMove.Invoke(this);
    }

    public bool WillBlock(Shape other) 
        => Bits.Any(x => other.Bits.Contains(x));

    public void SetY(int y)
    {
        for (int i = 0; i < Bits.Length; i++)
        {
            Bits[i].Y += (y - 1);
        }
    }

    public int GetHeighestBit() => Bits.Max(bit => bit.Y);
}

public class Line : Shape
{
    public Line()
    {
        /// ####
        Bits = new[]
        {
            new Point(1, 1),
            new Point(2, 1),
            new Point(3, 1),
            new Point(4, 1)
        };
    }
}

public class Plus : Shape
{
    public Plus()
    {
        ///  # 
        /// ###
        ///  #
        Bits = new[]
        {
            new Point(2, 3),
            new Point(3, 2),
            new Point(2, 2),
            new Point(1, 2),
            new Point(2, 1)
        };
    }
}

public class Corner : Shape
{
    public Corner()
    {
        ///   #
        ///   #
        /// ###
        Bits = new[]
        {
            new Point(3, 3),
            new Point(3, 2),
            new Point(3, 1),
            new Point(2, 1),
            new Point(1, 1)
        };
    }
}

public class Bar : Shape
{
    public Bar()
    {
        /// #
        /// #
        /// #
        /// #
        Bits = new[]
        {
            new Point(1, 4),
            new Point(1, 3),
            new Point(1, 2),
            new Point(1, 1),
        };
    }
}

public class Square : Shape
{
    public Square()
    {
        /// ##
        /// ##
        Bits = new[]
        {
            new Point(2, 2),
            new Point(1, 2),
            new Point(2, 1),
            new Point(1, 1),
        };
    }
}