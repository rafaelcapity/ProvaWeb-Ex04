using ProvaWeb_Ex04.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ProvaWeb_Ex04.Context
{
    public class DataContext : DbContext
    {
        public DbSet<Encriptar> Encriptars { get; set; }
    }
}