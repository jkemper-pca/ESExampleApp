using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
                return string.Format("{0} {1}", FirstName, LastName);
            }
            set
            {
                ;
            }
        }

        public string JobDescription { get; set; }
    }
}
