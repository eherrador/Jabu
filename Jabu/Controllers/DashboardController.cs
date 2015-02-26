using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using WebAppMvcJabu.Models;

namespace WebAppMvcJabu.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        #region Dashboard Busqueda

        public ActionResult Index()
        {
            Estado _estado = new Estado();
            Segmento _segmento = new Segmento();
            TipoConstruccion _tipoConstruccion = new TipoConstruccion();

            BusquedaViewModels vm = new BusquedaViewModels();
            vm.Estados = _estado.ObtenerEstados();
            vm.Segmentos = _segmento.ObtenerSegmentos();
            vm.TiposConstruccion = _tipoConstruccion.ObtenerTiposConstruccion();
            vm.Desarrollos = Desarrollo.ObtenerDesarrollos();

            return View(vm);
        }

        [HttpPost]
        public JsonResult ObtenerDesarrollos(int id_estado, int[] ids_demarcaciones, int[] ids_segmentos, int[] ids_tipos, int[] ids_superficies)
        {
            Desarrollo _desarrollo = new Desarrollo();

            string str_demarcaciones = ids_demarcaciones != null ? string.Join(",", ids_demarcaciones) : "";
            string str_segmentos = ids_segmentos != null ? string.Join(",", ids_segmentos) : "";
            string str_tipos = ids_tipos != null ? string.Join(",", ids_tipos) : "";
            string str_superficies = ids_superficies != null ? string.Join(",", ids_superficies) : "";

            return Json(_desarrollo.ObtenerDesarrollos(id_estado, str_demarcaciones, str_segmentos, str_tipos, str_superficies), JsonRequestBehavior.DenyGet);
        }

        [HttpPost]
        public JsonResult ObtenerDemarcacionesPorEstado(int id)
        {
            Demarcacion _demarcacion = new Demarcacion();
            return Json(_demarcacion.ObtenerDemarcaciones(id), JsonRequestBehavior.DenyGet);
        }

        #endregion

        #region Dashboard Listado

        [HttpPost]
        public PartialViewResult Listado(string ids, int pagina = 1, string ordenacion = "Desarrollo", string direccion = "ASC")
        {
            int[] _ids = new int[0];
            if (!ids.Equals(string.Empty))
                _ids = ids.Split(',').Select(int.Parse).ToArray();

            int _rowsTotales = 0;
            int _paginasTotales = 0;
            int _rowsPorPagina = 15;

            IEnumerable<Desarrollo> desarrollos = null;

            switch (ordenacion.ToLower())
            {
                case "desarrollo":
                    desarrollos = Desarrollo.ObtenerDesarrollosPorPagina(_ids, pagina, _rowsPorPagina, p => p.Nombre, direccion, out _paginasTotales, out _rowsTotales);
                    break;
                case "tipo":
                    desarrollos = Desarrollo.ObtenerDesarrollosPorPagina(_ids, pagina, _rowsPorPagina, p => p.TipoConstruccion, direccion, out _paginasTotales, out _rowsTotales);
                    break;
                case "segmento":
                    desarrollos = Desarrollo.ObtenerDesarrollosPorPagina(_ids, pagina, _rowsPorPagina, p => p.TipoSegmento, direccion, out _paginasTotales, out _rowsTotales);
                    break;
                case "unids_totales":
                    desarrollos = Desarrollo.ObtenerDesarrollosPorPagina(_ids, pagina, _rowsPorPagina, p => p.UnidadesTotales, direccion, out _paginasTotales, out _rowsTotales);
                    break;
                case "unids_disponibles":
                    desarrollos = Desarrollo.ObtenerDesarrollosPorPagina(_ids, pagina, _rowsPorPagina, p => p.UnidadesDisponibles, direccion, out _paginasTotales, out _rowsTotales);
                    break;
                case "absorcion":
                    desarrollos = Desarrollo.ObtenerDesarrollosPorPagina(_ids, pagina, _rowsPorPagina, p => p.Absorcion, direccion, out _paginasTotales, out _rowsTotales);
                    break;
                case "superficie":
                    desarrollos = Desarrollo.ObtenerDesarrollosPorPagina(_ids, pagina, _rowsPorPagina, p => p.SuperficieConstruccion, direccion, out _paginasTotales, out _rowsTotales);
                    break;
                case "precio":
                    desarrollos = Desarrollo.ObtenerDesarrollosPorPagina(_ids, pagina, _rowsPorPagina, p => p.PrecioActualizado, direccion, out _paginasTotales, out _rowsTotales);
                    break;
                case "precio_metro":
                    desarrollos = Desarrollo.ObtenerDesarrollosPorPagina(_ids, pagina, _rowsPorPagina, p => p.PrecioMetroCuadrado, direccion, out _paginasTotales, out _rowsTotales);
                    break;
            }

            var vm = new ListadoViewModels()
            {
                RowsPorPagina = _rowsPorPagina,
                RowsTotales = _rowsTotales,
                PaginaActual = pagina,
                PaginasTotales = _paginasTotales,
                Desarrollos = desarrollos,
                FiltroIds = ids
            };

            return PartialView("_PartialGridDesarrollos", vm);
        }

        #endregion

        #region Dashboard Detalles

        [HttpPost]
        public PartialViewResult Detalles(string id)
        {
            List<DesarrolloDatosGenerales> _ListDatosGenerales;
            List<DesarrolloServicios> _ListServicios;
            List<DesarrolloAmenidades> _ListAmenidades;
            List<DesarrolloPrototipos> _ListPrototipos;

            DetallesViewModels vm = new DetallesViewModels();
            vm.CargaListasDetalle(int.Parse(id), out _ListDatosGenerales, out _ListServicios, out _ListAmenidades, out _ListPrototipos);
            vm.ListDatosGenerales = _ListDatosGenerales;
            vm.ListServicios = _ListServicios;
            vm.ListAmenidades = _ListAmenidades;
            vm.ListPrototipos = _ListPrototipos;

            return PartialView("_PartialDesarrolloDetalles", vm);
        }

        #endregion

        #region Dashboard Comparativo

        [HttpPost]
        public PartialViewResult Comparativo(string IdsPromedio, string IdsComparativo)
        {
            List<List<DesarrolloDatosGenerales>> ListsDatosGeneralesMaster = new List<List<DesarrolloDatosGenerales>>();
            List<List<DesarrolloServicios>> ListsServiciosMaster = new List<List<DesarrolloServicios>>();
            List<List<DesarrolloAmenidades>> ListsAmenidadesMaster = new List<List<DesarrolloAmenidades>>();

            List<DesarrolloDatosGenerales> OutListDatosGenerales = new List<DesarrolloDatosGenerales>();
            List<DesarrolloServicios> OutListServicios = new List<DesarrolloServicios>();
            List<DesarrolloAmenidades> OutListAmenidades = new List<DesarrolloAmenidades>();

            ComparativoViewModels vm = new ComparativoViewModels();

            vm.CargaListasComparativo(IdsPromedio, out OutListDatosGenerales, out OutListServicios, out OutListAmenidades);
            ListsDatosGeneralesMaster.Add(OutListDatosGenerales);
            ListsServiciosMaster.Add(OutListServicios);
            ListsAmenidadesMaster.Add(OutListAmenidades);

            if (IdsComparativo.Split(',').Length == 3)
            {
                vm.CargaListasComparativo(int.Parse(IdsComparativo.Split(',')[0]), out OutListDatosGenerales, out OutListServicios, out OutListAmenidades);
                ListsDatosGeneralesMaster.Add(OutListDatosGenerales);
                ListsServiciosMaster.Add(OutListServicios);
                ListsAmenidadesMaster.Add(OutListAmenidades);

                vm.CargaListasComparativo(int.Parse(IdsComparativo.Split(',')[1]), out OutListDatosGenerales, out OutListServicios, out OutListAmenidades);
                ListsDatosGeneralesMaster.Add(OutListDatosGenerales);
                ListsServiciosMaster.Add(OutListServicios);
                ListsAmenidadesMaster.Add(OutListAmenidades);

                vm.CargaListasComparativo(int.Parse(IdsComparativo.Split(',')[2]), out OutListDatosGenerales, out OutListServicios, out OutListAmenidades);
                ListsDatosGeneralesMaster.Add(OutListDatosGenerales);
                ListsServiciosMaster.Add(OutListServicios);
                ListsAmenidadesMaster.Add(OutListAmenidades);

                //if (IdsComparativo.Split(',').Length == 3)
                //{
                    //vm.CargaListasComparativo(int.Parse(IdsComparativo.Split(',')[2]), out OutListDatosGenerales, out OutListServicios, out OutListAmenidades);
                    //ListsDatosGeneralesMaster.Add(OutListDatosGenerales);
                    //ListsServiciosMaster.Add(OutListServicios);
                    //ListsAmenidadesMaster.Add(OutListAmenidades);
                //}

                vm.ListDatosGenerales = Utils.CombineWith(ListsDatosGeneralesMaster.ToArray()[0].ToList(),
                                                          ListsDatosGeneralesMaster.ToArray()[1].ToList(),
                                                          ListsDatosGeneralesMaster.ToArray()[2].ToList(),
                                                          ListsDatosGeneralesMaster.ToArray()[3].ToList());

                vm.ListServicios = Utils.CombineWith(ListsServiciosMaster.ToArray()[0].ToList(),
                                                     ListsServiciosMaster.ToArray()[1].ToList(),
                                                     ListsServiciosMaster.ToArray()[2].ToList(),
                                                     ListsServiciosMaster.ToArray()[3].ToList());

                vm.ListAmenidades = Utils.CombineWith(ListsAmenidadesMaster.ToArray()[0].ToList(),
                                                      ListsAmenidadesMaster.ToArray()[1].ToList(),
                                                      ListsAmenidadesMaster.ToArray()[2].ToList(),
                                                      ListsAmenidadesMaster.ToArray()[3].ToList());
            }
            else
            {
                vm.CargaListasComparativo(int.Parse(IdsComparativo.Split(',')[0]), out OutListDatosGenerales, out OutListServicios, out OutListAmenidades);
                ListsDatosGeneralesMaster.Add(OutListDatosGenerales);
                ListsServiciosMaster.Add(OutListServicios);
                ListsAmenidadesMaster.Add(OutListAmenidades);

                vm.CargaListasComparativo(int.Parse(IdsComparativo.Split(',')[1]), out OutListDatosGenerales, out OutListServicios, out OutListAmenidades);
                ListsDatosGeneralesMaster.Add(OutListDatosGenerales);
                ListsServiciosMaster.Add(OutListServicios);
                ListsAmenidadesMaster.Add(OutListAmenidades);

                vm.ListDatosGenerales2 = Utils.CombineWith2(ListsDatosGeneralesMaster.ToArray()[0].ToList(),
                                                          ListsDatosGeneralesMaster.ToArray()[1].ToList(),
                                                          ListsDatosGeneralesMaster.ToArray()[2].ToList());

                vm.ListServicios2 = Utils.CombineWith2(ListsServiciosMaster.ToArray()[0].ToList(),
                                                     ListsServiciosMaster.ToArray()[1].ToList(),
                                                     ListsServiciosMaster.ToArray()[2].ToList());

                vm.ListAmenidades2 = Utils.CombineWith2(ListsAmenidadesMaster.ToArray()[0].ToList(),
                                                      ListsAmenidadesMaster.ToArray()[1].ToList(),
                                                      ListsAmenidadesMaster.ToArray()[2].ToList());
            }

            string regreso = string.Empty;
 
            if (ListsDatosGeneralesMaster.Count == 4)
               regreso = "_PartialDesarrollosComparativo";
            if (ListsDatosGeneralesMaster.Count == 3)
                regreso = "_PartialDesarrollosComparativo2";

            return PartialView(regreso, vm);
        }

        #endregion

        public ActionResult Grafico()
        {
            return View();
        }

        public ActionResult Reporte()
        {
            return View();
        }

        #region Dashboard Utils

        [HttpPost]
        public void RegisterEventLogMessage(string message)
        {
            Utils.WriteLogMessage(HttpContext.User.Identity.Name, message);
        }

        #endregion
    }
}