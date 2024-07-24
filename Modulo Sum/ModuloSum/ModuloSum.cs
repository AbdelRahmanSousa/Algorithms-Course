using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Problem
{
    // *****************************************
    // DON'T CHANGE CLASS OR FUNCTION NAME
    // YOU CAN ADD FUNCTIONS IF YOU NEED TO
    // *****************************************
    public class ModuloSum
    {
        public static bool SolveValue(int[] items, int N, int M)
        {
            HashSet<int> remainders = new HashSet<int>();
            remainders.Add(0); // Include 0 as the initial remainder to handle subarrays starting from index 0

            int sum = 0;
            for (int i = 0; i < N; i++)
            {
                sum = (sum + items[i]) % M;
                if (remainders.Contains(sum))
                    return true;

                HashSet<int> newRemainders = new HashSet<int>(remainders);
                foreach (int remainder in remainders)
                {
                    int newRemainder = (remainder + items[i]) % M;
                    newRemainders.Add(newRemainder);
                }
                remainders = newRemainders;
            }

            return false;
        }

        public static int[] ConstructSolution(int[] items, int N, int M)
        {
            Dictionary<int, List<int>> remaindersMap = new Dictionary<int, List<int>>();
            remaindersMap.Add(0, new List<int>() { -1 }); // Include -1 as the initial index to handle subarrays starting from index 0

            int sum = 0;
            for (int i = 0; i < N; i++)
            {
                sum = (sum + items[i]) % M;
                if (remaindersMap.ContainsKey(sum))
                {
                    List<int> indices = remaindersMap[sum];
                    int startIndex = indices[0] + 1;
                    int endIndex = i;
                    int[] subsequence = new int[endIndex - startIndex + 1];
                    Array.Copy(items, startIndex, subsequence, 0, subsequence.Length);
                    return subsequence;
                }
                else remaindersMap.Add(sum, new List<int> { i });
            }
            foreach (int key in remaindersMap.Keys)
            {
                int remainder = (key + items.Last()) % M;
                if (remainder == 0)
                {
                    List<int> indices = remaindersMap[key];
                    if (indices[0] == items.Length - 1) { continue; }
                    int startIndex = 0;
                    int endIndex = indices[0];
                    int[] subsequence = new int[endIndex - startIndex + 2];
                    Array.Copy(items, startIndex, subsequence, 0, subsequence.Length);
                    subsequence[subsequence.Length - 1] = items.Last();
                    return subsequence;
                }
            }
            return null;
        }



    }
}
