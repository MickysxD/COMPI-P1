using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COMPI_PY1.Clase
{
    class LexemaError
    {
        public List<string> esperado { get; set; }
        public string caracter { get; set; }
        public int posicion { get; set; }

        public LexemaError(string caracter, int posicion)
        {
            this.esperado = new List<string>();
            this.caracter = caracter;
            this.posicion = posicion;
        }
    }
}
