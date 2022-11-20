using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstProblem
{
    public class WhiteHats
    {
        public int whiteNumber(int [] count)
        {
            if (count.Length < 2 && count.Length > 50)
                return -1;
            int allTheSame = count[0];
            int max = count.Length - 1;
            int maxcount = 0;
            bool alltheSameBool = false;
            bool all0bool = false;
            for (int i = 0; i < count.Length; i++)
            {
                if(count[i]==max)
                {
                    maxcount++;
                }
               
                if (count[i] > count.Length)
                    return -1;
                if (count[i] != allTheSame)
                {
                    alltheSameBool = true;
                }
                if (count[i] != 0)
                { 
                    all0bool = true;
                }
                if (maxcount > 1)
                    break;
            }
            if (all0bool == false)
                return 0;
            if (alltheSameBool == false)
            {
                return count.Length;
            }
            if (maxcount != 1)
            {
                return -1;
            }
            else
                return max;
           
        }
    }
}
