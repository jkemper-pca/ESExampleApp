using ESExampleApp.Core;
using ESExampleApp.Core.Interfaces;
using Nest;
using System;
using System.Collections.Generic;
using System.Text;

namespace ESExampleApp.Infrastructure
{
    public class PersonRepository : IPersonRepository
    {
        private ElasticClient client;
        private ESExampleContext ESExampleContext;


        public PersonRepository(ESExampleContext context)
        {
            ESExampleContext = context;
            client = new ElasticClient(
                new ConnectionSettings(
                    new Uri("http://localhost:9200"))
                .DefaultIndex("person")
                );
        }

        public string Add(Person p)
        {
            p.Id = Guid.NewGuid().ToString();

            ESExampleContext.Add(p);
            ESExampleContext.SaveChanges();

            var response = client.IndexDocumentAsync(p).Result;
            return response.Id;
        }

        public void Edit(Person p)
        {
            ESExampleContext.Update(p);
            ESExampleContext.SaveChanges();

            client.Update<Person>(p.Id, u => u
                                .Doc(p));
        }

        public IReadOnlyCollection<Person> Get()
        {
            return client.Search<Person>(s => s
               .From(0)
               .Size(25)
            ).Documents;
        }

        public Person Get(string id)
        {
            //var response = client.Get<Person>(id);
            //return response.Source;

            return ESExampleContext.Person.Find(id);
        }

        public void Remove(Person p)
        {
            throw new NotImplementedException();
        }

        public IReadOnlyCollection<Person> Search(string search)
        {
            return client.Search<Person>(s => s
                .Query(q =>
                    q.MultiMatch(c => c
                        .Fields(f => f.Field(p => p.FullName).Field(p => p.JobDescription))
                        .Query(search)
                    )
                )
            ).Documents;
        }
    }
}
