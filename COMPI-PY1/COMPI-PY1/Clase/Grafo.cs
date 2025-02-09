﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        public List<Token> transiciones { get; set; }
        public NodoG primero { get; set; }
        public NodoG ultimo { get; set; }
        public int id { get; set; }
        public int i { get; set; }
        public Token nombre { get; set; }
        int contador = 1;
        public Grafo()
        {
            this.nodos = new List<NodoG>();
            this.transiciones = new List<Token>();
            this.id = 0;
            this.i = 0;
        }

        public void comienzo(List<Token> t, Token nombre)
        {
            this.tokens = t;
            this.nombre = nombre;

            primero = new NodoG(id);
            nodos.Add(primero);
            id++;
            recursivo();

            //primero = llenar();
            //graficar();
        }
        
        public void graficar()
        {
            StreamWriter escribir = new StreamWriter("Reportes\\"+nombre.lexema+ "-AFN.txt");
            escribir.WriteLine("digraph D{\nrankdir=LR;");

            for (int j = 0; j < nodos.Count; j++)
            {
                NodoG n = nodos[j];
                
                escribir.WriteLine(n.nombre + "[label=\""+n.nombre+"\"];");
                if (n.primero != null) {
                    if (n.p.Length == 1 && n.p[0] == 10)
                    {
                        escribir.WriteLine(n.nombre + "->" + n.primero.nombre + "[label=\"-Salto de linea-\"];");
                    }
                    else if (n.p.Length == 1 && n.p[0] == 9)
                    {
                        escribir.WriteLine(n.nombre + "->" + n.primero.nombre + "[label=\"-Tabulacion-\"];");
                    }
                    else
                    {
                        escribir.WriteLine(n.nombre + "->" + n.primero.nombre + "[label=\"" + n.p + "\"];");
                    }
                }
                if (n.segundo != null)
                {
                    escribir.WriteLine(n.nombre + "->" + n.segundo.nombre + "[label=\"" + n.s + "\"];");
                }
            }
            escribir.WriteLine("}");

            escribir.Close();

            string texto = "/K dot -Tpng Reportes\\" + nombre.lexema + "-AFN.txt -o Reportes\\" + nombre.lexema + "-AFN.jpg";

            //System.Diagnostics.Process.Start("CMD.exe", "/K dot -Tpng Ella.txt -o UML.png");

            Process.Start("CMD.exe", texto).Close();
            //System.Diagnostics.Process.Start("Reportes\\"+nombre.lexema+"Grafo.jpg");
        }
        
        public NodoG recursivo()
        {

            if (tokens[i].idToken == 1)
            {
                i++;

                NodoG m = puntoR();

                NodoG u = puntoR();

                return m;
            }
            else if (tokens[i].idToken == 4)
            {
                i++;

                int pri = id - 1;

                NodoG s = new NodoG(id);
                int seg = id;
                nodos[pri].p = "ε";
                nodos[pri].primero = s;
                //s.atrasp = nodos[pri];
                nodos.Add(s);
                id++;

                NodoG t = asteriscoR();

                NodoG c = new NodoG(id);
                nodos[id - 1].p = "ε";
                nodos[id - 1].primero = s;
                //s.atrasp = nodos[id - 1];
                nodos[id - 1].s = "ε";
                nodos[id - 1].segundo = c;
                //c.atrass = nodos[id - 1];
                nodos[pri].s = "ε";
                nodos[pri].segundo = c;
                //c.atrass = nodos[pri];
                nodos.Add(c);
                id++;

                return s;
            }
            else if (tokens[i].idToken == 2)
            {
                i++;
                int p = id - 1;

                NodoG s1 = new NodoG(id);
                nodos[p].p = "ε";
                nodos[p].primero = s1;
                //s1.atrasp = nodos[p];

                nodos.Add(s1);
                id++;

                NodoG t1 = orR();

                NodoG c = new NodoG(-1);
                nodos[id - 1].p = "ε";
                nodos[id - 1].primero = c;
                //c.atrasp = nodos[id - 1];

                NodoG s2 = new NodoG(id);
                nodos[p].s = "ε";
                nodos[p].segundo = s2;
                //s1.atrass = nodos[p];
                nodos.Add(s2);
                id++;

                NodoG t2 = orR();

                c.id = id;
                c.nombre = id.ToString();
                nodos[id - 1].p = "ε";
                nodos[id - 1].primero = c;
                //c.atrass = nodos[id - 1];
                nodos.Add(c);
                id++;

                return nodos[p];
            }
            else if (tokens[i].idToken == 3)
            {
                i++;
                int p = id - 1;

                NodoG s1 = new NodoG(id);
                nodos[p].p = "ε";
                nodos[p].primero = s1;
                nodos.Add(s1);
                id++;

                NodoG t1 = orR();

                NodoG c = new NodoG(-1);
                nodos[id - 1].p = "ε";
                nodos[id - 1].primero = c;

                //comentar s2 y t2 para omitir epxilons y ser directo
                NodoG s2 = new NodoG(id);
                nodos[p].s = "ε";
                nodos[p].segundo = s2;
                nodos.Add(s2);
                id++;

                NodoG t2 = new NodoG(id);
                nodos[id - 1].p = "ε";
                nodos[id - 1].primero = t2;
                nodos.Add(t2);
                id++;

                //cambiar tambien (id-1 por p) y (p por s) y (primero por segundo)
                c.nombre = id.ToString();
                c.id = id;
                nodos[id - 1].p = "ε";
                nodos[id - 1].primero = c;
                nodos.Add(c);
                id++;

                return nodos[p];
            }
            else if (tokens[i].idToken == 5)
            {
                i++;
                int iant = i;

                //int cominezo = i;
                int pri = id;
                NodoG p = puntoR();

                List<Token> cop = new List<Token>();
                for (int j = iant; j < i; j++)
                {
                    cop.Add(tokens[j]);
                }

                //graficar();

                int seg = id - 1;
                Grafo copia = copiar(cop);

                for (int i = 0; i < copia.nodos.Count; i++)
                {
                    int k = copia.nodos[i].id + id;
                    copia.nodos[i].id = k;
                    copia.nodos[i].nombre = k.ToString();
                    nodos.Add(copia.nodos[i]);
                }
                id += copia.nodos.Count;

                //for (int i = seg+1; i < nodos.Count; i++)
                //{
                //    if (nodos[i].pid != -1)
                //    {
                //        nodos[i].primero = nodos[nodos[i].pid];
                //    }

                //    if (nodos[i].sid != -1)
                //    {
                //        nodos[i].segundo = nodos[nodos[i].sid];
                //    }
                //}

                nodos[seg].p = "ε";
                nodos[seg].primero = copia.primero;
                //copia.atrasp = nodos[seg];

                //graficar();

                NodoG c = new NodoG(id);

                nodos[seg].s = "ε";
                nodos[seg].segundo = c;
                //c.atrass = nodos[seg];

                nodos[id - 1].p = "ε";
                nodos[id - 1].primero = copia.primero;
                //copia.atrasp = nodos[id - 1];

                nodos[id - 1].s = "ε";
                nodos[id - 1].segundo = c;
                //c.atrass = nodos[id - 1];

                //nodos[pri].s = "ε";
                //nodos[pri].segundo = c;

                nodos.Add(c);
                id++;
                //graficar();

                return p;
            }
            else
            {
                puntoR();
            }
            return null;
        }
        
        public Grafo copiar(List<Token> t) {
            Grafo temp = new Grafo();
            temp.comienzo(t, new Token(-1,-1,"asdf","pruebas"+i,-1,-1));
            
            return temp;
        }

        public NodoG puntoR()
        {
            if (tokens[i].idToken == 15 || tokens[i].idToken == 19)
            {
                NodoG temp = new NodoG(id);
                nodos[id - 1].p = tokens[i].lexema;
                agregart(tokens[i]);
                nodos[id - 1].primero = temp;
                nodos[id - 1].idp = tokens[i].idToken;
                //temp.atrasp = nodos[id - 1];
                nodos.Add(temp);
                id++;

                i++;
                return temp;
            }
            else
            {
                return recursivo();
            }

        }

        public NodoG asteriscoR()
        {
            if (tokens[i].idToken == 15 || tokens[i].idToken == 19)
            {
                NodoG temp = new NodoG(id);
                nodos[id - 1].p = tokens[i].lexema;
                agregart(tokens[i]);
                nodos[id - 1].primero = temp;
                nodos[id - 1].idp = tokens[i].idToken;
                //temp.atrasp = nodos[id - 1];
                nodos.Add(temp);
                id++;

                i++;
                return temp;
            }
            else
            {
                return recursivo();
            }

        }

        public NodoG orR()
        {
            if (tokens[i].idToken == 15 || tokens[i].idToken == 19)
            {
                NodoG temp = new NodoG(id);
                nodos[id - 1].p = tokens[i].lexema;
                agregart(tokens[i]);
                nodos[id - 1].primero = temp;
                nodos[id - 1].idp = tokens[i].idToken;
                //temp.atrasp = nodos[id - 1];
                nodos.Add(temp);
                id++;

                i++;
                return temp;
            }
            else
            {
                return recursivo();
            }

        }

        public void agregart(Token nuevo) {
            bool esta = true;
            foreach (Token item in transiciones)
            {
                if (nuevo.lexema.Equals(item.lexema) && nuevo.idToken == 15) {
                    esta = false;
                }
                else if (nuevo.lexema.Equals(item.lexema) && nuevo.idToken == 19)
                {
                    esta = false;
                }
            }

            if (esta)
            {
                transiciones.Add(nuevo);
            }
        }
    }
}
