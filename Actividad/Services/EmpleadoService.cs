using Actividad.Entities;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Actividad.Services
{
    public class EmpleadoService
    {
        public event Func<string, Task> OnSearch = delegate { return Task.CompletedTask; };

        public async Task notificarBusqueda(string nombrerepresentante)
        {
            if (OnSearch != null)
            {
                await OnSearch.Invoke(nombrerepresentante);
            }
        }

        private List<RepresentanteListCLS> lista;
        private JefeRepresentanteService jefeService;
        private SucursalService sucursalService;
        public EmpleadoService()
        {
            jefeService = new JefeRepresentanteService();
            sucursalService = new SucursalService();
            lista = new List<RepresentanteListCLS>();
            lista.Add(new RepresentanteListCLS { Num_Empl = 1, Nombre = "Empleado 1", nombreJefe = "Charly", nombreSucursal = "Santa Cruz" });
            lista.Add(new RepresentanteListCLS { Num_Empl = 2, Nombre = "Empleado 2",nombreJefe = "Miguel" , nombreSucursal = "La Paz"});
        }
        public List<RepresentanteListCLS> listarRepresentantes()
        {
            return lista;
        }

        public List<RepresentanteListCLS> filtrarRepresentantes(string nombrerepresentante)
        {
            List<RepresentanteListCLS> l = listarRepresentantes();
            if(nombrerepresentante == "")
            {
                return l;
            }
            else
            {
                List<RepresentanteListCLS> listafiltrada = l.Where(p => p.Nombre.ToUpper().Contains(nombrerepresentante.ToUpper())).ToList();
                return listafiltrada;
            }
        }

        public void eliminarEmpleado(int Num_Empl)
        {
            var listaQueda = lista.Where(p => p.Num_Empl != Num_Empl).ToList();
            lista = listaQueda;
        }

        public RepresentantesVentasCLS recuperarRepresentantePorId(int Num_Empl)
        {
            var obj = lista.Where(p => p.Num_Empl == Num_Empl).FirstOrDefault();
            if (obj != null)
            {
                return new RepresentantesVentasCLS { 
                    Num_Empl = obj.Num_Empl, 
                    Nombre = obj.Nombre, Cargo = obj.Cargo,
                    Edad = obj.Edad, 
                    FechaContrato = obj.FechaContrato, 
                    Cuota = obj.Cuota, Ventas = obj.Ventas,
                    idJefe = jefeService.obtenerIdJefe(obj.nombreJefe),
                    idSucursal = sucursalService.obtenerIdSucursal(obj.nombreSucursal)};
            }
            else
            {
                return new RepresentantesVentasCLS();
            }
        }

        public int obtenerIdEmpleado(string Nombre)
        {
            var obj = lista.Where(p => p.Nombre == Nombre).FirstOrDefault();
            if (obj == null)
            {
                return 0;
            }
            else
            {
                return obj.Num_Empl;
            }
        }
        public string obtenerNombreEmpleado(int Num_Empl)
        {
            var obj = lista.Where(p => p.Num_Empl == Num_Empl).FirstOrDefault();
            if (obj == null)
            {
                return "";
            }
            else
            {
                return obj.Nombre;
            }
        }

        public void guardarRepresentante(RepresentantesVentasCLS representante)
        {
            if (representante.Num_Empl == 0)
            {
                int Num_Empl = lista.Select(p => p.Num_Empl).Max() + 1;
                lista.Add(new RepresentanteListCLS
                {
                    Num_Empl = Num_Empl,
                    Nombre = representante.Nombre,
                    Edad = representante.Edad,
                    Cargo = representante.Cargo,
                    FechaContrato = representante.FechaContrato,
                    Cuota = representante.Cuota,
                    Ventas = representante.Ventas,
                    nombreJefe = jefeService.obtenerNombreJefe(representante.idJefe),
                    nombreSucursal = sucursalService.obtenerNombreSucursal(representante.idSucursal)
                });
            }
            else
            {
                var obj = lista.Where(p => p.Num_Empl == representante.Num_Empl).FirstOrDefault();
                if (obj != null)
                {
                    obj.Nombre = representante.Nombre;
                    obj.Edad = representante.Edad;
                    obj.Cargo = representante.Cargo;
                    obj.FechaContrato = representante.FechaContrato;
                    obj.Cuota = representante.Cuota;
                    obj.Ventas = representante.Ventas;
                    obj.nombreJefe = jefeService.obtenerNombreJefe(representante.idJefe);
                    obj.nombreSucursal = sucursalService.obtenerNombreSucursal(representante.idSucursal);
                }
            }
        }
    }
}