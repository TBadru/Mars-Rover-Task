using System; // using classes from system namespace
using System.Collections.Generic;
using System.Collections; 
using System.Linq;
using System.Text;

namespace MarsRoverTask // container for classes
{
    static class Program
    {

        // declaring variables with the "public" access modifier to make code accessible to all classes

        public static int i_ptX = 0;
        public static int i_ptY = 0;
        public static String s_direction = ""; // square directly from the assumption variable decalred

        // declaring the rover's direction and command variables with the "private" access modifier to make code accessible within the same class

        private static String validDirections = "NSEW"; // All 4 Cardinal Compass Point variable
        private static String northDirection = "N";     // North Cardinal Compass Point variable
        private static String southDirection = "S";     // South Cardinal Compass Point variable
        private static String eastDirection = "E";      // East Cardinal Compass Point variable
        private static String westDirection = "W";      // West Cardinal Compass Point variable
        private static String validCommands = "LRM";    // North Cardinal Compass Point variable 
        private static String moveLeft = "L";           // Rover spins Left by 90 degree variable declaration
        private static String moveRight = "R";          // Rover spins Right by 90 degree variable declaration
        private static String moveCommand = "M";        // Move Forward one grid point variable declaration
        private static Boolean isDebugChecked = false;


        private static void debugOut(String msg)
        {
            if (isDebugChecked)
            {
                Console.WriteLine(msg);
            }
        }

        private static String publish_values()
        {
            String s = i_ptX + " " + i_ptY + " " + s_direction;
            Console.WriteLine(s);
            return s;
        }

        private static void doMove()
        {
            switch (s_direction) // switch statement
            {
                case "N":
                    debugOut("doMove().1 --> (s_direction == northDirection)");
                    i_ptY = i_ptY + 1;
                    break; // the switch terminates, and the flow of control jumps to the next line following the switch statement.
                case "E":
                    debugOut("doMove().2 --> (s_direction == eastDirection)");
                    i_ptX = i_ptX + 1;
                    break;
                case "S":
                    debugOut("doMove().3 --> (s_direction == southDirection)");
                    i_ptY = i_ptY - 1;
                    break;
                case "W":
                    debugOut("doMove().4 --> (s_direction == westDirection)");
                    i_ptX = i_ptX - 1;
                    break;
            }
        }

        // spin/rotate rover
        private static void doSpin(String d)
        {
            s_direction = ((validDirections.IndexOf(d) > -1) || (validCommands.IndexOf(d) > -1)) ? d : s_direction;
            debugOut("doSpin().1 --> d=" + d + ", s_direction=" + s_direction);
        }

        // move rover
        private static void doCommand(String c)
        {
            debugOut("doCommand().1 --> c=" + c);
            switch (c)
            {
                case "L":
                    debugOut("doCommand().2 --> (c == moveLeft)");
                    switch (s_direction)
                    {
                        case "N":
                            debugOut("doCommand().3 --> doSpin(westDirection)");
                            doSpin(westDirection);
                            break;
                        case "W":
                            debugOut("doCommand().4 --> doSpin(southDirection)");
                            doSpin(southDirection);
                            break;
                        case "S":
                            debugOut("doCommand().5 --> doSpin(eastDirection)");
                            doSpin(eastDirection);
                            break;
                        case "E":
                            debugOut("doCommand().6 --> doSpin(northDirection)");
                            doSpin(northDirection);
                            break;
                    }
                    break;
                case "R":
                    debugOut("doCommand().7 --> (c == moveRight)");
                    switch (s_direction)
                    {
                        case "N":
                            debugOut("doCommand().8 --> doSpin(eastDirection)");
                            doSpin(eastDirection);
                            break;
                        case "E":
                            debugOut("doCommand().9 --> doSpin(southDirection)");
                            doSpin(southDirection);
                            break;
                        case "S":
                            debugOut("doCommand().10 --> doSpin(westDirection)");
                            doSpin(westDirection);
                            break;
                        case "W":
                            debugOut("doCommand().11 --> doSpin(northDirection)");
                            doSpin(northDirection);
                            break;
                    }
                    break;
                case "M":
                    debugOut("doCommand().12 --> (c == moveCommand)");
                    doMove();
                    break;
            }
        }

        private static bool IsInteger(string theValue)
        {
            try
            {
                Convert.ToInt32(theValue); // represents 32 bit signed integers from negative to positive 
                return true;
            }
            catch
            {
                return false;
            }
        }

        private static String parseCommand(String c)
        {
            String aTok;
            String aCmd;
            Boolean b;
            Stack items = new Stack();
            String[] toks = c.Split(' ');
            for (int i = 0; i < toks.Length; i++)
            {
                aTok = toks[i];
                debugOut("parseCommand().1 aTok=" + aTok);
                if (aTok.Length > 1)
                {
                    for (var j = 0; j < aTok.Length; j++)
                    {
                        aCmd = aTok.Substring(j, 1);
                        debugOut("parseCommand().2 aCmd=" + aCmd);
                        doCommand(aCmd);
                    }
                }
                else
                {
                    b = IsInteger(aTok);
                    debugOut("parseCommand().3 --> b=" + b);
                    if (b)
                    {
                        items.Push(aTok);
                        debugOut("parseCommand().4 items.Count=" + items.Count);
                        if (items.Count == 2)
                        {
                            i_ptY = Convert.ToInt32(items.Pop());
                            i_ptX = Convert.ToInt32(items.Pop());
                        }
                    }
                    else if (validDirections.IndexOf(aTok) > -1)
                    {
                        s_direction = aTok;
                        debugOut("parseCommand().5 s_direction=" + s_direction);
                    }
                    else if (validCommands.IndexOf(aTok) > -1)
                    {
                        debugOut("parseCommand().6 doCommand(" + aTok + ")");
                        doCommand(aTok);
                    }
                }
            }
            return publish_values();
        }

        // 
        static void Main(string[] args)
        {
            String cmd = "5 5";
            String expected = "5 5";
            String x = parseCommand(cmd);
            if (x != expected) //  if statement to specify a block of code to be executed, if specified condition is true
            {
                Console.WriteLine("error message #1");
            }

            cmd = "1 2 N";
            expected = "1 2 N";
            x = parseCommand(cmd);
            if (x != expected)
            {
                Console.WriteLine("error message #2");
            }

            cmd = "LMLMLMLMM";
            expected = "1 3 N";
            x = parseCommand(cmd);
            if (x != expected)
            {
                Console.WriteLine("error message #3");
            }

            cmd = "3 3 E";
            expected = "3 3 E";
            x = parseCommand(cmd);
            if (x != expected)
            {
                Console.WriteLine("error message #4");
            }

            cmd = "MMRMMRMRRM"; 
            expected = "5 1 E";
            x = parseCommand(cmd);
            if (x != expected)
            {
                Console.WriteLine("error message #5");
            }

          
        }
    }
}
