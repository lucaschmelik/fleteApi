using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace fleteApi.Models
{
    public class Viaje
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int? HorarioDesde { get; set; }
        public int? HorarioHasta { get; set; }
        public string Barrio { get; set; }
        public string Direccion { get; set; }
        public string Recibe { get; set; }
        public string Envia { get; set; }
        public string Telefono { get; set; }
        public DateTime? Completado { get; set; }
        public DateTime? FechaEntrega { get; set; }
        public int? Orden { get; set; }
        public string Observaciones { get; set; }
    }
}
