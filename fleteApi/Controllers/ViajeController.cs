using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using fleteApi.Models;
using Microsoft.EntityFrameworkCore;

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

        [HttpPut("completado")]
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
