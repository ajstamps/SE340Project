using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SE340.Models
{
    public class Vehicle
    {
        public int ID { get; set; }

        [DisplayName("Make")]
        public string Make { get; set; }

        [DisplayName("Model")]
        public string Model { get; set; }

        [DisplayName("Drive Type")]
        public DriveType Drive { get; set; }

        [DisplayName("Engine Layout")]
        public EngineLayout Engine_Layout { get; set; }

        [DisplayName("Cylinder Count")]
        public int Num_cylinders { get; set; }

        [DisplayName("Displacement")]
        public float Displacement { get; set; }

        [DisplayName("Weight")]
        public float Weight { get; set; }
    }

    public enum DriveType
    {
        FRONT = 1,
        REAR = 2,
        ALL = 3
    }

    public enum EngineLayout
    {
        FRONT = 1,
        MID = 2,
        REAR = 3
    }
}
