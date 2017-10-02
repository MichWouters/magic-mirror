using System;
using System.Collections.Generic;
using System.Text;

namespace MagicMirror.Business.Models.Cognitive
{
    public class FaceInfoModel : IModel
    {
        public Guid FaceId { get; set; }
        public double Age { get; set; }
        public string Gender { get; set; }
    }
}
