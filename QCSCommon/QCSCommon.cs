using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace QCSCommon
{
    public class QcsData
    {
        static public string qcsFileName = "qcsData.xml";
    }
    
    [XmlRoot("ClientInfo")]
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
    [XmlRoot("Person")]
    public class Person
    {
        public string FirstName {get; set;}
        public string LastName  { get; set; }
        public string Email     { get; set; }
    }
    [XmlRoot("InstallattionData")]
    public class InstallattionData
    {
        public string InstallationType { get; set; }
        public string MachineBrand { get; set; }
        public string MachineModel { get; set; }
        public string MachineYear { get; set; }
        public string MachineHours { get; set; }
        public string ScannedString { get; set; }
    }
    public class InstallationDataManager
    {
        private List<InstallattionData> _data = new List<InstallattionData>();

        public void Add(InstallattionData data)
        {
            _data.Add(data);
        }
        public void AddRange(List<InstallattionData> dataArray)
        {
            _data.AddRange(dataArray);
        }
        public int Count()
        {
            return _data.Count;
        }
        public InstallattionData[] GetAll()
        {
            return _data.ToArray();
        }
    }
}
