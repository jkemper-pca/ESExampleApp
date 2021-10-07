using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ESExampleApp.Core
{
    public class Person
    {
        
        [Key]
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName
        {
            get
            {
                return FirstName + ' ' + LastName;
            }
        }

        public string JobDescription { get; set; }
    }
}
