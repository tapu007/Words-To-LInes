using System.Collections.Generic;

namespace WordToLineApp.ResponseModels
{
    public class LinesResponse
    {
        public LinesResponse()
        {
            Lines = new();
        }
        public List<string> Lines { get; set; }
        public int StatusCode { get; set; }
    }
}
