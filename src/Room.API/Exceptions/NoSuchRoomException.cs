﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Room.API.Exceptions
{
    public class NoSuchRoomException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NoSuchRoomException"/> class.
        /// </summary>
        public NoSuchRoomException()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NoSuchRoomException"/> class with the name of the
        /// parameter that causes this exception.
        /// </summary>
        /// <param name="errorMessage">The error message that explains the reason for this exception.</param>
        public NoSuchRoomException(string errorMessage) : base(errorMessage)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NoSuchRoomException"/> class with a specified
        /// error message and the exception that is the cause of this exception.
        /// </summary>
        /// <param name="NoSuchMonsterException">The error message that explains the reason for this exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic)
        /// if no inner exception is specified.</param>
        public NoSuchRoomException(string errorMessage, Exception innerException) : base(errorMessage, innerException)
        {

        }
    }
}
