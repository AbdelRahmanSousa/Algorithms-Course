using Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using GraphGenerator;

namespace Problem
{

    public class Problem : ProblemBase, IProblem
    {
        #region ProblemBase Methods
        public override string ProblemName { get { return "PlagiarismChecking"; } }

        public override void TryMyCode()
        {            

            //Case1
            Tuple<string, string>[] edges1 = new Tuple<string,string>[3];
            edges1[0] = new Tuple<string, string>("1", "4");
            edges1[1] = new Tuple<string, string>("4", "5");
            edges1[2] = new Tuple<string, string>("2", "3");
            Tuple<string, string> query11 = new Tuple<string, string>("1", "3");
            int expected11 = 0;
            int output11 = PlagiarismChecking.CheckPlagiarism(edges1, query11);
            PrintCase(edges1, query11, output11, expected11);

            Tuple<string, string> query12 = new Tuple<string, string>("5", "4");
            int expected12 = 1;
            int output12 = PlagiarismChecking.CheckPlagiarism(edges1, query12);
            PrintCase(edges1, query12, output12, expected12);

            Tuple<string, string> query13 = new Tuple<string, string>("5", "1");
            int expected13 = 2;
            int output13 = PlagiarismChecking.CheckPlagiarism(edges1, query13);
            PrintCase(edges1, query13, output13, expected13);
            
            //Case3
            Tuple<string, string>[] edges2 = new Tuple<string, string>[6];
            edges2[0] = new Tuple<string, string>("1", "2");
            edges2[1] = new Tuple<string, string>("2", "3");
            edges2[2] = new Tuple<string, string>("5", "4");
            edges2[3] = new Tuple<string, string>("5", "6");
            edges2[4] = new Tuple<string, string>("3", "5");
            edges2[5] = new Tuple<string, string>("4", "2");
            Tuple<string, string> query21 = new Tuple<string, string>("1", "5");
            int expected21 = 3;
            int output21 = PlagiarismChecking.CheckPlagiarism(edges2, query21);
            PrintCase(edges2, query21, output21, expected21);

            Tuple<string, string> query22 = new Tuple<string, string>("5", "3");
            int expected22 = 1;
            int output22 = PlagiarismChecking.CheckPlagiarism(edges2, query22);
            PrintCase(edges2, query22, output22, expected22);
            
            //Case4
            Tuple<string, string>[] edges3 = new Tuple<string, string>[11];
            edges3[0] = new Tuple<string, string>("1", "5");
            edges3[1] = new Tuple<string, string>("1", "4");
            edges3[2] = new Tuple<string, string>("1", "3");
            edges3[3] = new Tuple<string, string>("1", "2");
            edges3[4] = new Tuple<string, string>("2", "3");
            edges3[5] = new Tuple<string, string>("3", "4");
            edges3[6] = new Tuple<string, string>("4", "5");
            edges3[7] = new Tuple<string, string>("5", "2");
            edges3[8] = new Tuple<string, string>("6", "7");
            edges3[9] = new Tuple<string, string>("6", "8");
            edges3[10] = new Tuple<string, string>("8", "7");

            Tuple<string, string> query31 = new Tuple<string, string>("2", "4");
            int expected31 = 2;
            int output31 = PlagiarismChecking.CheckPlagiarism(edges3, query31);
            PrintCase(edges3, query31, output31, expected31);

            Tuple<string, string> query32 = new Tuple<string, string>("6", "4");
            int expected32 = 0;
            int output32 = PlagiarismChecking.CheckPlagiarism(edges3, query32);
            PrintCase(edges3, query32, output32, expected32);

            Tuple<string, string> query33 = new Tuple<string, string>("5", "1");
            int expected33 = 1;
            int output33 = PlagiarismChecking.CheckPlagiarism(edges3, query33);
            PrintCase(edges3, query33, output33, expected33);
        }

        

        Thread tstCaseThr;
        bool caseTimedOut ;
        bool caseException;

