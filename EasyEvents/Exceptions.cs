using System;

namespace EasyEvents
{
    [Serializable()]
    public class InvalidArgumentLengthException : System.Exception
    {
        public InvalidArgumentLengthException() : base() { }
        public InvalidArgumentLengthException(string message) : base(message) { }
        public InvalidArgumentLengthException(string message, System.Exception inner) : base(message, inner) { }
        
        protected InvalidArgumentLengthException(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
    
    [Serializable()]
    public class InvalidArgumentException : System.Exception
    {
        public InvalidArgumentException() : base() { }
        public InvalidArgumentException(string message) : base(message) { }
        public InvalidArgumentException(string message, System.Exception inner) : base(message, inner) { }
        
        protected InvalidArgumentException(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
    
    [Serializable()]
    public class InvalidCommandException : System.Exception
    {
        public InvalidCommandException() : base() { }
        public InvalidCommandException(string message) : base(message) { }
        public InvalidCommandException(string message, System.Exception inner) : base(message, inner) { }
        
        protected InvalidCommandException(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
    
    [Serializable()]
    public class CommandErrorException : System.Exception
    {
        public CommandErrorException() : base() { }
        public CommandErrorException(string message) : base(message) { }
        public CommandErrorException(string message, System.Exception inner) : base(message, inner) { }
        
        protected CommandErrorException(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
    
    [Serializable()]
    public class EventRunErrorException : System.Exception
    {
        public EventRunErrorException() : base() { }
        public EventRunErrorException(string message) : base(message) { }
        public EventRunErrorException(string message, System.Exception inner) : base(message, inner) { }
        
        protected EventRunErrorException(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}