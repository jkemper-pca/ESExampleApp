using System;
using System.Collections.Generic;
using System.Text;

namespace ESExampleApp.Core.Interfaces
{
    public interface IPersonRepository
    {
        string Add(Person p);

        void Remove(Person p);

        void Edit(Person p);

        IReadOnlyCollection<Person> Get();

        Person Get(string id);

        IReadOnlyCollection<Person> ESSearch(string search);
        IReadOnlyCollection<Person> ESSearch(string search, int from, int size);

        IReadOnlyCollection<Person> ESBigSearch(string search);
        IReadOnlyCollection<Person> ESBigSearch(string search, int from, int size);

        List<Person> SQLSearch(string search);
        List<Person> SQLBigSearch(string search);

        void BulkAdd(List<Person> persons);
    }
}
