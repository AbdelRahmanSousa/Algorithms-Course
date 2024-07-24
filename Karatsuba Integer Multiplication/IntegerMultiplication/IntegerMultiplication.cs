using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
namespace Problem
{
    public static class IntegerMultiplication
    {
        public static byte[] IntegerMultiply(byte[] X, byte[] Y, int N)
        {
            if (N <= 128)
            {
                byte[] res = new byte[2 * N];
                int i = 0;
                do
                {
                    int j = 0;
                    do
                    {
                        int product = X[i] * Y[j];
                        int digit = product % 10;
                        int carry = product / 10;
                        res[i + j] += (byte)digit;
                        res[i + j + 1] += (byte)carry;
                        int k = i + j;
                        if (res[i + j] >= 10)
                        {
                            res[i + j + 1] += (byte)(res[i + j] / 10);
                            res[i + j] %= 10;
                        }
                        j++;
                    } while (j < N);
                    i++;
                } while (i < N);
                return res;
            }
            byte[] A = X.Take(N / 2).ToArray();
            byte[] B = X.Skip(N / 2).ToArray();
            byte[] C = Y.Take(N / 2).ToArray();
            byte[] D = Y.Skip(N / 2).ToArray();
            byte[] AxC = null;
            byte[] BxD = null;
            byte[] ABxCD = null;
            Parallel.Invoke
                 (
                  () => { AxC = ParallelIntegerMultiply(A, C, N / 2); },
                  () => { BxD = ParallelIntegerMultiply(B, D, N / 2); },
                  () => { ABxCD = ParallelIntegerMultiply(Add(A, B), Add(C, D), N - N / 2); }
                 );
            byte[] ADBC = Subtract(Subtract(ABxCD, AxC), BxD);
            return Add(Add(AxC, Append(BxD, N)), Append(ADBC, N / 2));
        }
        public static byte[] ParallelIntegerMultiply(byte[] X, byte[] Y, int N)
        {
            if (N <= 128)
            {
                byte[] res = new byte[2 * N];
                int i = 0;
                do
                {
                    int j = 0;
                    do
                    {
                        int product = X[i] * Y[j];
                        int digit = product % 10;
                        int carry = product / 10;
                        res[i + j] += (byte)digit;
                        res[i + j + 1] += (byte)carry;
                        int k = i + j;
                        if (res[i + j] >= 10)
                        {
                            res[i + j + 1] += (byte)(res[i + j] / 10);
                            res[i + j] %= 10;
                        }
                        j++;
                    } while (j < N);
                    i++;
                } while (i < N);
                return res;
            }
            byte[] A = X.Take(N / 2).ToArray();
            byte[] B = X.Skip(N / 2).ToArray();
            byte[] C = Y.Take(N / 2).ToArray();
            byte[] D = Y.Skip(N / 2).ToArray();
            byte[] AxC = IntegerMultiply(A, C, N / 2);
            byte[] BxD = IntegerMultiply(B, D, N / 2);
            byte[] ABxCD = IntegerMultiply(Add(A, B), Add(C, D), N - N / 2);
            byte[] ADBC = Subtract(Subtract(ABxCD, AxC), BxD);
            return Add(Add(AxC, Append(BxD, N)), Append(ADBC, N / 2));
        }
        public static byte[] Add(byte[] a, byte[] b)
        {
            if (a.Length > b.Length)
            {
                b = Prepend(b, a.Length - b.Length);
            }
            int i = 0;
            int carry = 0;
            int n = a.Length;
            byte[] result = new byte[n];
            do
            {
                int sum = a[i] + b[i] + carry;
                result[i] = (byte)(sum % 10);
                carry = sum / 10;
                i++;
            } while (i < n);
            if (carry > 0)
            {
                byte[] temp = result;
                result = new byte[n + 1];
                Array.Copy(temp, result, n);
                result[n] = (byte)carry;
            }
            return result;
        }
        public static byte[] Subtract(byte[] a, byte[] b)
        {
            int borrow = 0;
            int i = 0;
            byte[] result = new byte[a.Length];
            do
            {
                int diff = a[i] - (b.Length > i ? b[i] : 0) - borrow;
                if (diff < 0)
                {
                    diff += 10;
                    borrow = 1;
                }
                else
                {
                    borrow = 0;
                }
                result[i] = (byte)diff;
                i++;
            } while (i < a.Length);
            return result;
        }
        public static byte[] Prepend(byte[] X, int N)
        {
            if (N <= 0) return X;
            byte[] arr1 = new byte[N];
            return X.Concat(arr1).ToArray();
        }
        public static byte[] Append(byte[] X, int N)
        {
            byte[] arr1 = new byte[N];
            return arr1.Concat(X).ToArray();
        }
    }
}

