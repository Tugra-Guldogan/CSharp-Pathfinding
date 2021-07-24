using System;

namespace KLibrary.KException
{
    public class PFException : Exception
    {
        public PFException(int type) : base()
        {
            if (type == 0)
                Message = "Cannot give number lower than 1.";
            else if (type == 1)
                Message = "Number must lower than 21.";
            else if (type == 2)
                Message = "Array length must be 2";
            else if (type == 3)
                Message = "Array was null";
        }
        public override string Message { get; }
    }
}
