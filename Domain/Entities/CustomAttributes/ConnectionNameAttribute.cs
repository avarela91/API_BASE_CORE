using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.CustomAttributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class ConnectionNameAttribute:Attribute
    {
        public string ConnectionName { get; }

        public ConnectionNameAttribute(string connectionName)
        {
            ConnectionName = connectionName;
        }
    }
}
