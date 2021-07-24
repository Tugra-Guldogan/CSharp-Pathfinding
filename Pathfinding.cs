using KLibrary.KException;
using System;
using KLibrary;
using System.Threading;

namespace KLibrary
{
    /// <summary>
    /// Use to make a Pathfinding with chosen Width and Height.
    /// </summary>
    public class Pathfinding
    {
        private int Width { get; set; }
        private int Height { get; set; }
        private readonly char[,] Grid;
        private Pos Pos { get; set; } = new Pos();
        const int NORMALMOVE = 10;
        const int DIAGONALMOVE = 14;
        private readonly int FirstPosX;
        private readonly int FirstPosY;
        private readonly int[] startPos, endPos;

        /// <summary>
        /// Pathfinding Constructor.
        /// </summary>
        /// <param name="width">Width of the grid.</param>
        /// <param name="height">Height of the grid.</param>
        /// <param name="startPos">Grid StartPos. (x, y)</param>
        /// <param name="endPos">Grid EndPos. (x,y)</param>
        public Pathfinding(int width, int height,int[] sp, int[] ep)
        {
            try
            {
                if (width < 1 || height < 1)
                    throw new PFException(0);
                Width = width;
                Height = height;
                Grid = new char[width,height];
                startPos = sp;
                endPos = ep;
                Pos.StartX = startPos[0];
                Pos.StartY = startPos[1];
                Pos.EndX = endPos[0];
                Pos.EndY = endPos[1];
                FirstPosX = Pos.StartX;
                FirstPosY = Pos.StartY;
            }
            catch { Console.WriteLine(new Exception().Message); }
        }

        private void SetGrid(int startX, int startY, int endX, int endY)
        {
            for (int i = 0; i < Height; i++)
                Console.Write("  " + i + " ");
            Console.WriteLine();
            for (int i = 0; i < Width; i++)
                for (int j = 0; j < Height; j++)
                    Grid[i, j] = ' ';

            Grid[startY, startX] = 'O';
            Grid[endY, endX] = 'X';
        }

        private enum Direction
        {
            LEFT,RIGHT,
            UP,DOWN,
            DIAGONALUPRIGHT,DIAGONALUPLEFT,
            DIAGONALDOWNRIGHT,DIAGONALDOWNLEFT
        }
        /// <summary>
        /// To launch and find way for the pathfinding
        /// </summary>
        public void Launch()
        {
            SetGrid(Pos.StartX,Pos.StartY,Pos.EndX,Pos.EndY);
            Show();
            ShowInfo();
            WaitForStart();
            StartFindWay();
            ResetPos();
            StartFindWayForDiagonal();
            bool b = Pos.FirstPoints < Pos.SecondPoints;
            SetGridToAllMovement(b);
            EndInfo(b);
        }

        private void EndInfo(bool first)
        {
            int a = Pos.FirstMovement.Count / 2;
            if (!first)
                a = Pos.SecondMovement.Count / 2;
            Console.WriteLine("Done! Took " + a + " movement.");
        }

        private void ShowInfo()
        {
            Console.WriteLine("STARTPOS POSITION : " + 
                Pos.StartX + " " + Pos.StartY);
            Console.WriteLine("ENDPOS POSITION : " +
                Pos.EndX + " " + Pos.EndY);
            Console.WriteLine("O => START POSITION\n" +
                "X => END POSITION\n" +
                "* => PATH FROM 'X' TO 'O'");
        }

        private void ResetPos()
        {
            Pos.StartX = FirstPosX;
            Pos.StartY = FirstPosY;
        }

        private void SetGridToAllMovement(bool first)
        {
            if (first)
            {
                for (int i = 0; i < Pos.FirstMovement.Count; i++)
                    if (i % 2 == 0)
                    {
                        Grid[Pos.FirstMovement[i], Pos.FirstMovement[i + 1]] = '*';
                        Show();
                        Thread.Sleep(250);
                    }
            }
            else
            {
                for (int i = 0; i < Pos.SecondMovement.Count; i++)
                    if (i % 2 == 0)
                    {
                        Grid[Pos.SecondMovement[i], Pos.SecondMovement[i + 1]] = '*';
                        Show();
                        Thread.Sleep(250);
                    }
            }

        }

