using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COMPI_PY1.Clase
{
    class NodoG
    {
        public NodoG primero { get; set; }
        public NodoG segundo { get; set; }
        public string nombre { get; set; }
        public string p { get; set; }
        public string s { get; set; }

        public NodoG(string nombre)
        {
            this.primero = null;
            this.segundo = null;
            this.nombre = nombre;
            this.p = "";
            this.s = "";
        }
    }
}
