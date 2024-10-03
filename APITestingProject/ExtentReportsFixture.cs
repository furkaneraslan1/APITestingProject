using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;

namespace APITestingProject
{
    public class ExtentReportsFixture : IDisposable
    {
        public ExtentReports Extent { get; private set; }
        public ExtentSparkReporter SparkReporter { get; private set; }

        public ExtentReportsFixture()
        {
            SparkReporter = new ExtentSparkReporter("index.html");

            Extent = new ExtentReports();
            Extent.AttachReporter(SparkReporter);

        }

        public void Dispose()
        {
            Extent.Flush();
        }
    }
}
