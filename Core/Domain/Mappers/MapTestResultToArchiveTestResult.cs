using Core.Domain.Model;

namespace Core.Domain.Mappers
{
    public class MapTestResultToArchiveTestResult: Mapper<TestResult, ArchiveTestResult>
    {
        public override ArchiveTestResult Map(TestResult source)
        {
            //  TODO: Convert to factory class
            var archiveTest = new ArchiveTestResult();

            archiveTest.Id = source.Id;
            archiveTest.Checked = source.Checked;
            archiveTest.Status = source.Status;
            archiveTest.Url = source.Url;
            archiveTest.HttpStatus = source.HttpStatus;
            archiveTest.Response = source.Response;
            archiveTest.Test = source.Test;
            //archiveTest.ScanResult = source.ScanResult;
            archiveTest.ScanResultId = source.ScanResultId;

            return archiveTest;
        }
    }
}