using ContainersWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ContainersWeb.BLL
{
    public class TrackingHelper
    {
        private readonly ApplicationDbContext _context;
        public string Message { get; set; }

        public TrackingHelper()
        {
            _context = new ApplicationDbContext();
        }

        public TrackingHelper(ApplicationDbContext context)
        {
            this._context = context;
        }

        // valida que si ya hay una salida o entrada, no se puede hacer otra
        public bool ValidateInOut(TrackingViewModel containerTracking)
        {
            bool result = true;

            if (containerTracking.TrackingType == TrackingType.Contenedor ||
                containerTracking.TrackingType == TrackingType.Rastra)
            {
                var container = _context.ContainerTracking
                    .Where(w => w.ContainerNumber.Trim() == containerTracking.ContainerNumber.Trim())
                    .OrderByDescending(o => o.ContainerTrackingId)
                    .FirstOrDefault();

                if (container != null)
                {
                    if (containerTracking.Type == container.Type && container.Type == Models.Type.Entrada)
                    {
                        Message = Resources.Resources.InsideText;
                        result = false;
                    }
                    else if (containerTracking.Type == container.Type && container.Type == Models.Type.Salida)
                    {
                        Message = Resources.Resources.OutsideText;
                        result = false;
                    }
                }
                else
                {
                    if (containerTracking.Type == Models.Type.Salida)
                    {
                        Message = Resources.Resources.OutText;
                        result = false;
                    }
                }
            }
            return result;
        }

        public bool ValidateEdit(TrackingViewModel containerTracking)
        {
            bool result = true;

            switch (containerTracking.Type)
            {
                case Models.Type.Entrada:
                    if (containerTracking.ContainerStatus == ContaninerStatus.Vacio)
                    {
                        //verifico que esten llenos todos los campos antes de cuardar edicion para cambiar el status
                        if (containerTracking.ContainerLicensePlate != null &&
                            containerTracking.ContainerNumber != null &&
                            containerTracking.CompanyOriginId != null &&
                            containerTracking.CompanyDestinationId != null &&
                            containerTracking.DriverId > 0 &&
                            containerTracking.SecuritySupervisorId > 0)
                        {
                            containerTracking.DocStatus = DocStatus.Listo;
                        }
                        else
                        {
                            containerTracking.DocStatus = DocStatus.Pendiente;
                        }

                    }
                    else
                    {
                        if (containerTracking.ContainerLicensePlate != null &&
                           containerTracking.ContainerNumber != null &&
                           containerTracking.ContainerLabel != null &&
                           containerTracking.DUA != null &&
                           containerTracking.CorrelAduana != null &&
                           containerTracking.CompanyOriginId != null &&
                           containerTracking.CompanyDestinationId != null &&
                           containerTracking.DriverId > 0 &&
                           containerTracking.SecuritySupervisorId > 0)
                        {
                            containerTracking.DocStatus = DocStatus.Listo;
                        }
                        else
                        {
                            containerTracking.DocStatus = DocStatus.Pendiente;
                        }
                    }
                    break;
                case Models.Type.Salida:
                    if (containerTracking.ContainerStatus == ContaninerStatus.Vacio)
                    {
                        //verifico que esten llenos todos los campos antes de cuardar edicion para cambiar el status
                        if (containerTracking.ContainerLicensePlate != null &&
                            containerTracking.ContainerNumber != null &&
                            containerTracking.DocNumber != null &&
                            containerTracking.CompanyOriginId != null &&
                            containerTracking.CompanyDestinationId != null &&
                            containerTracking.DriverId > 0 &&
                            containerTracking.SecuritySupervisorId > 0)
                        {
                            containerTracking.DocStatus = DocStatus.Listo;
                        }
                        else
                        {
                            containerTracking.DocStatus = DocStatus.Pendiente;
                        }

                    }
                    else
                    {
                        if (containerTracking.ContainerLicensePlate != null &&
                           containerTracking.ContainerNumber != null &&
                           containerTracking.ContainerLabel != null &&
                           containerTracking.DocNumber != null &&
                           containerTracking.DUA != null &&
                           containerTracking.CorrelAduana != null &&
                           containerTracking.CompanyOriginId != null &&
                           containerTracking.CompanyDestinationId != null &&
                           containerTracking.DriverId > 0 &&
                           containerTracking.SecuritySupervisorId > 0)
                        {
                            containerTracking.DocStatus = DocStatus.Listo;
                        }
                        else
                        {
                            containerTracking.DocStatus = DocStatus.Pendiente;
                        }
                    }
                    break;
                default:
                    containerTracking.DocStatus = DocStatus.Pendiente;
                    break;
            }

            return result;
        }
    }
}