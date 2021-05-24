using MongoDB.Driver;
using MongoDbReference.Models;
using System.Collections.Generic;
using System.Linq;

namespace MongoDbReference.Services
{
    public class BookService
    {
        private readonly IMongoCollection<Book> _books; // MongoDB Collection

        public BookService(IBookstoreDatabaseSettings settings)
        {
            // uses MongoDB.Driver to perform CRUD operations against the database
            var client = new MongoClient(settings.ConnectionString); 
            var database = client.GetDatabase(settings.DatabaseName);

            //  gain access to data in a specific collection
            _books = database.GetCollection<Book>(settings.BooksCollectionName);
        }

        // Find
        public List<Book> Get() =>
            _books.Find(book => true).ToList();

        // Find by id
        public Book Get(string id) =>
            _books.Find<Book>(book => book.Id == id).FirstOrDefault();

        // Insert One
        public Book Create(Book book)
        {
            _books.InsertOne(book);
            return book;
        }

        // Replace one
        public void Update(string id, Book bookIn) =>
            _books.ReplaceOne(book => book.Id == id, bookIn);

        // Delete One by entity
        public void Remove(Book bookIn) =>
            _books.DeleteOne(book => book.Id == bookIn.Id);

        // Delete One by Id
        public void Remove(string id) =>
            _books.DeleteOne(book => book.Id == id);
    }
}
