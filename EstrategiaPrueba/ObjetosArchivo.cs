using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using TradingPlatform.BusinessLayer;

namespace EstrategiaPrueba
{   
    [Serializable]    
    public class ObjetosArchivo
    {
        public string valor1;
        
        public Symbol simbolo;

        public ObjetosArchivo()
        {
            
        }

        public ObjetosArchivo(string valor1, Symbol simbolo)
        {
            this.valor1 = valor1;
            this.simbolo = simbolo;
        }


    }
}
