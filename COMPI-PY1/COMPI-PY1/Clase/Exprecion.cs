using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COMPI_PY1.Clase
{
    class Exprecion
    {
        public Token nombre { get; set; }
        public Grafo grafo { get; set;}
        public List<Token> tokens { get; set; }
        public List<Transicion> transiciones { get; set; }
        int estado = 0;

        public Exprecion()
        {
            this.tokens = new List<Token>();
            this.transiciones = new List<Transicion>();
            this.grafo = new Grafo();
        }

        public void comienzo()
        {
            Transicion temp = new Transicion("S" + estado);
            temp.anterior.Add(0);
            temp.lista.Add(0);
            transiciones.Add(temp);
            //llena la cabecera con los siguientes de las posibles transiciones
            siguientesCabecera(temp);
            //llena la cabecera con los numeros que tienen salto ε
            llenarCabecera(temp);

            temp.lista = temp.lista.Distinct().ToList();
            temp.lista.Sort();
            estado++;
            
            for (int i = 0; i < transiciones.Count; i++)
            {
                Transicion nodocol = transiciones[i];

                Transicion nodofila = nodocol.siguiente;
                while (nodofila != null)
                {
                    llenarNodos(nodocol.lista, grafo.nodos, nodofila);
                    nodofila.lista = nodofila.lista.Distinct().ToList();
                    nodofila.lista.Sort();

                    if (nodofila.lista.Count != 0)
                    {
                        nodofila.estado = crearCabecera(nodofila);
                    }
                    else
                    {
                        nodofila.estado = null;
                    }
                    
                    nodofila = nodofila.siguiente;
                }
            }
            
            

            int v = temp.lista.Find(x => x == grafo.nodos.Count-1);
            if (v == grafo.nodos.Count - 1)
            {
                temp.aceptacion = true;
            }
            graficarTabla();
            graficarTrans();
        }
        //nuevo.lista.Find(x => x == i); nuevo.Sort()
        public void siguientesCabecera(Transicion temp)
        {
            Transicion t = temp;
            for (int i = 0; i < grafo.transiciones.Count; i++)
            {
                Transicion nuevo = new Transicion(grafo.transiciones[i].lexema);
                nuevo.tipo = grafo.transiciones[i].idToken;
                t.siguiente = nuevo;
                t = nuevo;
            }
        }

        public void llenarCabecera(Transicion temp)
        {
            for (int i = 0; i < temp.anterior.Count; i++)
            {
                temp.lista.Add(temp.anterior[i]);
                buscarVacios(temp, grafo.nodos[temp.anterior[i]]);
            }
            temp.lista = temp.lista.Distinct().ToList();
            temp.lista.Sort();

            int v = temp.lista.Find(x => x == grafo.nodos.Count - 1);
            if (v == grafo.nodos.Count - 1)
            {
                temp.aceptacion = true;
            }
        }

        public void buscarVacios(Transicion temp, NodoG raiz)
        {
            if (raiz.p.Equals("ε"))
            {
                int ver = temp.lista.Find(x => x == raiz.primero.id);
                if (ver != raiz.primero.id)
                {
                    temp.lista.Add(raiz.primero.id);
                    buscarVacios(temp, raiz.primero);
                }
               
            }

            if (raiz.s.Equals("ε"))
            {
                int ver = temp.lista.Find(x => x == raiz.segundo.id);
                if (ver != raiz.segundo.id)
                {
                    temp.lista.Add(raiz.segundo.id);
                    buscarVacios(temp, raiz.segundo);
                }
            }
                
        }

        public void llenarNodos(List<int> lista, List<NodoG> nodos, Transicion temp)
        {
            for (int i = 0; i < lista.Count; i++)
            {
                NodoG nt = nodos[lista[i]];
                if (nt.primero != null && nt.p.Equals(temp.nombre) && nt.idp == temp.tipo)
                {
                    temp.lista.Add(nt.primero.id);
                }
                //buscarCoincidencias(temp, nt);
            }
        }
        public void buscarCoincidencias(Transicion temp, NodoG raiz)
        {
            if (raiz.primero != null && raiz.p.Equals(temp.nombre) && raiz.idp == temp.tipo)
            {
                int ver = temp.lista.Find(x => x == raiz.primero.id);
                if (ver != raiz.primero.id)
                {
                    temp.lista.Add(raiz.primero.id);
                    buscarCoincidencias(temp, raiz.primero);
                }
            }

            if (raiz.p.Equals("ε"))
            {
                int ver = temp.lista.Find(x => x == raiz.primero.id);
                if (ver != raiz.primero.id)
                {
                    temp.lista.Add(raiz.primero.id);
                    buscarCoincidencias(temp, raiz.primero);
                }

            }

            if (raiz.s.Equals("ε"))
            {
                int ver = temp.lista.Find(x => x == raiz.segundo.id);
                if (ver != raiz.segundo.id)
                {
                    temp.lista.Add(raiz.segundo.id);
                    buscarCoincidencias(temp, raiz.segundo);
                }
            }

        }

        public Transicion crearCabecera(Transicion temp)
        {
            //bool cumple = false;
            
            for (int i = 0; i < transiciones.Count; i++)
            {
                Transicion t = transiciones[i];


                List<int> ups = t.anterior.Intersect(temp.lista).ToList();

                if (ups.Count == temp.lista.Count && t.anterior.Count == ups.Count)
                {
                    return t;
                }

                //if (transiciones[i].anterior.Count == temp.lista.Count)
                //{

                //    for (int j = 0; j < temp.lista.Count; j++)
                //    {
                //        if (transiciones[i].anterior[j] == temp.lista[j])
                //        {
                //            cumple = true;
                //        }
                //        else
                //        {
                //            break;
                //        }
                //    }

                //}

                //if (cumple)
                //{
                //    return transiciones[i];
                //}

            }
            
            

            //if (!cumple)
            //{
                Transicion nuevo = new Transicion("S" + estado);
                nuevo.anterior = temp.lista;
                transiciones.Add(nuevo);
                siguientesCabecera(nuevo);
                llenarCabecera(nuevo);
                estado++;

                return nuevo;
            //}

            //return null;
        }

        public void graficarTabla()
        {
            StreamWriter escribir = new StreamWriter("Reportes\\" + nombre.lexema + "Tabla.txt");
            //escribir.WriteLine("digraph D{\nrankdir=LR;");

            escribir.WriteLine("digraph grafico{\ngraph [pad=\"0.5\", nodesep=\"0.5\", ranksep=\"2\"];\nnode [shape=plain]\nrankdir=LR;\n");
            escribir.Write("Foo [label=<\n<table border=\"0\" cellborder=\"1\" cellspacing=\"0\">\n<tr><td><i><b>Estado</b></i></td>");

            for (int i = 0; i < grafo.transiciones.Count; i++)
            {
                Token nodot = grafo.transiciones[i];
                if (nodot.lexema.Equals(">"))
                {
                    escribir.Write("<td><i><b>Signo mayor que</b></i></td>");
                }else if (nodot.lexema.Equals("<"))
                {
                    escribir.Write("<td><i><b>Signo menor que</b></i></td>");
                }
                else
                {
                    escribir.Write("<td><i><b>" + nodot.lexema + "</b></i></td>");
                }
            }

            escribir.Write("</tr>\n");
            
            for (int i = 0; i < transiciones.Count; i++)
            {
                Transicion tempcol = transiciones[i];

                if (tempcol.aceptacion)
                {
                    escribir.Write("<tr><td><b>*" + tempcol.nombre + " = [" + String.Join(", ", tempcol.lista) + "]</b></td>");
                }
                else
                {
                    escribir.Write("<tr><td><b>" + tempcol.nombre + " = [" + String.Join(", ", tempcol.lista) + "]</b></td>");
                }
                
            
                Transicion tempfila = tempcol.siguiente;
                while (tempfila != null)
                {
                    if (tempfila.estado != null)
                    {
                        escribir.Write("<td>[" + String.Join(", ", tempfila.lista) + "] = " + tempfila.estado.nombre + "</td>");
                    }
                    else
                    {
                        escribir.Write("<td>[" + String.Join(", ", tempfila.lista) + "]</td>");

                    }

                    tempfila = tempfila.siguiente;
                }

                escribir.Write("</tr>\n");
            }

            escribir.WriteLine("</table>>];\n}");
            
            escribir.Close();

            string texto = "/K dot -Tpng Reportes\\" + nombre.lexema + "Tabla.txt -o Reportes\\" + nombre.lexema + "Tabla.jpg";

            //System.Diagnostics.Process.Start("CMD.exe", "/K dot -Tpng Ella.txt -o UML.png");

            System.Diagnostics.Process.Start("CMD.exe", texto).Close();
            //System.Diagnostics.Process.Start("Reportes\\"+nombre.lexema+"Grafo.jpg");
        }

        public void graficarTrans()
        {
            StreamWriter escribir = new StreamWriter("Reportes\\" + nombre.lexema + "AFD.txt");
            escribir.WriteLine("digraph D{\nrankdir=LR;node[];\n");

            Transicion temp = null;
            for (int j = 0; j < transiciones.Count; j++)
            {
                temp = transiciones[j];

                Transicion n = temp.siguiente;
                if (temp.aceptacion)
                {
                    escribir.WriteLine(temp.nombre + "[label=\"" + temp.nombre + "\", shape=doublecircle];");
                }
                else
                {
                    escribir.WriteLine(temp.nombre + "[label=\"" + temp.nombre + "\", shape=circle];");
                }
                
                while (n != null)
                {
                    if (n.estado != null)
                    {
                        escribir.WriteLine(temp.nombre+"->" + n.estado.nombre + "[label=\"" + n.nombre + "\"];");
                    }
                    n = n.siguiente;
                }

            }
            escribir.WriteLine("}");

            escribir.Close();

            string texto = "/K dot -Tpng Reportes\\" + nombre.lexema + "AFD.txt -o Reportes\\" + nombre.lexema + "AFD.jpg";

            //System.Diagnostics.Process.Start("CMD.exe", "/K dot -Tpng Ella.txt -o UML.png");

            System.Diagnostics.Process.Start("CMD.exe", texto).Close();
            //System.Diagnostics.Process.Start("Reportes\\"+nombre.lexema+"Grafo.jpg");
        }

    }
}
