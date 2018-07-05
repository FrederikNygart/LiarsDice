﻿using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserService.DataTransferObjects
{
    public class User
    {
        public ObjectId Id { get; set; }
        public string Name { get; set; }
        public int PhoneNumber { get; set; }
    }
}
