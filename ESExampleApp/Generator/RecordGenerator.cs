using ESExampleApp.Core;
using ESExampleApp.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualBasic.FileIO;

namespace ESExampleApp.Generator
{
    public class RecordGenerator
    {
        public static void GenerateRecords(IPersonRepository personRepository)
        {
            List<Person> persons = new List<Person>();

            var path = @"records.csv";
            using (TextFieldParser csvParser = new TextFieldParser(path))
            {
                csvParser.SetDelimiters(new string[] { "," });
                csvParser.HasFieldsEnclosedInQuotes = true;

                while (!csvParser.EndOfData)
                {
                    string[] fields = csvParser.ReadFields();
                    persons.Add(new Person { FirstName = fields[0], LastName = fields[1], JobDescription = fields[2] });
                }
            }

            personRepository.BulkAdd(persons);
        }
    }
}
