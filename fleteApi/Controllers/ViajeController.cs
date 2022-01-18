using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.PeerToPeer.Collaboration;
using System.Security.Cryptography.X509Certificates;
using fleteApi.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace fleteApi.Controllers
{
    [Route("api/viajes")]
    public class ViajeController : ControllerBase
    {
        private readonly ViajeContext context;

        public ViajeController(ViajeContext oContext) { context = oContext; }

        [HttpGet]
        public ActionResult GetViajes()
        {
            try
            {
                return Ok(context.Viaje.ToList());
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
                return Ok(context.Viaje.FirstOrDefault(viaje => viaje.Id == id));
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
                return Ok(context.Viaje.Where(viaje => viaje.FechaEntrega.Value.Date == fecha.Date).ToList());
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
                return Ok(context.Viaje.Where(viaje => viaje.FechaEntrega.Value.Date == fecha.Date && viaje.Completado == null).OrderBy(x=> x.Orden ?? x.HorarioDesde).ThenBy(x=>x.HorarioHasta).ToList());
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
                return Ok(context.Viaje.Where(viaje => viaje.FechaEntrega.Value.Date == fecha.Date && viaje.Completado != null).OrderBy(x=>x.Completado).ToList());
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
                var listaViajes = context.Viaje.Where(x => x.FechaEntrega.Value.Month == fecha.Month && x.FechaEntrega.Value.Year == fecha.Year).ToList();

                var listaDias = new List<DiaCalendario>();

                foreach (var dia in listaViajes.Where(x => x.FechaEntrega != null).Select(x => x.FechaEntrega.Value.Day).Distinct())
                {
                    var viajesEncontrados = listaViajes.Where(x => x.FechaEntrega != null && x.FechaEntrega.Value.Day == dia).ToList();

                    var cantidadViajesCompletados = viajesEncontrados.Count(x => x.Completado != null);
                    var cantidadViajesPendientes = viajesEncontrados.Count(x => x.Completado == null);

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
                            content = $"{cantidadViajesPendientes} viajes pendientes",
                            type = "warning"
                        });
                    }
                    if (cantidadViajesCompletados != 0)
                    {
                        listaDias[^1].Items.Add(new DiaCalendarioItem()
                        {
                            content = $"{cantidadViajesCompletados} viajes completos",
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
        public ActionResult PostViaje([FromBody]Viaje viaje)
        {
            try
            {
                context.Viaje.Add(viaje);
                context.SaveChanges();
                return Ok(viaje);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("entregado")]
        public ActionResult FinalizarViaje(int id, DateTime completado)
        {
            try
            {
                var viajeEncontrado = context.Viaje.FirstOrDefault(x => x.Id == id);

                if (viajeEncontrado == null) return BadRequest("No se encontró el viaje a modificar.");

                viajeEncontrado.Completado = completado;

                context.Entry(viajeEncontrado).State = EntityState.Modified;

                context.SaveChanges();

                return Ok(viajeEncontrado);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("deshacer")]
        public ActionResult DeshacerEntregadoViaje(int id)
        {
            try
            {
                var viajeEncontrado = context.Viaje.FirstOrDefault(x => x.Id == id);

                if (viajeEncontrado == null) return BadRequest("No se encontró el viaje a modificar.");

                viajeEncontrado.Completado = null;

                context.Entry(viajeEncontrado).State = EntityState.Modified;

                context.SaveChanges();

                return Ok(viajeEncontrado);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("orden")]
        public ActionResult OrdenarViaje(int id, int ordenNuevo)
        {
            try
            {
                var viajeEncontrado = context.Viaje.FirstOrDefault(x => x.Id == id);

                if (viajeEncontrado == null) return BadRequest("No se encontró el viaje a modificar.");

                viajeEncontrado.Orden = ordenNuevo;

                context.Entry(viajeEncontrado).State = EntityState.Modified;

                context.SaveChanges();

                return Ok(viajeEncontrado);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut]
        public ActionResult ModificarViaje([FromBody] Viaje viaje)
        {
            try
            {
                context.Entry(viaje).State = EntityState.Modified;

                context.SaveChanges();

                return Ok(viaje);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("{id}")]
        public ActionResult EliminarViaje(int id)
        {
            try
            {
                var viaje = context.Viaje.FirstOrDefault(viaje => viaje.Id == id);

                if (viaje == null) return BadRequest("No se encontró el viaje a eliminar.");

                context.Viaje.Remove(viaje);

                context.SaveChanges();

                return Ok(viaje);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

    }
}
