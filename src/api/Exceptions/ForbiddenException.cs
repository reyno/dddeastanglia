using System;

namespace DDDEastAnglia.Api.Exceptions {
    public class ForbiddenException : Exception {
        public ForbiddenException() : base() { }
        public ForbiddenException(string message) : base(message) { }
    }
}
