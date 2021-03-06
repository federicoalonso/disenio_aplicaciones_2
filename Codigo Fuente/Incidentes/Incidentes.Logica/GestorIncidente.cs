using Incidentes.DatosInterfaz;
using Incidentes.Dominio;
using Incidentes.DTOs;
using Incidentes.Excepciones;
using Incidentes.LogicaInterfaz;
using System.Collections.Generic;
using System.Linq;

namespace Incidentes.Logica
{
    public class GestorIncidente : ILogicaIncidente
    {
        IRepositorioGestores _repositorioGestor;
        private const string acceso_no_autorizado = "Acceso no autorizado";
        private const string argumento_nulo = "El argumento no puede ser nulo";
        private const string usuario_no_pertenece = "El usuario no pertenece al proyecto";
        private const string elemento_no_existe = "El elemento no existe";
        private const string elemento_ya_existe = "Un elemento con similares atributos ya existe";
        private const int largo_maximo_nombre = 50;
        private const int largo_minimo_nombre = 5;

        public GestorIncidente(IRepositorioGestores repositorioGestores)
        {
            _repositorioGestor = repositorioGestores;
        }

        public IncidenteDTO Alta(IncidenteDTO entity)
        {
            if (entity == null) throw new ExcepcionArgumentoNoValido(argumento_nulo);
            bool existe = _repositorioGestor.RepositorioIncidente.Existe(c => c.Nombre == entity.Nombre);
            if (existe) throw new ExcepcionArgumentoNoValido(elemento_ya_existe);

            Incidente alta = entity.convertirDTO_Dominio();
            Validaciones.ValidarIncidente(alta);
            _repositorioGestor.RepositorioIncidente.Alta(alta);
            _repositorioGestor.Save();

            List<Incidente> ls = new List<Incidente>();
            ls.Add(alta);
            IEnumerable<IncidenteDTO> resp = convertirListaADTO(ls);
            return resp.FirstOrDefault();
        }

        public void Baja(int id)
        {
            Incidente aEliminar = Obtener(id).convertirDTO_Dominio();

            _repositorioGestor.RepositorioIncidente.Eliminar(aEliminar);
            _repositorioGestor.Save();
        }

        public IncidenteDTO Modificar(int id, IncidenteDTO entity)
        {
            if (entity == null) throw new ExcepcionArgumentoNoValido(argumento_nulo);
            if (entity.DesarrolladorId != 0)
            {
                bool pertenece = _repositorioGestor.RepositorioProyecto.VerificarUsuarioPerteneceAlProyecto(entity.DesarrolladorId, entity.ProyectoId);
                if (!pertenece) throw new ExcepcionAccesoNoAutorizado(usuario_no_pertenece);
            }

            Incidente aModificar = Obtener(id).convertirDTO_Dominio();
            Validaciones.ValidarIncidente(aModificar);

            aModificar = CopiarIncidente(aModificar, entity);

            _repositorioGestor.RepositorioIncidente.Modificar(aModificar);
            _repositorioGestor.Save();

            List<Incidente> ls = new List<Incidente>();
            ls.Add(aModificar);
            IEnumerable<IncidenteDTO> resp = convertirListaADTO(ls);
            return resp.FirstOrDefault();
        }

        public IncidenteDTO Obtener(int id)
        {
            bool existe = _repositorioGestor.RepositorioIncidente.Existe(c => c.Id == id);
            if (!existe) throw new ExcepcionElementoNoExiste(elemento_no_existe);
            Incidente aObtener = _repositorioGestor.RepositorioIncidente.ObtenerPorCondicion(c => c.Id == id, false).FirstOrDefault();
            List<Incidente> ls = new List<Incidente>();
            ls.Add(aObtener);
            IEnumerable<IncidenteDTO> resp = convertirListaADTO(ls);
            return resp.FirstOrDefault();
        }

        public IEnumerable<IncidenteDTO> ObtenerTodos()
        {
            return convertirListaADTO(_repositorioGestor.RepositorioIncidente.ObtenerTodos(false).ToList());
        }

        public List<IncidenteDTO> ListaDeIncidentesDeLosProyectosALosQuePertenece(int idUsuario, string nombreProyecto, IncidenteDTO incidente)
        {
            return convertirListaADTO(_repositorioGestor.RepositorioUsuario.ListaDeIncidentesDeLosProyectosALosQuePertenece(idUsuario, nombreProyecto, incidente.convertirDTO_Dominio())).ToList();
        }

        public IncidenteDTO ObtenerParaUsuario(int idUsuario, int idIncidente)
        {
            Incidente inc = Obtener(idIncidente).convertirDTO_Dominio();
            if (!_repositorioGestor.RepositorioProyecto.VerificarIncidentePerteneceAlProyecto(idIncidente, inc.ProyectoId))
                throw new ExcepcionAccesoNoAutorizado(acceso_no_autorizado);
            List<Incidente> ls = new List<Incidente>();
            ls.Add(inc);
            IEnumerable<IncidenteDTO> resp = convertirListaADTO(ls);
            return resp.FirstOrDefault();
        }

        private Incidente CopiarIncidente(Incidente copia, IncidenteDTO copiado)
        {
            Incidente inci = copiado.convertirDTO_Dominio();
            if (copia.Nombre != copiado.Nombre && copiado.Nombre != null)
            {
                bool existe = _repositorioGestor.RepositorioIncidente.Existe(c => c.Nombre == copiado.Nombre);
                if (existe) throw new ExcepcionArgumentoNoValido(elemento_ya_existe);
                Validaciones.ValidarLargoTexto(copiado.Nombre, largo_maximo_nombre, largo_minimo_nombre, "Nombre Incidente");
            }

            if (copiado.Nombre != null) copia.Nombre = copiado.Nombre;
            if (inci.EstadoIncidente != Incidente.Estado.Indiferente) copia.EstadoIncidente = inci.EstadoIncidente;
            if (copiado.ProyectoId != 0) copia.ProyectoId = copiado.ProyectoId;
            if (copiado.Version != null) copia.Version = copiado.Version;
            if (copiado.Descripcion != null) copia.Descripcion = copiado.Descripcion;
            if (copiado.DesarrolladorId != 0)
            {
                copia.DesarrolladorId = copiado.DesarrolladorId;
            }
            else
            {
                copia.DesarrolladorId = 0;
            }
            if (copiado.Duracion != 0) copia.Duracion = copiado.Duracion;
            return copia;
        }
        private IEnumerable<IncidenteDTO> convertirListaADTO(List<Incidente> incidentes)
        {
            List<IncidenteDTO> ret = new List<IncidenteDTO>();
            foreach (Incidente i in incidentes)
            {
                IncidenteDTO nueva = new IncidenteDTO(i);
                Proyecto p = _repositorioGestor.RepositorioProyecto.ObtenerPorCondicion(p => p.Id == i.ProyectoId, false).FirstOrDefault();
                nueva.NombreProyecto = p.Nombre;
                if (i.DesarrolladorId != 0)
                {
                    Usuario u = _repositorioGestor.RepositorioUsuario.ObtenerPorCondicion(u => u.Id == i.DesarrolladorId, false).FirstOrDefault();
                    nueva.DesarrolladorNombre = u.Nombre + " " + u.Apellido;
                }
                ret.Add(nueva);
            }
            return ret;
        }
    }
}
