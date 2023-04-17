using AutoMapper;
using Google.Cloud.Firestore;

namespace UCare.Infrastructure.Firebase
{
    public abstract class RepositoryBase
    {
        protected readonly FirestoreDb firestoreDb;
        protected readonly Mapper mapper;
        protected readonly string collection;
        protected readonly string document;
        protected readonly string SubCollection;
        protected readonly string SubDocument;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="firestoreDb"></param>
        /// <param name="mapper"></param>
        /// <param name="collection"></param>
        protected RepositoryBase(FirestoreDb firestoreDb, Mapper mapper, string collection)
        {
            this.firestoreDb = firestoreDb;
            this.mapper = mapper;
            this.collection = collection;
            this.document = string.Empty;
            this.SubCollection = string.Empty;
            this.SubDocument = string.Empty;
        }

        protected RepositoryBase(FirestoreDb firestoreDb, Mapper mapper, string collection, string document, string SubCollection, string SubDocument)
        {
            this.firestoreDb = firestoreDb;
            this.mapper = mapper;
            this.collection = collection;
            this.document = document;
            this.SubCollection = SubCollection;
            this.SubDocument = SubDocument;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public CollectionReference GetCollection()
        {
            if (
                string.IsNullOrEmpty(collection) &&
                string.IsNullOrWhiteSpace(document) &&
                string.IsNullOrWhiteSpace(SubCollection) &&
                string.IsNullOrWhiteSpace(SubDocument))
                return firestoreDb.Collection(collection).Document(document).Collection(SubCollection);
            else
                return firestoreDb.Collection(collection);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DocumentReference GetId(string id)
        {
            return GetCollection().Document(id);
        }

        public Dictionary<string, object> GetToUpdate(dynamic obj)
        {
            var result = new Dictionary<string, object>();

            if (obj != null)
            {
                Type type = obj.GetType();

                foreach (var prop in type.GetProperties())
                {
                    if (prop.Name != "Id")
                        result[prop.Name] = prop.GetValue(obj, null);
                }
            }
            return result;
        }
    }
}
