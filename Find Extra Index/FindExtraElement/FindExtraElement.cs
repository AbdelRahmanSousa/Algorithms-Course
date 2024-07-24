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
    public static class FindExtraElement
    {
        #region YOUR CODE IS HERE
        /// <summary>
        /// Find index of extra element in first array
        /// </summary>
        /// <param name="arr1">first sorted array with an extra element</param>
        /// <param name="arr2">second sorted array</param>
        /// <returns>index of the extra element in arr1</returns>
        public static int FindIndexOfExtraElement(int[] arr1, int[] arr2)
        {
            int currentIndex = arr1.Length / 2;
            int startIndex = 0;
            int endIndex = arr1.Length;
            int step = 0;
            while (currentIndex < arr2.Length)
            {
                if (arr1[currentIndex] >= arr2[currentIndex])
                {
                    if (arr1[currentIndex] == arr2[currentIndex] || arr1[currentIndex] == arr2[currentIndex + 1])
                    {
                        startIndex = currentIndex;
                        step = (endIndex - startIndex) / 2;
                        currentIndex += step;
                        
                    }
                    else { return currentIndex; }
                }
                else
                {
                    if (arr1[currentIndex] == arr2[currentIndex - 1])
                    {
                        endIndex = currentIndex;
                        step = (endIndex - startIndex) / 2;
                        if (step == 0) {
                            return 0;
                        }
                        currentIndex -= step;
                    }
                    else
                    {
                        return currentIndex;
                    }
                }
            }
            return currentIndex;
        } 
        #endregion
    }
}
