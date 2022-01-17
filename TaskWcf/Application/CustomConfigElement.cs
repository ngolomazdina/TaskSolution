using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace TaskWcf.Application
{
    public class CustomConfigElement : ConfigurationElement
    {
        [ConfigurationProperty("name", IsKey = true, IsRequired = true)]
        public string Name
        {
            get { return ((string)(base["name"])); }
            set { base["name"] = value; }
        }

        [ConfigurationProperty("value", IsKey = false, IsRequired = true)]
        public string Value
        {
            get { return ((string)(base["value"])); }
            set { base["value"] = value; }
        }
    }

    [ConfigurationCollection(typeof(CustomConfigElement))]
    public class CustomConfigElementCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new CustomConfigElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((CustomConfigElement)(element)).Name;
        }

        public CustomConfigElement this[int idx]
        {
            get { return (CustomConfigElement)BaseGet(idx); }
        }
    }

    public class CustomConfigElementConfigSection : ConfigurationSection
    {
        [ConfigurationProperty("CustomConfigElement")]
        public CustomConfigElementCollection CustomItems
        {
            get { return ((CustomConfigElementCollection)(base["CustomConfigElement"])); }
        }
    }

}


