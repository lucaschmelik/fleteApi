﻿using System;

namespace fleteApi.Models
{
    public class Viaje
    {
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
    }
}
