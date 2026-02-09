Imports System.Runtime.CompilerServices

''' <summary>
''' 勤務日かどうか（PlanItems に開始・終了があるものが 1 件以上）
''' </summary>
Public Module AttendanceExtensions

    <Extension()>
    Public Function IsWorkingDay(model As AttendanceModel) As Boolean
        Return model.PlanItems.Any(Function(p) p.StartTime.HasValue AndAlso p.EndTime.HasValue) OrElse
               model.ExtraTasks.Any(Function(p) p.StartTime.HasValue AndAlso p.EndTime.HasValue)
    End Function

End Module