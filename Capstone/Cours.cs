//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Capstone
{
    using System;
    using System.Collections.Generic;
    
    public partial class Cours
    {
        public Cours()
        {
            this.Classrooms = new HashSet<Classroom>();
        }
    
        public int CourseID { get; set; }
        public string CourseName { get; set; }
        public short HoursPerSession { get; set; }
    
        public virtual ICollection<Classroom> Classrooms { get; set; }
    }
}