/*using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace Problem
{

    public class foldResult
    {
        public byte[] res;
        public byte carry;
        public foldResult(ref byte[] res, byte carry)
        {
            this.carry = carry;
            this.res = res;
        }
    }
    // *****************************************
    // DON'T CHANGE CLASS OR FUNCTION NAME
    // YOU CAN ADD FUNCTIONS IF YOU NEED TO
    // *****************************************
    public static class IntegerMultiplication
    {
        #region YOUR CODE IS HERE
        //Your Code is Here:
        //==================
        /// <summary>
        /// Multiply 2 large integers of N digits in an efficient way [Karatsuba's Method]
        /// </summary>
        /// <param name="X">First large integer of N digits [0: least significant digit, N-1: most signif. dig.]</param>
        /// <param name="Y">Second large integer of N digits [0: least significant digit, N-1: most signif. dig.]</param>
        /// <param name="N">Number of digits (power of 2)</param>
        /// <returns>Resulting large integer of 2xN digits (left padded with 0's if necessarily) [0: least signif., 2xN-1: most signif.]</returns>
        private static byte[] naiveMultiply(ref byte[] X, ref byte[] Y, int startX, int startY, int N)
        {
            byte[] res = new byte[2 * N];
            int carry = 0;
            int i = 0;
            for (; i < N; i++)
            {
                int sum = carry;
                for (int j = i; j >= 0; j--)
                {
                    sum += X[j + startX] * Y[i - j + startY];
                }
                res[i] = (byte)(sum % 10);
                carry = sum / 10;
            }
            for (; i < res.Length; i++)
            {
                int sum = carry;
                int limit = i - N + 1;
                for (int j = N - 1; j >= limit; j--)
                {
                    sum += X[j + startX] * Y[N - j - 1 + startY + limit];
                }
                res[i] = (byte)(sum % 10);
                carry = sum / 10;
            }
            return res;
        }
        private static foldResult fold(byte[] number, int startIndex, int length)
        {
            int halfLength = length / 2;
            byte[] result = new byte[halfLength];
            int i = 0;
            int carry = 0;
            for (int u = startIndex; i < halfLength; i++, u++)
            {
                int sum = number[u] + number[u + halfLength] + carry;
                result[i] = (byte)(sum % 10);
                carry = sum / 10;
            }
            return new foldResult(ref result, (byte)carry);
        }
        private static byte[] AddPad(byte[] a, int aPad, byte[] b, int bPad)
        {
            byte[] longest;
            byte[] shortest;
            if (a.Length + aPad > b.Length + bPad)
            {
                longest = a;
                shortest = b;
            }
            else
            {
                longest = b;
                shortest = a;
                int temp = bPad;
                bPad = aPad;
                aPad = temp;
            }
            byte[] result = new byte[longest.Length + aPad];
            int resIndex = 0;
            int bIndex = 0;
            int aIndex = 0;
            if (aPad > bPad)
            {
                resIndex = bPad;
                int diff = aPad - bPad;
                for (; bIndex < diff; bIndex++, resIndex++)
                {
                    result[resIndex] = shortest[bIndex];
                }
            }
            else
            {
                resIndex = aPad;
                int diff = bPad - aPad;
                for (; aIndex < diff; aIndex++, resIndex++)
                {
                    result[resIndex] = longest[aIndex];
                }
                if (longest.Length - aIndex < shortest.Length - bIndex)
                {
                    byte[] temp = longest;
                    longest = shortest;
                    shortest = temp;
                    int tempIndex = aIndex;
                    aIndex = bIndex;
                    bIndex = tempIndex;
                }
            }
            int carry = 0;
            for (; bIndex < shortest.Length; bIndex++, aIndex++, resIndex++)
            {
                int sum = longest[aIndex] + shortest[bIndex] + carry;
                result[resIndex] = (byte)(sum % 10);
                carry = sum / 10;
            }
            for (; aIndex < longest.Length; aIndex++, resIndex++)
            {
                int sum = longest[aIndex] + carry;
                result[resIndex] = (byte)(sum % 10);
                carry = sum / 10;
            }
            if (carry == 1)
            {
                Array.Resize(ref result, result.Length + 1);
                result[resIndex] = (byte)carry;
            }
            return result;
        }
        private static byte[] Subtract(byte[] a, byte[] b)
        {
            int borrow = 0;
            int resIndex = 0;
            byte[] result = new byte[a.Length];
            for (; resIndex < b.Length; resIndex++)
            {
                int diff = a[resIndex] - b[resIndex] - borrow;
                if (diff < 0)
                {
                    diff += 10;
                    borrow = 1;
                }
                else
                {
                    borrow = 0;
                }
                result[resIndex] = (byte)diff;
            }
            for (; resIndex < a.Length; resIndex++)
            {
                int diff = a[resIndex] - borrow;
                if (diff < 0)
                {
                    diff += 10;
                    borrow = 1;
                }
                else
                {
                    borrow = 0;
                }
                result[resIndex] = (byte)diff;
            }

            return result;
        }
        private static byte[] KaratsubaMultiply(byte[] X, byte[] Y, int startX, int startY, int N)
        {
            if (N <= 256)
            {
                return naiveMultiply(ref X, ref Y, startX, startY, N);
            }
            int halfN = N / 2;
            int zLength = halfN;
            byte[] M1 = null;
            byte[] M2 = null;
            byte[] Z = null;
            Parallel.Invoke(new ParallelOptions() { MaxDegreeOfParallelism = 2 },
                new Action(() => { M1 = KaratsubaMultiply(X, Y, startX, startY, halfN); }),
                new Action(() => { M2 = KaratsubaMultiply(X, Y, startX + halfN, startY + halfN, halfN); }),
                new Action(() =>
                {
                    foldResult resX = fold(X, startX, N);
                    foldResult resY = fold(Y, startY, N);
                    Z = KaratsubaMultiply(resX.res, resY.res, 0, 0, halfN);
                    if (resX.carry > 0 && resY.carry > 0)
                    {
                        Z = AddPad(AddPad(Z, 0, new byte[] { 1 }, N), 0, AddPad(resX.res, halfN, resY.res, halfN), 0);
                    }
                    else if (resX.carry > 0)
                    {
                        Z = AddPad(Z, 0, resY.res, halfN);

                    }
                    else if (resY.carry > 0)
                    {
                        Z = AddPad(Z, 0, resX.res, halfN);
                    }
                }));
            byte[] subtractRes = Subtract(Z, M1);
            subtractRes = Subtract(subtractRes, M2);
            return AddPad(AddPad(M2, N, M1, 0), 0, subtractRes, halfN);
        }
        static public byte[] IntegerMultiply(byte[] X, byte[] Y, int N)
        {
            byte[] res = KaratsubaMultiply(X, Y, 0, 0, N);
            if (res.Length != 2 * N) Array.Resize(ref res, 2 * N);
            return res;
        }

        #endregion
    }
}
*/