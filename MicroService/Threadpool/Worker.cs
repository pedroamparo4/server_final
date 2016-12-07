using System;
using System.Threading;

namespace MicroService.Threadpool
{
    public class Worker
    {
        #region Properties
        public AnalyzerJob AnalyzerJob { get; set; }
        public JobStatus Status { get; set; }
        public string ErrorMessage { get; set; }
        public DateTime Start { get; set; }
        public DateTime Finish { get; set; }
        public Thread Thread { get; set; }        
        #endregion
    }
}
