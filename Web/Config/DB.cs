using Dos.ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetWebMVC.Web.Config
{
    public class DB
    {
        public static readonly DbSession Context = new DbSession("DosConnMySql");
    }

}
