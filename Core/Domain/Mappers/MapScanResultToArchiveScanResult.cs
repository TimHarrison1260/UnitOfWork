using System;
using System.Collections.Generic;
using Core.Domain.Model;
using Core.Interfaces.Mappers;

namespace Core.Domain.Mappers
{
    public class MapScanResultToArchiveScanResult: Mapper<ScanResult, ArchiveScanResult>
    {
        private readonly IMapper<TestResult, ArchiveTestResult> _testResultMapper;

        public MapScanResultToArchiveScanResult(IMapper<TestResult, ArchiveTestResult> testResultMapper)
        {
            _testResultMapper = testResultMapper ?? throw new ArgumentNullException(nameof(testResultMapper));
        }

        public override ArchiveScanResult Map(ScanResult source)
        {
            //  TODO: Move to Factory class
            var archiveScan = new ArchiveScanResult();

            archiveScan.Id = source.Id;
            archiveScan.Scanned = source.Scanned;
            archiveScan.Url = source.Url;
            archiveScan.WebsiteId = source.WebsiteId;
            archiveScan.WebSite = source.WebSite;

            //IEnumerable<TestResult> tests = source.TestResults.ToList();
            //archiveScan.TestResults = _testResultMapper.Map(tests);

            archiveScan.TestResults = new List<ArchiveTestResult>();
            foreach (var testResult in source.TestResults)
            {
                var archiveTestResult = _testResultMapper.Map(testResult);
                archiveScan.TestResults.Add(archiveTestResult);
            }

            return archiveScan;
        }
    }
}