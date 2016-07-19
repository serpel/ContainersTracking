using ContainersWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ContainersWeb.BLL
{
    public class TrailerTrackingHelper
    {
        private readonly ApplicationDbContext _context;
        public string Message { get; set; }

        public TrailerTrackingHelper()
        {
            _context = new ApplicationDbContext();
        }

        public TrailerTrackingHelper(ApplicationDbContext context)
        {
            this._context = context;
        }

        // valida que si ya hay una salida o entrada, no se puede hacer otra
        public bool ValidateOnCreate(TrailerTracking trailerTracking)
        {
            bool result = true;

            var container = _context.ContainerTracking
                      .Where(w => w.ContainerNumber.Trim() == trailerTracking.TrailerNumber.Trim())
                      .OrderByDescending(o => o.InsertedAt)
                      .FirstOrDefault();

            if (container != null)
            {
                if (trailerTracking.Type == container.Type && container.Type == Models.Type.Entrada)
                {
                    Message = Resources.Resources.InsideText;
                    result = false;
                }
                else if (trailerTracking.Type == container.Type && container.Type == Models.Type.Salida)
                {
                    Message = Resources.Resources.OutsideText;
                    result = false;
                }
            }
            else
            {
                if (trailerTracking.Type == Models.Type.Salida)
                {
                    Message = Resources.Resources.OutText;
                    result = false;
                }
            }

            return result;
        }

        public bool ValidateOnEdit(TrailerTracking trailerTracking)
        {
            bool result = true;

            switch (trailerTracking.Type)
            {
                case Models.Type.Entrada:
                case Models.Type.Salida:
                    if (trailerTracking.ContainerStatus == ContaninerStatus.Vacio)
                    {
                        //verifico que esten llenos todos los campos antes de cuardar edicion para cambiar el status
                        if (trailerTracking.ContainerLicensePlate != null &&
                            trailerTracking.TrailerNumber != null &&
                            trailerTracking.CompanyOriginId > 0 &&
                            trailerTracking.CompanyDestinationId > 0 &&
                            trailerTracking.DriverId > 0 &&
                            trailerTracking.SecuritySupervisorId > 0)
                        {
                            trailerTracking.DocStatus = DocStatus.Listo;
                        }
                        else
                        {
                            trailerTracking.DocStatus = DocStatus.Pendiente;
                        }

                    }
                    else
                    {
                        if (trailerTracking.ContainerLicensePlate != null &&
                           trailerTracking.TrailerNumber != null &&
                           trailerTracking.ContainerLabel != null &&
                           trailerTracking.DocNumber != null &&
                           trailerTracking.CompanyOriginId > 0 &&
                           trailerTracking.CompanyDestinationId > 0 &&
                           trailerTracking.DriverId > 0 &&
                           trailerTracking.SecuritySupervisorId > 0)
                        {
                            trailerTracking.DocStatus = DocStatus.Listo;
                        }
                        else
                        {
                            trailerTracking.DocStatus = DocStatus.Pendiente;
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