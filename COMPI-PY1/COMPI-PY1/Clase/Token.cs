using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COMPI_PY1.Clase
{
    class Token
    {
        private int no;
        private int id;
        private string lexema;
        private int fila;
        private int columna;

        public Token(int no, int id, string lexema, int fila, int columna)
        {
            this.no = no;
            this.id = id;
            this.lexema = lexema;
            this.fila = fila;
            this.columna = columna;
        }
        

    }
}