        protected override void RunOnSpecificFile(string fileName, HardniessLevel level, int timeOutInMillisec)
        {
            int testCases;
            int actualResult = int.MinValue;
            int output = int.MinValue;

            FileStream file = new FileStream(fileName, FileMode.Open, FileAccess.Read);

            StreamReader sr = new StreamReader(file);
            string line = sr.ReadLine();
            testCases = int.Parse(line);
            int totalCases = testCases;
            int correctCases = 0;
            int wrongCases = 0;
            int timeLimitCases = 0;
            bool readTimeFromFile = false;
            if (timeOutInMillisec == -1)
            {
                readTimeFromFile = true;
            }
            int i = 1;
            while (testCases-- > 0)
            {
                line = sr.ReadLine();
                string[] lineParts = line.Split(',');
                var query = new Tuple<string, string>(lineParts[0], lineParts[1]);
                int e = int.Parse(sr.ReadLine());
                
                var edges = new Tuple<string, string>[e];
                for (int j = 0; j < e; j++)
                {
                    line = sr.ReadLine();
                    lineParts = line.Split(',');
                    edges[j] = new Tuple<string, string>(lineParts[0], lineParts[1]);
                }
                line = sr.ReadLine();
                actualResult = int.Parse(line);
                caseTimedOut = true;
                caseException = false;
                {
                    tstCaseThr = new Thread(() =>
                    {
                        try
                        {
                            Stopwatch sw = Stopwatch.StartNew();
                            output = PlagiarismChecking.CheckPlagiarism(edges, query);
                            sw.Stop();
                            //PrintCase(vertices,edges, output, actualResult);
                            Console.WriteLine("|E| = {0}, time in ms = {1}", edges.Length, sw.ElapsedMilliseconds);
                            Console.WriteLine("{0}", output);
                        }
                        catch
                        {
                            caseException = true;
                            output = int.MinValue;
                        }
                        caseTimedOut = false;
                    });

                    //StartTimer(timeOutInMillisec);
                    if (readTimeFromFile)
                    {
                        timeOutInMillisec = int.Parse(sr.ReadLine().Split(':')[1]);
                    }
                    tstCaseThr.Start();
                    tstCaseThr.Join(timeOutInMillisec);
                }

                if (caseTimedOut)       //Timedout
                {
                    Console.WriteLine("Time Limit Exceeded in Case {0}.", i);
					tstCaseThr.Abort();
                    timeLimitCases++;
                }
                else if (caseException) //Exception 
                {
                    Console.WriteLine("Exception in Case {0}.", i);
                    wrongCases++;
                }
                else if (output == actualResult)    //Passed
                {
                    Console.WriteLine("Test Case {0} Passed!", i);
                    correctCases++;
                }
                else                    //WrongAnswer
                {
                    Console.WriteLine("Wrong Answer in Case {0}.", i);
                    Console.WriteLine(" your answer = {0}, correct answer = {1}", output, actualResult);
                    wrongCases++;
                }

                i++;
            }
            file.Close();
            sr.Close();
            Console.WriteLine();
            Console.WriteLine("# correct = {0}", correctCases);
            Console.WriteLine("# time limit = {0}", timeLimitCases);
            Console.WriteLine("# wrong = {0}", wrongCases);
            Console.WriteLine("\nFINAL EVALUATION (%) = {0}", Math.Round((float)correctCases / totalCases * 100, 0)); 
        }

        protected override void OnTimeOut(DateTime signalTime)
        {
        }

        public override void GenerateTestCases(HardniessLevel level, int numOfCases, bool includeTimeInFile = false, float timeFactor = 1)
        {
            throw new NotImplementedException();

        }

        #endregion

        #region Helper Methods
        private static void PrintCase(Tuple<string, string>[] edges, Tuple<string, string> query, int output, int expected)
        {
            Console.WriteLine("Edges: ");
            for (int i = 0; i < edges.Length; i++)
            {
                Console.WriteLine("{0}, {1}", edges[i].Item1, edges[i].Item2);
            }

            Console.WriteLine("QUERY: {0}, {1}", query.Item1, query.Item2);
            Console.WriteLine("Output: {0}", output);
            Console.WriteLine("Expected: {0}", expected);
            if (output == expected)    //Passed
            {
                Console.WriteLine("CORRECT");
            }
            else                    //WrongAnswer
            {
                Console.WriteLine("WRONG");
            }
            Console.WriteLine();
        }
        
        #endregion
   
    }
}
