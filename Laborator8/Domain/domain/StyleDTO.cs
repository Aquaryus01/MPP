using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;


namespace Domain.domain
{
    [Serializable]
    public class StyleDTO
    {

        public StyleDTO(Distance distance, Style style)
        {
            this.Distance = Distance;
            this.Style = Style;
        }


        public Distance Distance { set; get; }
        public Style Style { set; get; }

        public override string ToString()
        {
            return "dist :" + Distance + "styl " + Style;
        }
    }
}
