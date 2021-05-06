using System;

namespace OPTEL.UI.Desktop.Services.Data
{
    public struct ActionResult
    {
        public Exception Exception { get; set; }

        public bool HasException => Exception != null;
    }

    public struct ActionResult<T>
    {
        public T Result { get; set; }

        public Exception Exception { get; set; }

        public bool HasException => Exception != null;
    }
}
