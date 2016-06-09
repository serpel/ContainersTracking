using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ContainersWeb.Models;

namespace ContainersWeb.BLL
{
    public class ContainerTrackingHelper
    {
        private readonly ApplicationDbContext _context;
        public string Message { get; set; }

        public ContainerTrackingHelper()
        {
            _context = new ApplicationDbContext();
        }

        public ContainerTrackingHelper(ApplicationDbContext context)
        {
            this._context = context;
        }

        // valida que si ya hay una salida o entrada, no se puede hacer otra
        public bool ValidateOnCreate(ContainerTracking containerTracking)
        {
            bool result = true;

            var container = _context.ContainerTracking
                      .Where(w => w.ContainerNumber.Trim() == containerTracking.ContainerNumber.Trim())
                      .OrderByDescending(o => o.InsertedAt)
                      .FirstOrDefault();

            if (container != null)
            {
                if (containerTracking.Type == container.Type && container.Type == Models.Type.In)
                {
                    Message = Resources.Resources.InsideText;
                    result = false;
                }
                else if (containerTracking.Type == container.Type && container.Type == Models.Type.Out)
                {
                    Message = Resources.Resources.OutsideText;
                    result = false;
                }
            }
            else
            {
                if(containerTracking.Type == Models.Type.Out)
                {
                    Message = Resources.Resources.OutText;
                    result = false;
                }
            }

            return result;
        }

        public bool ValidateOnEdit(ContainerTracking containerTracking)
        {
            bool result = true;

            switch (containerTracking.Type)
            {
                case Models.Type.In:
                case Models.Type.Out:
                    if(containerTracking.ContainerStatus == ContaninerStatus.Empty)
                    {
                        //verifico que esten llenos todos los campos antes de cuardar edicion para cambiar el status
                        if(containerTracking.ContainerLicensePlate != null &&
                            containerTracking.ContainerNumber != null &&
                            containerTracking.CompanyOriginId > 0 &&
                            containerTracking.CompanyDestinationId > 0 &&
                            containerTracking.DriverId > 0 && 
                            containerTracking.SecuritySupervisorId > 0)
                        {
                            containerTracking.DocStatus = DocStatus.Ready;
                        }else
                        {
                            containerTracking.DocStatus = DocStatus.Pending;
                        }

                    }
                    else
                    {
                        if (containerTracking.ContainerLicensePlate != null &&
                           containerTracking.ContainerNumber != null &&
                           containerTracking.ContainerLabel != null &&
                           containerTracking.DocNumber != null &&
                           containerTracking.CorrelAduana != null &&
                           containerTracking.CompanyOriginId > 0 &&
                           containerTracking.CompanyDestinationId > 0 &&
                           containerTracking.DriverId > 0 &&
                           containerTracking.SecuritySupervisorId > 0)
                        {
                            containerTracking.DocStatus = DocStatus.Ready;
                        }
                        else
                        {
                            containerTracking.DocStatus = DocStatus.Pending;
                        }
                    }
                    break;
                default:
                    break;
            }

            return result;
        }
    }
}