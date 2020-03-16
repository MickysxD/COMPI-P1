using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COMPI_PY1.Clase
{
    class Conjunto
    {
        public List<string> caracteres { get; set; }
        public Token nombre { get; set; }

        public Conjunto(Token nombre)
        {
            this.caracteres = new List<string>();
            this.nombre = nombre;
        }
    }
}
