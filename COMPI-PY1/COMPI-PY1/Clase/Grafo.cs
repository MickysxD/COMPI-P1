using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COMPI_PY1.Clase
{
    class Grafo
    {
        public List<Token> tokens { get; set; }
        public List<NodoG> nodos { get; set; }
        public NodoG primero { get; set; }
        public NodoG ultimo { get; set; }
        public int id { get; set; }
        public int i { get; set; }
        public Token nombre { get; set; }
        int contador = 1;
        public Grafo()
        {
            this.nodos = new List<NodoG>();
            this.id = 0;
            this.i = 0;
        }

        public void comienzo(List<Token> t, Token nombre)
        {
            this.tokens = t;
            this.nombre = nombre;

            primero = llenar();
            graficar();
        }

        public NodoG llenar()
        {
            if (primero == null)
            {
                NodoG p = new NodoG(id.ToString());
                nodos.Add(p);
                primero = p;
                id++;
            }

            if (tokens[i].idToken == 1)
            {
                i++;
                
                NodoG m = punto();
                
                NodoG u = punto();

                return m;
            }
            else if (tokens[i].idToken == 4)
            {
                i++;

                int pri = id-1;
                
                NodoG s = new NodoG(id.ToString());
                int seg = id;
                nodos[pri].p = "ε";
                nodos[pri].primero = s;
                nodos.Add(s);
                id++;

                NodoG t = asterisco();

                NodoG c = new NodoG(id.ToString());
                nodos[id - 1].p = "ε";
                nodos[id - 1].primero = s;
                nodos[id - 1].s = "ε";
                nodos[id - 1].segundo = c;
                nodos[pri].s = "ε";
                nodos[pri].segundo = c;
                nodos.Add(c);
                id++;

                return s;
            }
            else if (tokens[i].idToken == 2)
            {
                i++;
                int p = id-1;

                NodoG s1 = new NodoG(id.ToString());
                nodos[p].p = "ε";
                nodos[p].primero = s1;
                nodos.Add(s1);
                id++;

                NodoG t1 = or();

                NodoG c = new NodoG("");
                nodos[id - 1].p = "ε";
                nodos[id - 1].primero = c;

                NodoG s2 = new NodoG(id.ToString());
                nodos[p].s = "ε";
                nodos[p].segundo = s2;
                nodos.Add(s2);
                id++;

                NodoG t2 = or();

                c.nombre = id.ToString();
                nodos[id-1].p = "ε";
                nodos[id-1].primero = c;
                nodos.Add(c);
                id++;

                return nodos[p];
            }
            else if (tokens[i].idToken == 3)
            {
                i++;
                int p = id - 1;

                NodoG s1 = new NodoG(id.ToString());
                nodos[p].p = "ε";
                nodos[p].primero = s1;
                nodos.Add(s1);
                id++;

                NodoG t1 = or();

                NodoG c = new NodoG("");
                nodos[id - 1].p = "ε";
                nodos[id - 1].primero = c;

                //comentar s2 y t2 para omitir epxilons y ser directo
                NodoG s2 = new NodoG(id.ToString());
                nodos[p].s = "ε";
                nodos[p].segundo = s2;
                nodos.Add(s2);
                id++;

                NodoG t2  = new NodoG(id.ToString());
                nodos[id - 1].p = "ε";
                nodos[id - 1].primero = t2;
                nodos.Add(t2);
                id++;
                
                //cambiar tambien (id-1 por p) y (p por s) y (primero por segundo)
                c.nombre = id.ToString();
                nodos[id - 1].p = "ε";
                nodos[id - 1].primero = c;
                nodos.Add(c);
                id++;

                return nodos[p];
            }
            if (tokens[i].idToken == 5)
            {
                i++;

                NodoG p;
                bool si = false;
                if (tokens[i+1].idToken != 15 || tokens[i + 1].idToken != 19)
                {
                    p = punto();
                    i -= 2+contador;
                    si = true;
                }
                else
                {
                    p = punto();
                    i--;
                }
                


                int pri = id - 1;

                NodoG s = new NodoG(id.ToString());
                int seg = id;
                nodos[pri].p = "ε";
                nodos[pri].primero = s;
                nodos.Add(s);
                id++;

                NodoG t = asterisco();

                NodoG c = new NodoG(id.ToString());
                nodos[id - 1].p = "ε";
                nodos[id - 1].primero = s;
                nodos[id - 1].s = "ε";
                nodos[id - 1].segundo = c;
                nodos[pri].s = "ε";
                nodos[pri].segundo = c;
                nodos.Add(c);
                id++;
                if (si)
                {
                    si = false;
                    contador += 1;
                }

                return p;
            }
            return null;
        }
        //epsilon ε
        public NodoG punto()
        {
            if (tokens[i].idToken == 15 || tokens[i].idToken == 19)
            {
                NodoG temp = new NodoG(id.ToString());
                nodos[id - 1].p = tokens[i].lexema;
                nodos[id - 1].primero = temp;
                nodos.Add(temp);
                id++;

                i++;
                return temp;
            }
            else
            {
                return llenar();
            }
            
        }

        public NodoG asterisco()
        {
            if (tokens[i].idToken == 15 || tokens[i].idToken == 19)
            {
                NodoG temp = new NodoG(id.ToString());
                nodos[id - 1].p = tokens[i].lexema;
                nodos[id - 1].primero = temp;
                nodos.Add(temp);
                id++;

                i++;
                return temp;
            }
            else
            {
                return llenar();
            }

        }

        public NodoG or()
        {
            if (tokens[i].idToken == 15 || tokens[i].idToken == 19)
            {
                NodoG temp = new NodoG(id.ToString());
                nodos[id - 1].p = tokens[i].lexema;
                nodos[id - 1].primero = temp;
                nodos.Add(temp);
                id++;

                i++;
                return temp;
            }
            else
            {
                return llenar();
            }

        }
        
        public void graficar()
        {
            StreamWriter escribir = new StreamWriter("Reportes\\"+nombre.lexema+"Grafo.txt");
            escribir.WriteLine("digraph D{\nrankdir=LR;");

            for (int j = 0; j < nodos.Count; j++)
            {
                NodoG n = nodos[j];
                escribir.WriteLine(n.nombre + "[label=\""+n.nombre+"\"];");
                if (n.primero != null) {
                    escribir.WriteLine(n.nombre + "->" + n.primero.nombre + "[label=\""+n.p+"\"];");
                }
                if (n.segundo != null)
                {
                    escribir.WriteLine(n.nombre + "->" + n.segundo.nombre + "[label=\"" + n.s + "\"];");
                }
            }
            escribir.WriteLine("}");

            escribir.Close();

            string texto = "/K dot -Tpng Reportes\\" + nombre.lexema + "Grafo.txt -o Reportes\\" + nombre.lexema + "Grafo.jpg";

            //System.Diagnostics.Process.Start("CMD.exe", "/K dot -Tpng Ella.txt -o UML.png");

            System.Diagnostics.Process.Start("CMD.exe", texto).Close();
            //System.Diagnostics.Process.Start("Reportes\\"+nombre.lexema+"Grafo.jpg");
        }

    }
}
