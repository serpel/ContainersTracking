using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using ContainersWeb.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;

namespace ContainersWeb.Controllers
{
    public class MyApiController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/Api    
        public HttpResponseMessage GetAll()
        {
            var response = this.Request.CreateResponse(HttpStatusCode.OK);
            try
            {
                //new { Name = model.Name, ImageUrl = model.PictureFile, ModelUrl = model.ModelFile }
                var containers = db.ContainerTracking
                   .ToList()
                   .Select(s => new
                   {
                       s.ContainerTrackingId,
                       Type = s.Type == 0 ? Resources.Resources.Out : Resources.Resources.In,
                       DocStatus = s.DocStatus == 0 ? Resources.Resources.Pending : Resources.Resources.Ready,
                       ContainerStatus = s.ContainerStatus == 0 ? Resources.Resources.Empty : Resources.Resources.Full,
                       Date = s.InsertedAt.ToString("yyyy-MM-dd hh:mm"),
                       s.ContainerNumber,
                       s.ContainerLicensePlate
                   });

                String json = JsonConvert.SerializeObject(containers, Formatting.Indented);

                JArray jarray = new JArray();
                jarray.Add(JToken.Parse(json).ToList());

                int count = containers.Count();
                JObject obj = new JObject();
                obj["draw"] = 10;
                obj["recordsTotal"] = count;
                obj["recordsFiltered"] = count;
                obj["data"] = jarray;

                response.Content = new StringContent(obj.ToString(), Encoding.UTF8, "text/plain");
            }
            catch (Exception e)
            {
                response = this.Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
            return response;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}