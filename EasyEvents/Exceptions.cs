using System;

namespace EasyEvents
{
    [Serializable]
    public class InvalidArgumentLengthException : Exception
    {
        public InvalidArgumentLengthException() { }
        public InvalidArgumentLengthException(string message) : base(message) { }
        public InvalidArgumentLengthException(string message, Exception inner) : base(message, inner) { }
        
        protected InvalidArgumentLengthException(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
    
    [Serializable]
    public class InvalidArgumentException : Exception
    {
        public InvalidArgumentException() { }
        public InvalidArgumentException(string message) : base(message) { }
        public InvalidArgumentException(string message, Exception inner) : base(message, inner) { }
        
        protected InvalidArgumentException(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
    
    [Serializable]
    public class InvalidCommandException : Exception
    {
        public InvalidCommandException() { }
        public InvalidCommandException(string message) : base(message) { }
        public InvalidCommandException(string message, Exception inner) : base(message, inner) { }
        
        protected InvalidCommandException(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
    
    [Serializable]
    public class CommandErrorException : Exception
    {
        public CommandErrorException() { }
        public CommandErrorException(string message) : base(message) { }
        public CommandErrorException(string message, Exception inner) : base(message, inner) { }
        
        protected CommandErrorException(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
    
    [Serializable]
    public class EventRunErrorException : Exception
    {
        public EventRunErrorException() { }
        public EventRunErrorException(string message) : base(message) { }
        public EventRunErrorException(string message, Exception inner) : base(message, inner) { }
        
        protected EventRunErrorException(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
    
    [Serializable]
    public class EventNotFoundException : Exception
    {
        public EventNotFoundException() { }
        public EventNotFoundException(string message) : base(message) { }
        public EventNotFoundException(string message, Exception inner) : base(message, inner) { }
        
        protected EventNotFoundException(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}