using System;
using System.Collections.Generic;
using System.Text;

namespace Tiger.WebApi.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Field)]
    public class ResultCodeMessageAttribute : Attribute
    {
        public string Message { get; private set; } = "";
        public ResultCodeMessageAttribute(string message)
        {
            this.Message = message;
        }
    }
}
