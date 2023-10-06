using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirforceAgniVirBackchodLogTracker.Model
{
    public class BookOut
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [NotNull]
        public int UserID { get; set; }

        public string PurposeOfVisit { get; set; }

        public string TimeOut { get; set; }

        public string TimeIn { get; set; }


    }
}
