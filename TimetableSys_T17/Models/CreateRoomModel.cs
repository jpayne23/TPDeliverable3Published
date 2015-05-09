using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace TimetableSys_T17.Models
{
    public class CreateRoomModel
    {
        [Required]
        [StringLength(8)]
        public string roomCode { get; set; }
        [Required]
        public int buildingID { get; set; }
        [Required]
        public int capacity { get; set; }
        [Required]
        public bool lab { get; set; }
        [Required]
        public int @private { get; set; }


    }
}

//TO DO: - Specify data types more accurately


//using System;
//using System.Data.Entity;


//namespace Test1.Models
//{
//    public class Movie
//    {
//        public int ID { get; set; }
//        public string Title { get; set; }
//        public DateTime ReleaseDate { get; set; }
//        public string Genre { get; set; }
//        public decimal Price { get; set; }
//    }

//    public class MovieDBContext : DbContext 
//    {
//        public DbSet<Movie> Movies { get; set; }
//    }
//}