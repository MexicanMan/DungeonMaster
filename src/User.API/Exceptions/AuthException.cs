using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace User.API.Exceptions
{
    public class AuthException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AuthException"/> class.
        /// </summary>
        public AuthException()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthException"/> class with the name of the
        /// parameter that causes this exception.
        /// </summary>
        /// <param name="errorMessage">The error message that explains the reason for this exception.</param>
        public AuthException(string errorMessage) : base(errorMessage)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthException"/> class with a specified
        /// error message and the exception that is the cause of this exception.
        /// </summary>
        /// <param name="AuthException">The error message that explains the reason for this exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic)
        /// if no inner exception is specified.</param>
        public AuthException(string errorMessage, Exception innerException) : base(errorMessage, innerException)
        {

        }
    }
}
