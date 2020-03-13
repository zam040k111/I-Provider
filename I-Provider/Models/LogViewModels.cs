using I_Provider.BLL.Logger.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace I_Provider.Models
{
    public class LogViewModels
    {
        public IEnumerable<Log> Logs { get; set; }
        public PageInfo PageInfo { get; set; }
    }
    public class PageInfo
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalItems { get; set; }
        public int TotalPages
        {
            get { return (int)Math.Ceiling((decimal)TotalItems / PageSize); }
        }
    }
}
