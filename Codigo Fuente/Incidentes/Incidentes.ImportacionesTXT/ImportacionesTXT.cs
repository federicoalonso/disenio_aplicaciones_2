using Incidentes.DTOs;
using Incidentes.LogicaInterfaz;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Incidentes.ImportacionesTXT
{
    public class ImportacionTXT : IFuente
    {
        private string _rutaFuente { get; set; }

        public List<ProyectoDTO> ImportarBugs(string rutaFuente)
        {
            _rutaFuente = rutaFuente;
            List<ProyectoDTO> retorno = new List<ProyectoDTO>();
            List<string> lineas = File.ReadAllLines(_rutaFuente).ToList();
            int id_bug = 30;
            int nombre_bug = id_bug + 4 + 1;
            int descripcion_bug = nombre_bug + 60 + 1;
            int version_bug = descripcion_bug + 150 + 1;
            int estado_bug = version_bug + 10 + 1;
            foreach (var item in lineas)
            {
                string nombreProyecto = item.Substring(0, 30).Trim();
                string nombreIncidente = item.Substring(nombre_bug, 60).Trim();
                string descripcionIncidente = item.Substring(descripcion_bug, 140).Trim();
                string versionIncidente = item.Substring(version_bug, 10).Trim();
                string estadoIncidente = item.Substring(estado_bug).Trim();

                ProyectoDTO proyecto = new ProyectoDTO()
                {
                    Nombre = nombreProyecto
                };

                IncidenteDTO incidente = new IncidenteDTO()
                {
                    Nombre = nombreIncidente,
                    Descripcion = descripcionIncidente,
                    Version = versionIncidente
                };
                if (estadoIncidente.Equals("Activo"))
                {
                    incidente.EstadoIncidente = IncidenteDTO.Estado.Activo;
                }
                else
                {
                    incidente.EstadoIncidente = IncidenteDTO.Estado.Resuelto;
                }
                proyecto.Incidentes.Add(incidente);
                retorno.Add(proyecto);
            }
            return retorno;
        }
    }
}
