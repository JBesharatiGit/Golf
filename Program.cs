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

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine();
            Console.WriteLine("[I den här spel '*' visar boll position och 'O' visar Cup.]");
            Console.WriteLine("[För att öka/minska Angle och Velocity anväder Arrowkey]");
            Console.WriteLine("[ArrowKey L/R byter Heltal och U/D byter decimal del av tal.]");
            Console.WriteLine("[För att confirmera varje steg använder ReturnKey.]");

            Console.WriteLine("----------------------------------------------------------------------");
            Console.ResetColor();

            char onOf = ' ';
            bool answer = false;
            while (answer==false&&char.ToLower(onOf)!='y' && char.ToLower(onOf) != 'n')
            {
                Console.Write("Start Golf(y/n)");
                char.TryParse(Console.ReadLine(),out  onOf);
            }
            
            if (char.ToLower(onOf) == 'y')
            {
                swing.ExitFlag = false;
                swing.endFlag = true;
                Console.Clear();
                swing.PrintForm(ref swing.currentPosision);
                swing.currentPosision = Console.CursorTop;
            }
            else
                swing.ExitFlag = true;

            #endregion Spel

            while (swing.ExitFlag == false)
            {
                while (swing.endFlag == true)
                {
                    try
                    {
                        swing.SelectVelocity();//Sätter velocity värde för a swing
                        swing.Message("            ");
                        swing.SelectAngle();//Sätter Angle värde för a swing
                        swing.WritePost();

                        swing.MakeDecision(swing.LstPoints.Last().totalDistans);//bestämmer användare vinner eller förlorar,hur mycket spelaren försökte och bollen hamnar bort.

                        swing.PrintPostBypost(1);//Skriver ut resultat av verje swing
                    }
                    catch (Exception e)
                    {
                        swing.Message(e.Message);
                        Console.ReadKey();
                    }
                    if (swing.endFlag == true)
                    {
                        swing.Point = new Points();
                        swing.Message(":>Next Enter");
                        Console.ReadKey();
                        swing.Message("");
                    }
                }

                NextPlay();//Frågar från användaren om vill försöka en spel till
            }
         }
        public static void NextPlay()
        {
            swing.Message("Continue or end ?(y/n)");
            ConsoleKeyInfo k; //= new ConsoleKeyInfo();
            do
            {
                k = Console.ReadKey();
            } while (char.ToLower(k.KeyChar) != 'y' && char.ToLower(k.KeyChar) != 'n');
            
            if (char.ToLower(k.KeyChar) == 'y')
            {
                swing = new Swing();
                swing.ExitFlag = false;
                swing.endFlag = true;
                Console.Clear();
                swing.PrintForm(ref swing.currentPosision);
                swing.currentPosision = Console.CursorTop;
                
                //swing.LstPoints = new List<Points>();
                
            }
            else
                swing.ExitFlag = true;
        }

    }

    class Swing
    {
        const double Gravity = 9.8;
        private double angleInRadians;
        private const double cupPosition=52;
        public int counter = 0;
        public bool direction=true;
        public  int currentPosision = 0;
        public bool ExitFlag = true;
        public bool endFlag = false;
        private  Points _points= new Points();
        public Points Point { get => _points; set => _points = value; }
        public List<Points> LstPoints = new List<Points>();

        public double CalcuateDistanc()
        {
            double result; //= double.NaN;
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


        }//Räknar ut avstånd som bollen kommer att röra sig på grund av Velocity och Angle som valde.
        public void WritePost() //Lägger varje sting till Collection.
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
                PrintPlayeLine();
                Console.SetCursorPosition((int)Point.totalDistans, 6);
                Console.WriteLine('*');
            }
            catch (Exception e)
            {
                Message(e.Message);
                Console.ReadKey();
            }
            
        }//Visar positionen som bollen hamnar(med hjälp av *) 
        public void PrintPlayeLine()//skriver ut spel linje  
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
        public void PrintForm(ref int currentPosision)
        {

            PrintPlayeLine();
            Console.SetCursorPosition(0, 10);
            Console.Write("Select Velocity(ArroKey):");

            Console.SetCursorPosition(0, 11);
            Console.Write("Select Angle(ArroKey):");

            Console.SetCursorPosition(0, 16);
            Console.WriteLine("---------------------------------------------------------");
            Console.WriteLine("Rad Angel Velocity Distance TotalDistance DistanceToHolle");
            Console.WriteLine("---------------------------------------------------------");
            currentPosision = Console.CursorTop;
        }//striver ut tabel form
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

        }//Skriver ut resultat av verje swing and total match resultat
        public void MakeDecision(double distance) 
        {
            if ((distance>=cupPosition-0.2) && (distance <= cupPosition+0.2 ))
            {
                DrawDistance();
                Console.SetCursorPosition((int)cupPosition-4, 8);
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
        }//bestämmer användare vinner eller förlorar,hur mycket spelaren försökte och bollen hamnar bort och till sist ska spelen sluta eller förtsätta.
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

            Point.Velocity=ArrowkeyControl(Point.Velocity,10);

            Console.SetCursorPosition(0, 10);
            Console.ResetColor();
            Console.Write("Select Velocity(ArroKey):");
        }//Sätter velocity värde för a swing
        public void SelectAngle()
        {
            if (LstPoints.Count > 0)
            {
                Point.Angle = LstPoints.Last().Angle;
            }
            Console.SetCursorPosition(0, 11);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("Select Angle(ArroKey)   :");
            Console.ResetColor();

            Point.Angle = ArrowkeyControl(Point.Angle,11);

            Console.SetCursorPosition(0, 11);
            Console.ResetColor();
            Console.Write("Select Angle(ArroKey)   :");

        }//Sätter Angle värde för a swing
        public double ArrowkeyControl(double p, int cell)
        {
            ConsoleKeyInfo k ;
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
                                 Message(" ");
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
                                Message(" ");
                                d += 0.1;
                                break;

                            case ConsoleKey.Enter:
                                break;
                            default:
                            {
                                Console.SetCursorPosition(26, cell);
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
                    Console.Write("         ");
                    Console.SetCursorPosition(26, cell);
                    d = Math.Round(d, 2);
                    Console.Write(d);
                    
                } while (k.Key != ConsoleKey.Enter);
            return d;
        }//Byter Angle/Velocity värde med hjälp av Arrokeys(R/L/U/D) 
        public void Message(string message)
        {
            for (int i =12; i < 16; i++)
            {
                Console.SetCursorPosition(0, i);
                Console.WriteLine("                                                                                                   ");
            }

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
            
            Mydelay(50);
            Console.ResetColor();

        }//Skriver ut ut meddelande
        public void Mydelay(double seconds)
        {
            DateTime d = DateTime.Now.AddMilliseconds(seconds);
            do { } while (DateTime.Now < d);
        }//Gör försening
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

  
}