        private void StartFindWayForDiagonal()
        {
            Direction d = Direction.DIAGONALDOWNLEFT;
            while (Pos.StartX != Pos.EndX &&
                Pos.StartY != Pos.EndY)
            {
                if (Pos.StartX < Pos.EndX && Pos.StartY < Pos.EndY)
                    d = Direction.DIAGONALDOWNRIGHT;
                else if (Pos.StartX < Pos.EndX && Pos.StartY > Pos.EndY)
                    d = Direction.DIAGONALUPRIGHT;
                else if (Pos.StartX > Pos.EndX && Pos.StartY > Pos.EndY)
                    d = Direction.DIAGONALUPLEFT;
                MoveDiagonal(d);
            }

            while (Pos.StartX != Pos.EndX)
            {
                if (Pos.StartX > Pos.EndX) d = Direction.LEFT;
                else d = Direction.RIGHT;
                MoveNormal(d, false);
            }

            while (Pos.StartY != Pos.EndY)
            {
                if (Pos.StartY > Pos.EndY) d = Direction.UP;
                else d = Direction.DOWN;
                MoveNormal(d, false);
                //Console.WriteLine("MOVENORMAL DETECTED WITH DIRECTION : " + d);
            }
        }

        private void StartFindWay()
        {
            // UP or DOWN or RIGHT or LEFT move.
            Direction d = Direction.RIGHT;
            while (Pos.StartX != Pos.EndX)
            {
                if (Pos.StartX > Pos.EndX) d = Direction.LEFT;
                MoveNormal(d,true);
            }
            while(Pos.StartY != Pos.EndY)
            {
                if (Pos.StartY > Pos.EndY) d = Direction.UP;
                else d = Direction.DOWN;
                //Console.WriteLine("2");
                MoveNormal(d,true);
                //Console.WriteLine("MOVENORMAL DETECTED WITH DIRECTION : " + d);
            }
        }

        private void MoveDiagonal(Direction d)
        {
            switch (d)
            {
                case Direction.DIAGONALDOWNLEFT:
                    Pos.StartX--;
                    Pos.StartY++;
                    break;
                case Direction.DIAGONALDOWNRIGHT:
                    Pos.StartY++;
                    Pos.StartX++;
                    break;
                case Direction.DIAGONALUPLEFT:
                    Pos.StartX--;
                    Pos.StartY--;
                    break;
                case Direction.DIAGONALUPRIGHT:
                    Pos.StartX++;
                    Pos.StartY--;
                    break;
            }

            Pos.SecondPoints += DIAGONALMOVE;
            Pos.SecondMovement.Add(Pos.StartY);
            Pos.SecondMovement.Add(Pos.StartX);
        }

        private void MoveNormal(Direction d, bool first)
        {
            switch (d)
            {
                case Direction.LEFT:
                    Pos.StartX--;
                    break;
                case Direction.RIGHT:
                    Pos.StartX++;
                    break;
                case Direction.DOWN:
                    Pos.StartY++;
                    break;
                case Direction.UP:
                    Pos.StartY--;
                    break;
            }

            if (first)
            {
                Pos.FirstPoints += NORMALMOVE;
                Pos.FirstMovement.Add(Pos.StartY);
                Pos.FirstMovement.Add(Pos.StartX);
            }
            else
            {
                Pos.SecondPoints += NORMALMOVE;
                Pos.SecondMovement.Add(Pos.StartY);
                Pos.SecondMovement.Add(Pos.StartX);
            }
        }
        private void WaitForStart()
        {
            Console.WriteLine("Press enter to continue.");
            Console.ReadLine();
        }

        private void Show()
        {
            string result = " ";
            result = CreateStrip(result);
            result += "\n";
            for(int i = 0,count = 0; i< Width; i++,count++)
            {
                result += "| ";
                for (int j = 0; j < Height; j++)
                    result += Grid[i, j] + " | ";
                result += count + "\n ";
                result = CreateStrip(result);
                result += "\n";
            }
            Console.WriteLine(result);
        }

        private string CreateStrip(string s)
        {
            for (int i = 0; i < Height * 4; i++)
                s += "-";
            return s;
        }
    }
}
