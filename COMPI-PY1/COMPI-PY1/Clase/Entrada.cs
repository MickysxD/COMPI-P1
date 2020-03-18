﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COMPI_PY1.Clase
{
    class Entrada
    {
        public Token nombre { get; set; }
        public string texto { get; set; }
        public Exprecion exprecion { get; set; }

        public Entrada(Token nombre)
        {
            this.nombre = nombre;
            this.texto = "";
        }

        public bool Comienzo()
        {
            if (exprecion != null)
            {
                return exprecion.validar(texto);
            }
            else
            {
                return false;
            }
        }
    }
}
