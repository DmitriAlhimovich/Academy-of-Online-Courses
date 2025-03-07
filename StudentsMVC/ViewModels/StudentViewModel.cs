﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Students.MVC.ViewModels
{
    public class StudentViewModel: PersonViewModel
    {
        public int StudentId { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "Дата рождения")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DateOfBirth { get; set; }
        [Display(Name = "Номер группы")]
        public int? GroupId { get; set; }
        public GroupViewModel Group { get; set; }
        public List<AssessmentViewModel> Assessments { get; set; }

        public string GetFullName => $"{Surname} {Name} {MiddleName}";
    }
}
