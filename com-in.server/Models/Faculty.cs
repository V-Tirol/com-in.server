﻿namespace com_in.server.Models
{
    public class Faculty
    {
        public int Id { get; set; }
        public int FacultyId {  get; set; }
        public string InstitutionalEmail {  get; set; }
        public string Department {  get; set; }
        public string Name {  get; set; }
        public string Position { get; set; }
        public bool isActive {  get; set; }
    }
}
