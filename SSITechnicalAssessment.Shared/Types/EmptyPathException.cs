using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSITechnicalAssessment.Shared.Types
{
    public class EmptyPathException: Exception
    {
        public EmptyPathException(string message) : base(message) { }
    }
}
