using ESExampleApp.Core;
using ESExampleApp.Core.Interfaces;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
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
            client = new ElasticClient(                                         //This should probably be in appsettings.json
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
               .Size(2000)
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


        public IReadOnlyCollection<Person> ESSearch(string search)
        {
            return this.ESSearch(search, 0, 2000);
        }
        public IReadOnlyCollection<Person> ESSearch(string search, int from, int size)
        {
            return client.Search<Person>(s => s
                .Query(q =>
                    q.MultiMatch(c => c
                        .Fields(f => f
                            .Field(Infer.Field<Person>(p => p.FullName.Suffix("keyword"), 5))
                            .Field(Infer.Field<Person>(p => p.FullName, 2))   //Multiply score weight by 2 to make this field "more important"
                            .Field(p => p.JobDescription.Suffix("substring")))              //Use field with ngram analyzer for substrings
                        .Query(search)
                        .Fuzziness(Fuzziness.AutoLength(5, 15))
                    )
                )
                .From(from)                                                             //Offset
                .Size(size)                                                             //# results
                .Sort( s => s
                    .Descending(SortSpecialField.Score)                                 //Sort results by score
                )
                //.Source(src => src                                                   //Limit fields in result
                //   .Excludes( x => x.Field(f => f.LongDescription))
                //)
            ).Documents;
        }

        public IReadOnlyCollection<Person> ESBigSearch(string search)
        {
            return this.ESBigSearch(search, 0, 2000);
        }

        public IReadOnlyCollection<Person> ESBigSearch(string search, int from, int size)
        {
            return client.Search<Person>(s => s
                .Query(q =>
                    q.MultiMatch(c => c
                        .Fields(f => f
                            //.Field(Infer.Field<Person>(p => p.FullName.Suffix("keyword"), 5))
                            //.Field(Infer.Field<Person>(p => p.FullName, 2))                     //Multiply score weight by 2 to make this field "more important"
                            //.Field(p => p.JobDescription.Suffix("substring"))                   //Use field with ngram analyzer for substrings
                            .Field(p => p.LongDescription)
                        )              
                        .Query(search)
                        .Fuzziness(Fuzziness.AutoLength(5, 15))
                    )
                )
                .From(from)                                                             //Offset
                .Size(size)                                                             //# results
                .Sort(s => s
                   .Descending(SortSpecialField.Score)                                 //Sort results by score
                )
            ).Documents;
        }

        public List<Person> SQLSearch(string search)
        {
            List<Person> matches = ESExampleContext.Person.Where(x => 
                                        x.FullName.Contains(search) ||
                                        x.JobDescription.Contains(search)
                                    )
                //.Select(o => new Person { FirstName = o.FirstName, LastName = o.LastName, JobDescription = o.JobDescription })
                .Take(2000).ToList();

            return matches;
        }

        public List<Person> SQLBigSearch(string search)
        {
            var searchElements = from s in search.Split(' ')
                                  select s.Trim();

            List<Person> persons = ESExampleContext.Person.ToList();
            List<Person> matches = new List<Person>();
            foreach (Person person in persons)
            {
                var LDElements = from s in person.LongDescription.Split(' ')
                                 select s.Trim();
                if(searchElements.All(i => LDElements.Contains(i)))
                {
                    matches.Add(person);
                }
            }

            return matches;
        }

        public void BulkAdd(List<Person> persons)
        {
            foreach(Person person in persons)
            {
                person.Id = Guid.NewGuid().ToString();
            }

            client.IndexMany<Person>(persons);
            ESExampleContext.Person.AddRange(persons);
            ESExampleContext.SaveChanges();
        }
    }
}
