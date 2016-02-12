﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientCare.Shared.Model.Exceptions
{
    public class IncorrectRelationshipException : Exception
    {
        public string PropertyName { get; set; }
        public string TypeName { get; set; }

        public IncorrectRelationshipException(string typeName, string propertyName, string message)
            : base(string.Format("{0}.{1}: {2}", typeName, propertyName, message))
        {
        }
    }
}