﻿using System.Xml.Serialization;

namespace Saml2.Core.Models.Xml
{
    public class SignatureMethod: BaseStringElement
    {
        [XmlAttribute(DataType = "anyURI", AttributeName = "Algorithm")]
        public string Algorithm { get; set; }
    }
}