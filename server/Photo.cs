using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace server
{
    public class PhotoImg
    {
        public PhotoImg()
        { }

        public int Id { get; set; }
        public string UserId { get; set; }
        public string Photo { get; set; }
        public string Description { get; set; }
        public string Tags { get; set; }
        /*public string PhotoId { get; set; } DESCOMENTAR LUEGO */
    }
}
