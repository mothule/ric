using System;

namespace RIC.Models
{
    public class AppException : Exception
    {
        public int Error { get; set; }
        public string ErrorDescription { get; set; }

        public AppException(int error, string errorDescription)
            : base(errorDescription)
        {
            Error = error;
            ErrorDescription = errorDescription;
        }
        public AppException(int error)
            : base()
        {
            Error = error;
        }
        public AppException(int error, string errorDescription, Exception innerException)
            : base(errorDescription, innerException)
        {
            Error = error;
            ErrorDescription = errorDescription;
        }
    }
}
