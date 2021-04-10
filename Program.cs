using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Golf
{
    class Program
    {
        public static Swing swing = new Swing();
         static void Main(string[] args)
        {
            
           
            #region Console Window Setting test
            //Console.BufferWidth = 100;
            Console.WindowWidth = 100;
            Console.WindowHeight = 50;
            Console.WindowTop = 0;
            Console.WindowLeft = 0;
            #endregion

            #region Kör i gång

            char onOf = ' ';

            while (swing.endFlag == false || (char.ToLower(onOf) != 'y' && char.ToLower(onOf) != 'n'))
            {
                Console.Write("Kör i gång Golf(y/n)");
                swing.endFlag = char.TryParse(Console.ReadLine(), out onOf);
            }
            
            Console.Clear();

            swing.printForm(ref swing.currentPosision);

            swing.currentPosision = Console.CursorTop;

            #endregion Spel

            
            while (swing.endFlag == true)
            {
                try
                {
                    swing.SelectVelocity();
                    swing.Message("            ");
                    swing.SelectAngle();
                    swing.writePost();

                    swing.MakeDecision(swing.LstPoints.Last().totalDistans);

                    swing.PrintPostBypost(1);
                }
                catch (Exception e)
                {

                    swing.Message(e.Message);
                    Console.ReadKey();
                    
                
                }
                swing.Point = new Points();
                swing.Message(":>Next Enter");
                Console.ReadKey();
                swing.Message("            ");
            }

        }
 
    }

    class Swing
    {
        const double Gravity = 9.8;
        private double angleInRadians;
        private  double cupPosition=52;
        public int counter = 0;
        public bool direction=true;
        public  int currentPosision = 0;
        public bool endFlag = false;
        double y=0;
        double x=0;
        private  Points _points= new Points();
        public Points Point { get => _points; set => _points = value; }
        public List<Points> LstPoints = new List<Points>();

        public double CalcuateDistanc()
        {
            double result = double.NaN;
            try
            {
                angleInRadians = (Math.PI / 180) * Point.Angle;
                result=Math.Round(Math.Pow(Point.Velocity, 2) / Gravity * Math.Sin(2 * angleInRadians), 2);
            }
            catch (Exception e)
            {

                throw new Exception(e.Message);

            }
            return result;


        }
        public void writePost() 
        {
            Point.Distans = CalcuateDistanc();

            if (LstPoints.Count() > 0)
                if (direction == true)
                    Point.totalDistans = LstPoints.Last().totalDistans + Point.Distans;
                else 
                {
                    Point.totalDistans = LstPoints.Last().totalDistans - Point.Distans;
                    direction = false;
                }
            else
                Point.totalDistans = Point.Distans;
            
            Point.DistanceToHolle = Math.Round( Math.Abs(cupPosition- Point.totalDistans),2);
            counter++;
            Point.SvingNo = counter;
            LstPoints.Add(Point);
        }
        public void DrawDistance()
        {
            try
            {
                printPlayeLine();
                Console.SetCursorPosition((int)Point.totalDistans, 6);
                Console.WriteLine('*');
            }
            catch (Exception e)
            {
                Message(e.Message);
                Console.ReadKey();
            }
            
        }
        public void printPlayeLine()
        {
            Console.SetCursorPosition((int)cupPosition, 5);
            Console.WriteLine("Cup");
            Console.WriteLine("                                                                                   ");
            Console.SetCursorPosition((int)cupPosition, 6);
            Console.WriteLine("O");
            Console.WriteLine("...................................................................................");
            Console.SetCursorPosition((int)cupPosition, 7);
            Console.WriteLine("+");
        } 
        public void printForm(ref int currentPosision)
        {

            printPlayeLine();
            Console.SetCursorPosition(0, 10);
            Console.Write("Select Velocity(ArroKey):");

            Console.SetCursorPosition(0, 11);
            Console.Write("Select Angle(ArroKey):");

            Console.SetCursorPosition(0, 16);
            Console.WriteLine("---------------------------------------------------------");
            Console.WriteLine("Rad Angel Velocity Distance TotalDistance DistanceToHolle");
            Console.WriteLine("---------------------------------------------------------");
            currentPosision = Console.CursorTop;
        }
        public void PrintPostBypost(int oneRecord)
        {
            if (oneRecord==1)
            {
                Console.SetCursorPosition(0, 19);
                Console.WriteLine("{0,-3} {1,-2}\t  {2,-2}\t   {3,-6}\t{4,-6}\t  {5,-6}", LstPoints.Last().SvingNo, LstPoints.Last().Angle
                                              , LstPoints.Last().Velocity, LstPoints.Last().Distans, LstPoints.Last().totalDistans, LstPoints.Last().DistanceToHolle);
            }
            else if (oneRecord == 2)
            {
                foreach (var item in LstPoints)
                {
                    Console.SetCursorPosition(0, currentPosision);
                    Console.WriteLine("{0,-3} {1,-2}\t  {2,-2}\t   {3,-6}\t{4,-6}\t  {5,-6}", item.SvingNo,item.Angle
                                      , item.Velocity, item.Distans, item.totalDistans, item.DistanceToHolle);
                    currentPosision = Console.CursorTop;
                }
            }
           
        }
        public void MakeDecision(double distance) 
        {
            if ((distance>=cupPosition-0.5) && (distance <= cupPosition+0.5 ))
            {
                DrawDistance();
                Console.SetCursorPosition((int)52, 8);
                Console.WriteLine("-->!<--");
                Message("Goal");
                PrintPostBypost(2);
                endFlag = false;

            }
            else if (distance > cupPosition)
            {
                if (LstPoints.Last().DistanceToHolle > 10)
                {
                    PrintPostBypost(2);
                    endFlag = false;
                    throw new ArgumentOutOfRangeException("Ball move too far away from the Cup.It takes you out of play.", "");
                }
                    
                DrawDistance();
                direction = false;
            }
            else if(counter>5)
            {
                PrintPostBypost(2);
                endFlag = false;
                throw new Exception("Many swings have been taken.It takes you out of play.");

            }
            else if(distance < cupPosition)
            {
                DrawDistance();
                direction = true;
            }
        }
        public double CalcCordinat()
        {
            y = (-Gravity * Math.Pow(x, 2)) / (2 * Math.Pow(Point.Velocity, 2) * Math.Pow(Math.Cos(Point.Angle), 2)) + x * Math.Tan(Point.Angle);
            return Math.Round(y, 2);
        }
        public void SelectVelocity()
        {
            if (LstPoints.Count>0)
            {
                Point.Velocity = LstPoints.Last().Velocity;
            }

            Console.SetCursorPosition(0, 10);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("Select Velocity(ArroKey):");
            Console.ResetColor();

            //Message("Use ArrowKey then Enter");
            
            Point.Velocity=ArrowkeyControl(Point.Velocity,10);

            Console.SetCursorPosition(0, 10);
            Console.ResetColor();
            Console.Write("Select Velocity(ArroKey):");
        }
        public void SelectAngle()
        {
            if (LstPoints.Count > 0)
            {
                Point.Angle = LstPoints.Last().Angle;
            }
            ConsoleKeyInfo k = new ConsoleKeyInfo();
            Console.SetCursorPosition(0, 11);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("Select Angle(ArroKey)   :");
            Console.ResetColor();

            //Message("Use ArrowKey then Enter");
            Point.Angle = ArrowkeyControl(Point.Angle,11);

            Console.SetCursorPosition(0, 11);
            Console.ResetColor();
            Console.Write("Select Angle(ArroKey)   :");

        }
        public double ArrowkeyControl(double p, int cell)
        {
            ConsoleKeyInfo k = new ConsoleKeyInfo();
            double d = 0.0;
            if (p > 0)
                d = p;
                do
                {
                    k = Console.ReadKey();
                    try
                    {
                        switch (k.Key)
                        {
                            case ConsoleKey.RightArrow:
                                d++;
                                break;
                            case ConsoleKey.LeftArrow:
                                d--;
                                if(d < 0)
                                {
                                    d++;
                                    throw new ArgumentOutOfRangeException("ArgumentOutOfRangeException","");
                                }
                                break;
                            case ConsoleKey.DownArrow:
                                d -= 0.1;
                                if (d < 0)
                                {
                                    d += 0.1;
                                    throw new ArgumentOutOfRangeException("ArgumentOutOfRangeException", "");
                                }
                                break;
                            case ConsoleKey.UpArrow:
                                d += 0.1;
                                break;

                            case ConsoleKey.Enter:
                                break;
                            default:
                            {
                                Console.SetCursorPosition(25, cell);
                                Console.Write("         ");
                                throw new FormatException();
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Message(e.Message);
                        //Console.ReadKey();
                    
                    }
  
                    Console.SetCursorPosition(26, cell);
                    d = Math.Round(d, 2);
                    Console.Write(d);
                } while (k.Key != ConsoleKey.Enter);
            return d;
        }
        public void Message(string message)
        {
            for (int i =12; i < 16; i++)
            {
                Console.SetCursorPosition(0, 12);
                Console.WriteLine("                                                                                                   ");
            }
            //Console.SetCursorPosition(0, 12);
            //Console.WriteLine("                                                                                                   ");
            //Console.SetCursorPosition(0, 13);
            //Console.WriteLine("                                                                                                   ");
            //Console.SetCursorPosition(0, 14);
            //Console.WriteLine("                                                                                                   ");
            //mydelay(500);
            //Console.SetCursorPosition(0, 15);
            //Console.WriteLine("                                                                                                   ");

            string helpMessage = "";
            string transfer = "";
            if (message.Length>80)
            {
                transfer = message.Substring( 0, 80);
                message =message.Remove(0, 80);
                helpMessage = message.Substring(0,message.Length);
                message = transfer;
            }
           
            Console.SetCursorPosition(0, 12);
            Console.WriteLine("                                                                                                   ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.SetCursorPosition(0, 12);
            Console.Write(message);
            
            Console.SetCursorPosition(0, 13);
            Console.Write(helpMessage);
            
            mydelay(50);
            Console.ResetColor();

        }
        public void mydelay(double seconds)
        {
            DateTime d = DateTime.Now.AddMilliseconds(seconds);
            do { } while (DateTime.Now < d);
        }
  
    }
    public  class Points
    {
        public  double Angle;
        public  double Velocity;
        public  double Distans;
        public  double totalDistans;
        public double DistanceToHolle;
        public int SvingNo;
        
    }

    #region xy
    //Point point = new Point();
    //Point[] arrPoints = new Point[100];
    //for (int i = 0; i < 100; i++)
    //{
    //    s.X = i;
    //    point.x = s.X;
    //    point.y = s.c();
    //    arrPoints[i] = point;

    //}
    //foreach (var item in arrPoints)
    //{
    //    Console.WriteLine($"X = {item.x} Y = {item.y}");
    //}
    //for (int j = 0; j < 50; j++)
    //{
    //    if (j % 5 == 0)
    //    {
    //        Console.SetCursorPosition((int)arrPoints[j].x / 10, 100 - ((int)arrPoints[j].y) / 10);
    //        Console.WriteLine('.');

    //    }
    //}
    //============================
    //public  class Point
    //{
    //    public double a;
    //    public double b;

    //    public double x;
    //    public double y;
    //    public double r;

    //    public Point() { }
    //    public double DistanceTo(Point pt) 
    //    {
    //        pt.y = Math.Sqrt(Math.Pow(pt.r, 2) - Math.Pow((pt.x - a), 2));

    //        return Math.Round(pt.y - b,1);
    //    }
    //}
    #endregion
}
