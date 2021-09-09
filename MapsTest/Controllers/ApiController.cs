using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Asa.DataStore;

namespace Asa.MapApi.Controllers
{
    public class ApiController : Controller
    {
        // GET: Api
        public ActionResult Index()
        {
            ViewBag.Listado_Categorias = _ListCategory();
            return View();
        }

        public ActionResult Categories(string id, Dictionary<string, object> entity)
        {
            // sheetName: Categorias
            switch (Request.HttpMethod)
            {
                case "GET":
                   
                    return Json(_ListCategory(), JsonRequestBehavior.AllowGet);
                    
            }

            Response.StatusCode = 400;
            return Json(new { error = "Method not suported." }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult ValidateForm(string id, Dictionary<string, object> entity)
        {
            
            if (Request.HttpMethod == "POST")
            {
                // entity <-- inbound
                return Json(new { entity = entity, isvalid = false, errors = new string[] {"Message"} }, JsonRequestBehavior.AllowGet);
            }

            Response.StatusCode = 400;
            return Json(new { error = "Method not suported." }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult POIs(string id, Dictionary<string, object> entity)
        {
            
            switch (Request.HttpMethod)
            {
                case "GET":
                    var _POIs = new List<Dictionary<string, object>>();
                    this._ListPOIS();

                    
                     //return Json(new { pois = _POIs.ToArray() }, JsonRequestBehavior.AllowGet);

                    //yo agregue esta lista
                    return Json(new { pois = this._ListPOIS().ToArray() }, JsonRequestBehavior.AllowGet);
                case "POST":
                    //InsertPOI (entity)
                    break;
                case "PUT":
                    //UpdatePOI (id, entity)
                    break;
                case "DELETE":
                    //DeletePOI (id)
                    break;
            }

            Response.StatusCode = 400;
            return Json(new { error = "Method not suported." }, JsonRequestBehavior.AllowGet);
        }

        
        private List<Object> _ListPOIS()
        {
            var driver = new XlsDriver();
            var conn = driver.Connect(@"C:\Users\ARCGIS\Downloads\homework-net-2K20-master\homework-net-2K20-master\MapsTest\Data\ds.xls");
            conn.Open();
            var dt = driver.ListData(conn, "POIs"); // cambiar el Pois por el nombre de la hoja que tiene los datos de categria


            var data = new List<Object>();
            foreach (DataRow row in dt.Rows)
            {
                IDictionary<string, object> props = new Dictionary<string, object>();
                foreach (DataColumn col in dt.Columns)
                {
                    props.Add(col.ColumnName.Replace(" ", "").Replace("(", "").Replace(")", ""), row[col.Ordinal]);
                }
                
                data.Add(props);
                //aca hay algo raro xq esta este return aca???
                    ///return data;
            }
            conn.Close();
            return data;
        }

        private IEnumerable<SelectListItem> _ListCategory()
        {
            var driver = new XlsDriver();
            var conn = driver.Connect(@"C:\Users\ARCGIS\Downloads\homework-net-2K20-master\homework-net-2K20-master\MapsTest\Data\ds.xls");
            conn.Open();
            var dt = driver.ListData(conn, "Categories"); // cambiar el Pois por el nombre de la hoja que tiene los datos de categria


            
            var listadoCategoria = new List<SelectListItem>();

            foreach (DataRow row in dt.Rows)
            {
                SelectListItem item = new SelectListItem() { Text = row.ItemArray[1].ToString(), Value = row.ItemArray[0].ToString() };
                listadoCategoria.Add(item);

            }
            conn.Close();
            
             
            return listadoCategoria.OrderBy(x => x.Value);
        }

    }
}
