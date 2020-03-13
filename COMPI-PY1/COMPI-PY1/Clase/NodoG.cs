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
        public int pid { get; set; }
        public int sid { get; set; }
        public int idp { get; set; }
        public string nombre { get; set; }
        public int id { get; set; }
        public string p { get; set; }
        public string s { get; set; }

        public NodoG(int id)
        {
            this.primero = null;
            this.segundo = null;
            this.id = id;
            this.nombre = id.ToString();
            this.p = "";
            this.s = "";
            this.pid = -1;
            this.sid = -1;
        }
    }
}
