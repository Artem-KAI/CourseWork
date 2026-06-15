using System;

namespace BLL.Helpers;


public static class TeacherDegreeHelper
{
    public static string ValidateAndNormalizeDegree(string degree)
    {
        if (string.IsNullOrWhiteSpace(degree))
        {
            throw new ArgumentException("Науковий ступінь є обов'язковим.");
        }

        string d = degree.Trim().ToLower();
        return d switch
        {
            "bachelor" or "бакалавр" => "Бакалавр",
            "master" or "магістр" => "Магістр",
            "phd" or "доктор філософії" => "Доктор філософії",
            "кандидат наук" => "Кандидат наук",
            "doctorofscience" or "doctor of science" or "доктор наук" => "Доктор наук",
            "associateprofessor" or "associate professor" or "доцент" => "Доцент",
            "professor" or "професор" => "Професор",
            _ => throw new ArgumentException("Неприпустимий науковий ступінь. Допустимі варіанти: Бакалавр, Магістр, Доктор філософії, Кандидат наук, Доктор наук, Доцент, Професор.")
        };
    }

    public static readonly string[] AllowedDegrees = new[]
    {
        "Бакалавр",
        "Магістр",
        "Доктор філософії",
        "Кандидат наук",
        "Доктор наук",
        "Доцент",
        "Професор"
    };
}
