using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ValueBlue.Core.Entities.Abstract;

namespace Identity.Entities.Concrete
{
    public class UserDocument : IDocumentEntity
    {
        public ObjectId? Id { get; set; }

        public string? Username { get; set; }

        public string? Password { get; set; }

        public UserRole Role { get; set; }
    }
}
