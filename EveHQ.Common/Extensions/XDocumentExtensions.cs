using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EveHQ.Common.Extensions
{
    using System.Xml.Linq;

    public static class XDocumentExtensions
    {
        public static XAttribute TryAttribute(this XElement element, string name)
        {
            XAttribute attribute = null;
            if (element != null && !name.IsNullOrWhiteSpace())
            {
                attribute = element.Attribute(name);
            }

            if (attribute == null)
            {
                attribute = new XAttribute(name, string.Empty);
            }
            return attribute;
        }
    }
}
