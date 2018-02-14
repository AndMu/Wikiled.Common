using System;
using System.Runtime.Serialization;

namespace Wikiled.Common.Resources
{
    [Serializable]
    public class ResourcesException : Exception
    {
        public ResourcesException(string message)
            : base(message)
        {
        }

        public ResourcesException()
        {
        }

        public ResourcesException(string message, Exception inner)
            : base(message, inner)
        {
        }

        public ResourcesException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            // Any other custom data to be transferred
            // info.AddValue(CustomDataName, DataValue);
        }
    }
}
