using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using fleteApi.Context;
using fleteApi.Models;
using Newtonsoft.Json;

namespace fleteApi.Controllers
{
    [Route("api/Viajes")]
    public class ViajesController : ControllerBase
    {
        private readonly FleteContext context;

        public ViajesController(FleteContext oContext) { context = oContext; }

        [HttpGet]
        public ActionResult GetViajes()
        {
            try
            {
                return Ok(context.Viajes.ToList());
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("{id}")]
        public ActionResult GetViajesPorId(int id)
        {
            try
            {
                return Ok(context.Viajes.FirstOrDefault(Viajes => Viajes.Id == id));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("fecha")]
        public ActionResult GetViajesPorFecha(DateTime fecha)
        {
            try
            {
                return Ok(context.Viajes.Where(Viajes => Viajes.FechaEntrega.Value.Date == fecha.Date).ToList());
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("pendientes")]
        public ActionResult GetViajesPendientesPorFecha(DateTime fecha)
        {
            try
            {
                return Ok(context.Viajes.Where(Viajes => Viajes.FechaEntrega.Value.Date == fecha.Date && Viajes.Completado == null).OrderBy(x => x.Orden ?? x.HorarioDesde).ThenBy(x => x.HorarioHasta).ToList());
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("completados")]
        public ActionResult GetViajesCompletadosPorFecha(DateTime fecha)
        {
            try
            {
                return Ok(context.Viajes.Where(Viajes => Viajes.FechaEntrega.Value.Date == fecha.Date && Viajes.Completado != null).OrderBy(x => x.Completado).ToList());
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("calendario")]
        public ActionResult GetViajesCalendarioPorFecha(DateTime fecha)
        {
            try
            {
                var listaViajes = context.Viajes.Where(x => x.FechaEntrega.Value.Month == fecha.Month && x.FechaEntrega.Value.Year == fecha.Year).ToList();

                var listaDias = new List<DiaCalendario>();

                foreach (var dia in listaViajes.Where(x => x.FechaEntrega != null).Select(x => x.FechaEntrega.Value.Day).Distinct())
                {
                    var ViajesEncontrados = listaViajes.Where(x => x.FechaEntrega != null && x.FechaEntrega.Value.Day == dia).ToList();

                    var cantidadViajesCompletados = ViajesEncontrados.Count(x => x.Completado != null);
                    var cantidadViajesPendientes = ViajesEncontrados.Count(x => x.Completado == null);

                    listaDias.Add(new DiaCalendario()
                    {
                        NumeroDia = dia,
                        NumeroMes = fecha.Month,
                        Status = true,
                        Items = new List<DiaCalendarioItem>()
                    });

                    if (cantidadViajesPendientes != 0)
                    {
                        listaDias[^1].Items.Add(new DiaCalendarioItem()
                        {
                            content = $"{cantidadViajesPendientes} Viajes pendientes",
                            type = "warning"
                        });
                    }
                    if (cantidadViajesCompletados != 0)
                    {
                        listaDias[^1].Items.Add(new DiaCalendarioItem()
                        {
                            content = $"{cantidadViajesCompletados} Viajes completos",
                            type = "success"
                        });
                    }
                }

                return Ok(JsonConvert.SerializeObject(listaDias));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        public ActionResult PostViajes([FromBody] Viaje Viajes)
        {
            try
            {
                context.Viajes.Add(Viajes);
                context.SaveChanges();
                return Ok(Viajes);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("entregado")]
        public ActionResult FinalizarViajes(int id, DateTime completado)
        {
            try
            {
                var ViajesEncontrado = context.Viajes.FirstOrDefault(x => x.Id == id);

                if (ViajesEncontrado == null) return BadRequest("No se encontró el Viajes a modificar.");

                ViajesEncontrado.Completado = completado;

                context.Entry(ViajesEncontrado).State = EntityState.Modified;

                context.SaveChanges();

                return Ok(ViajesEncontrado);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("deshacer")]
        public ActionResult DeshacerEntregadoViajes(int id)
        {
            try
            {
                var ViajesEncontrado = context.Viajes.FirstOrDefault(x => x.Id == id);

                if (ViajesEncontrado == null) return BadRequest("No se encontró el Viajes a modificar.");

                ViajesEncontrado.Completado = null;

                context.Entry(ViajesEncontrado).State = EntityState.Modified;

                context.SaveChanges();

                return Ok(ViajesEncontrado);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("orden")]
        public ActionResult OrdenarViajes(int id, int ordenNuevo)
        {
            try
            {
                var ViajesEncontrado = context.Viajes.FirstOrDefault(x => x.Id == id);

                if (ViajesEncontrado == null) return BadRequest("No se encontró el Viajes a modificar.");

                ViajesEncontrado.Orden = ordenNuevo;

                context.Entry(ViajesEncontrado).State = EntityState.Modified;

                context.SaveChanges();

                return Ok(ViajesEncontrado);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut]
        public ActionResult ModificarViajes([FromBody] Viaje Viajes)
        {
            try
            {
                context.Entry(Viajes).State = EntityState.Modified;

                context.SaveChanges();

                return Ok(Viajes);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("{id}")]
        public ActionResult EliminarViajes(int id)
        {
            try
            {
                var Viajes = context.Viajes.FirstOrDefault(Viajes => Viajes.Id == id);

                if (Viajes == null) return BadRequest("No se encontró el Viajes a eliminar.");

                context.Viajes.Remove(Viajes);

                context.SaveChanges();

                return Ok(Viajes);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

    }
}
