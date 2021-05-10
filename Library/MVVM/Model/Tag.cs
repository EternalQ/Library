﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.MVVM.Model
{
    public class Tag
    {
        public int TagId { get; set; }

        public string Name { get; set; }

        public List<Book> Books { get; set; }
    }
}
