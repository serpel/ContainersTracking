using ContainersWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ContainersWeb.BLL
{
    public class DriverHelper
    {
        private readonly ApplicationDbContext _context;
        public string Message { get; set; }

        public DriverHelper()
        {
            _context = new ApplicationDbContext();
        }

        public DriverHelper(ApplicationDbContext db)
        {
            _context = db;
        }

        public bool ValidateDriver(Driver driver)
        {
            bool result = false;

            var query = _context.Drivers.Where(w => w.CardId.Trim() == driver.CardId.Trim()).FirstOrDefault();

            if(query == null)
            {
                result = true;
            }else
            {
                Message = Resources.Resources.DriverCardError;
            }

            return result;
        }
    }

}