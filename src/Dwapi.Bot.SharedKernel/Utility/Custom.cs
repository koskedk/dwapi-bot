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
    }
}
