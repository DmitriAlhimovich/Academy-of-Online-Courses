﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Students.DAL.Models
{
    public class Lesson
    {
        [Key]
        public int LessonId { get; set; }
        public string Name { get; set; }
        public int NumberLesson { get; set; }
        public string Homework { get; set; }
        public string Description { get; set; }
        public int CourseId { get; set; }
        public Course Course { get; set; }
        public List<Assessment> Assessments { get; set; }
    }
}
