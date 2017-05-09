using System;

namespace DerbyHacks.Model
{
    public class RecognitionCandidate
    {
        public string Subject { get; set; }
        public double Confidence { get; set; }
        public DateTime DateTime { get; set; }
    }
}