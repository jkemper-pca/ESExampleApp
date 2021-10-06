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

        Person Get(int id);

        IReadOnlyCollection<Person> Search(string search);
    }
}
