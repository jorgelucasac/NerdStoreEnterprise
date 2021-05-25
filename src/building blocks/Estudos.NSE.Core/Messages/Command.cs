using System;
using FluentValidation.Results;

namespace Estudos.NSE.Core.Messages
{
    public abstract class Command : Message
    {
        public DateTime Timestamp { get; private set; }
        public ValidationResult ValidationResult { get; set; }

        protected Command()
        {
            Timestamp = DateTime.Now;
        }

        protected virtual bool EhValido()
        {
            throw new NotImplementedException();
        }
    }
}