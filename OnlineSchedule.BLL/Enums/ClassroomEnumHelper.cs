using System;
using System.Collections.Generic;

namespace BLL.Enums;

/// <summary>
/// Допоміжний клас для забезпечення зв'язку між корпусами та готовими аудиторіями.
/// Дозволяє для вибраного корпусу отримувати перелік закріплених за ним аудиторій та валідувати їх сумісність.
/// </summary>
public static class ClassroomEnumHelper
{
    /// <summary>
    /// Повертає список готових назв аудиторій для обраного навчального корпусу.
    /// </summary>
    /// <param name="building">Навчальний корпус</param>
    /// <returns>Колекція готових аудиторій</returns>
    public static IReadOnlyCollection<ClassroomName> GetReadyClassroomNames(this ClassroomBuilding building)
    {
        return building switch
        {
            ClassroomBuilding.MainBuilding => new[] { ClassroomName.Room_101, ClassroomName.Room_102, ClassroomName.Room_103 },
            ClassroomBuilding.ScienceBuilding => new[] { ClassroomName.Room_201, ClassroomName.Room_202, ClassroomName.Room_203 },
            ClassroomBuilding.EngineeringBuilding => new[] { ClassroomName.Room_301, ClassroomName.Room_302, ClassroomName.Room_303 },
            ClassroomBuilding.ITBuilding => new[] { ClassroomName.Room_401, ClassroomName.Room_402, ClassroomName.Room_403 },
            _ => Array.Empty<ClassroomName>()
        };
    }

    /// <summary>
    /// Перевіряє, чи належить вибрана аудиторія вказаному навчальному корпусу.
    /// </summary>
    /// <param name="building">Навчальний корпус</param>
    /// <param name="room">Назва аудиторії</param>
    /// <returns>True, якщо аудиторія належить корпусу; інакше False</returns>
    public static bool IsValidRoomForBuilding(this ClassroomBuilding building, ClassroomName room)
    {
        var readyRooms = building.GetReadyClassroomNames();
        foreach (var r in readyRooms)
        {
            if (r == room)
            {
                return true;
            }
        }
        return false;
    }
}
