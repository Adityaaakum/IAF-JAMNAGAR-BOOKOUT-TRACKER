using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirforceAgniVirBackchodLogTracker.Model
{
    public class Cadet
    {

        [PrimaryKey,AutoIncrement]
        public int Id { get; set; }
        [NotNull]
        public string Name { get; set; }
        
        [Unique]
        public string ServiceNo { get; set; }

        public string Trade { get; set; }

        public string Unit { get; set; }

        public string Billet { get; set; }

        
        public string VechileNumber { get; set; }

        public string MobileNumber { get; set; }
        public string MentorName { get; set; }

        public string MentorMobileNumber { get; set; }

        
        public int TotalLateEntries { get; set; }

        public int isBookedOut { get; set; }


    }
}
