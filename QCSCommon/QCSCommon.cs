using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QCSCommon
{
    public class ClientInfo
    {
        public const string AccountPrefix = "CA-";
        public Person Installer { get; set; }
        public string AccountNumber { get; set; }
        public string Region { get; set; }
        public string AgreementType { get; set; }
        public ClientInfo()
        {
            Installer = new Person();
        }

    }
    public class Person
    {
        public string FirstName {get; set;}
        public string LastName  { get; set; }
        public string Email     { get; set; }
    }
}
