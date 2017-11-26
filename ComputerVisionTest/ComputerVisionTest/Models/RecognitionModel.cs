using System;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace ComputerVisionTest.Models
{
    public class RecognitionModel
    {
        [Key]
        public int Id { get; set; }
        public String Key { get; set; }

        [DataType(DataType.Upload)]
        public HttpPostedFileBase ImageUpload { get; set; }
    }
}