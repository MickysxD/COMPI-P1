using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COMPI_PY1.Clase
{
    class Transicion
    {
        public List<int> lista { get; set; }
        public List<int> anterior { get; set; }
        public Transicion siguiente { get; set; }
        public string nombre { get; set; }
        public bool aceptacion { get; set; }
        public int tipo { get; set; }
        public Transicion estado { get; set; }

        public Transicion(string nombre)
        {
            this.lista = new List<int>();
            this.anterior = new List<int>();
            this.siguiente = null;
            this.nombre = nombre;
        }

        public Transicion(string nombre, List<int> nueva)
        {
            this.lista = nueva;
            this.siguiente = null;
            this.nombre = nombre;
        }
    }
}
