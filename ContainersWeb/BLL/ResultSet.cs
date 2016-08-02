using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls; //For SortBy method
using System.Web.Mvc;
using ContainersWeb.Models;


namespace ContainersWeb.BLL
{
    public class ResultSet
    {
        public List<ContainerTrackingSearchViewModel> GetResult(string search, string sortOrder, int start, int length, List<ContainerTrackingSearchViewModel> dtResult, List<string> columnFilters)
        {
            return FilterResult(search, dtResult,columnFilters).SortBy(sortOrder).Skip(start).Take(length).ToList();
        }

        public int Count(string search, List<ContainerTrackingSearchViewModel> dtResult, List<string> columnFilters)
        {
            return FilterResult(search, dtResult, columnFilters).Count();
        }

        private IQueryable<ContainerTrackingSearchViewModel> FilterResult(string search, List<ContainerTrackingSearchViewModel> dtResult, List<string> columnFilters)
        {
            IQueryable<ContainerTrackingSearchViewModel> results = dtResult.AsQueryable();

            results = results.Where(p => (search == null || (p.ContainerNumber.ToLower().Contains(search.ToLower()) || p.ContainerLicensePlate != null && p.ContainerLicensePlate.ToLower().Contains(search.ToLower())
                || p.InsertedAt != null && p.InsertedAt.ToLower().Contains(search.ToLower())
                || p.ContainerTrackingId.ToString().Contains(search.ToLower())
                || p.ContainerStatus.ToLower().Contains(search.ToLower())
                || p.Type.ToLower().Contains(search.ToLower())
                || p.TrackingType.ToLower().Contains(search.ToLower())
                || p.DocStatus.ToLower().Contains(search.ToLower())))
                && (columnFilters[2] == null || (p.ContainerNumber != null && p.ContainerNumber.ToLower().Contains(columnFilters[2].ToLower())))
                && (columnFilters[3] == null || (p.ContainerLicensePlate != null && p.ContainerLicensePlate.ToLower().Contains(columnFilters[3].ToLower())))
                && (columnFilters[7] == null || (p.UpdatedAt != null && p.InsertedAt.ToLower().Contains(columnFilters[7].ToLower()))));

            return results;
        }
    }
}