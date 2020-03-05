using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COMPI_PY1.Clase
{
    class TokenError
    {
        public int noToken { get; set; }
        public string lexema { get; set; }
        public int fila { get; set; }
        public int columna { get; set; }

        public TokenError(int noToken, string lexema, int fila, int columna)
        {
            this.noToken = noToken;
            this.lexema = lexema;
            this.fila = fila;
            this.columna = columna;
        }
    }
}
