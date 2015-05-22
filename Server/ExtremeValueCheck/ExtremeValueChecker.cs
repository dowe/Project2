using Common.DataTransferObjects;

namespace Server.ExtremeValueCheck
{
    public class ExtremeValueChecker : IExtremeValueChecker
    {

        public bool isExtreme(Test test)
        {
            return (test.ResultValue <= test.Analysis.ExtremeMinValue || test.ResultValue >= test.Analysis.ExtremeMaxValue);
        }
    }
}
