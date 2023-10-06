using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirforceAgniVirBackchodLogTracker.Model
{
    public class User
    {
        [PrimaryKey,AutoIncrement]
        public int Id { get; set; }
        [Unique,NotNull]
        public string username { get; set; }
        [NotNull]
        public string password { get; set; }

        [NotNull]
        public string UserType { get; set; }

        [NotNull]
        public bool IsLogged { get; set; }



    }
}
