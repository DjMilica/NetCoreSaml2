﻿using System.Xml.Serialization;

namespace Saml2.Core.Models.Xml
{
    public class DigestMethod: BaseStringElement
    {
        [XmlAttribute(DataType = "anyURI", AttributeName = SamlAttributeSelector.Algorithm)]
        public string Algorithm { get; set; }
    }
}