﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace STaskManagerApi
{
    public partial class Account
    {
        public Account()
        {
            Task = new HashSet<Task>();
        }

        public int Uid { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }

        public virtual ICollection<Task> Task { get; set; }
    }
}