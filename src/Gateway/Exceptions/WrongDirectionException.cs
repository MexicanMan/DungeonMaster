﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gateway.Exceptions
{
    public class WrongDirectionException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WrongDirectionException"/> class.
        /// </summary>
        public WrongDirectionException()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WrongDirectionException"/> class with the name of the
        /// parameter that causes this exception.
        /// </summary>
        /// <param name="errorMessage">The error message that explains the reason for this exception.</param>
        public WrongDirectionException(string errorMessage) : base(errorMessage)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WrongDirectionException"/> class with a specified
        /// error message and the exception that is the cause of this exception.
        /// </summary>
        /// <param name="NoSuchMonsterException">The error message that explains the reason for this exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic)
        /// if no inner exception is specified.</param>
        public WrongDirectionException(string errorMessage, Exception innerException) : base(errorMessage, innerException)
        {

        }
    }
}
