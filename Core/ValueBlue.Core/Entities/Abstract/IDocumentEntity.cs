using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ValueBlue.Core.Entities.Abstract
{
    public interface IDocumentEntity
    {
        ObjectId? Id { get; set; }
    }
}
