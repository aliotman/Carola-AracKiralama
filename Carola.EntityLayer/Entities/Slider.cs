using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carola.EntityLayer.Entities
{
    public class Slider
    {
        public int SliderId { get; set; }
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public string Description { get; set; }
        public string BackgroundImageUrl { get; set; }
        public bool IsActive { get; set; }
        public int Order { get; set; }
    }
}
