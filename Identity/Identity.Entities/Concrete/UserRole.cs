﻿using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Entities.Concrete
{
    public class UserRole
    {
        public ObjectId? Id { get; set; }

        public string? RoleName { get; set; }
    }
}
