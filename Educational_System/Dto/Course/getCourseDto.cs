using System;
using System.Collections.Generic;

namespace Educational_System.Dto.Course
{
    public class CourseSubLessons
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public List<CourseLessons> Lessons { get; set; } = new List<CourseLessons>();
    }

    public class CourseLessons
    {
        public string LessonTitle { get; set; }
        public string Content { get; set; }
        public int LessonOrder { get; set; }
        public string LessonDescription { get; set; }
    }

    public class getCourseDto
    {
        public int ID { get; set; }
        public string CourseTitle { get; set; }
        public string Description { get; set; }
        public string InstructorName { get; set; }
        public string CategoryName { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ThumbnailURL { get; set; }
        public decimal TotalAmount { get; set; }

        // Discount Details
        public DateTime DiscountStartDate { get; set; }
        public DateTime DiscountEndDate { get; set; }
        public decimal DiscountValue { get; set; }

        public bool IsDiscountValid => DateTime.Now >= DiscountStartDate && DateTime.Now <= DiscountEndDate;
        public decimal TotalAmountAfterDiscount =>
            IsDiscountValid ? TotalAmount - ((DiscountValue/100) * TotalAmount) : TotalAmount;

        // Sub-lessons
        public List<CourseSubLessons> SubLessons { get; set; } = new List<CourseSubLessons>();
    }
}
