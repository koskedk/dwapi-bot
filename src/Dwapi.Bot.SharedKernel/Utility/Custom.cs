using System;

namespace Dwapi.Bot.SharedKernel.Utility
{
    public static class Custom
    {
        public static int PageCount(int batchSize, long totalRecords)
        {
            if (totalRecords > 0)
            {
                if (totalRecords < batchSize)
                {
                    return 1;
                }
                return (int) Math.Ceiling(totalRecords / (double) batchSize);
            }
            return 0;
        }

        public static int GetPerc(int count, int total)
        {
            var perc=  ((float)count / (float)total) * 100;
            return  (int) perc;
        }
        public static int GetPerc(int initial, int count, int total)
        {
            int ratio = 99 - initial;
            var perc=  ((float)count / (float)total) * ratio;
            return initial + (int) perc;
        }
    }
}
