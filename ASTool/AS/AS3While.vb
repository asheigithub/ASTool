﻿Namespace AS3
    Public Class AS3While
        Implements IAS3Stmt

        Public Condition As AS3StmtExpressions

        Public Body As List(Of IAS3Stmt)

        Public Sub Write(tabs As Integer, srcout As ISrcOut) Implements IAS3Stmt.Write

            Condition.Write(tabs, srcout)

            srcout.WriteLn("while (" & Condition.as3exprlist(Condition.as3exprlist.Count - 1).Value.ToString() & ")", tabs)
            srcout.WriteLn("{", tabs)
            For Each s In Body

                s.Write(tabs + 1, srcout)
            Next

            Condition.Write(tabs + 1, srcout)

            srcout.WriteLn("}", tabs)
        End Sub
    End Class
End Namespace

