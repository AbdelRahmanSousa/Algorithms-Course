using Helpers;
using Problem;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helpers
{
    class Program
    {
        static void oversizedCase() {
            int multiplier = 1080;
            int testCases;
            int N = 0;
            byte[] output = null;
            byte[] actualResult = null;
            int j = 0;

            Stream s = new FileStream("IntegerMultiplication_Hard.txt", FileMode.Open);
            BinaryReader br = new BinaryReader(s);

            testCases = br.ReadInt32();
            N = br.ReadInt32();
            long length = multiplier * N;
            byte[] X = new byte[length];
            byte[] Y = new byte[length];
            int i = N;
            
                for (j = 0; j < N; j++)
                {
                    X[j] = br.ReadByte();
                }
                for (j = 0; j < N; j++)
                {
                    Y[j] = br.ReadByte();
                }

            while (i < length) {
                Array.Copy(X,0,X,i,N);
                Array.Copy(Y, 0, Y, i, N);
                i += N;
            }
            Stopwatch sw = new Stopwatch();
            sw.Start();
            output = IntegerMultiplication.IntegerMultiply(X,Y,N * multiplier);
            sw.Stop();
            Console.WriteLine("OverSized " + sw.ElapsedMilliseconds);
        }

        static int timeOutInMillisec = 400000 ;
        static void Main(string[] args)
        {
            Console.Write("\nEnter your choice: [1] Trial Cases [2] Sample Test Cases [3] Complete Test Cases... [any key for exit] ");
            ConsoleKeyInfo cki = Console.ReadKey();
            Console.WriteLine();
            while (cki.Key == ConsoleKey.D1 || cki.Key == ConsoleKey.D2 || cki.Key == ConsoleKey.D3)
            {
                IProblem problem = null;

                int hardniessLevelSelection = cki.KeyChar - '0';

                problem = new Problem.Problem();

                //problem.GenerateTestCases(HardniessLevel.Easy, 5);
                //problem.GenerateTestCases(HardniessLevel.Hard, 1);

                ExcuteProblem(problem, hardniessLevelSelection, timeOutInMillisec);
                Console.WriteLine();
                Console.Write("\nEnter your choice: [1] Trial Cases [2] Sample Test Cases [3] Complete Test Cases... [any key for exit] ");
                cki = Console.ReadKey();
                Console.WriteLine();
            }
           
        }


        private static void ExcuteProblem(IProblem problem, int hardniessLevelSelection, int timeOutInMillisec)
        {
            switch (hardniessLevelSelection)
            {
                case 1:
                    problem.TryMyCode();
                    break;
                case 2:
                    problem.Run(HardniessLevel.Easy, timeOutInMillisec);
                    break;
                case 3:
                    problem.Run(HardniessLevel.Hard, timeOutInMillisec);
                    break;
                default:
                    Console.WriteLine("Invalid Input");
                    break;
            }
            
        }
    }
}
