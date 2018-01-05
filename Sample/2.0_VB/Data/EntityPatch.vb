Imports System.Linq
Imports System.Reflection
Imports Newtonsoft.Json.Linq
Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema

Namespace MyApp.Data
    Public NotInheritable Class EntityPatch
        Private Sub New()
        End Sub
        Public Shared Sub Apply(obj As Object, patch As JObject)
            Dim typeInfo = obj.[GetType]().GetTypeInfo()

            For Each [property] As KeyValuePair(Of String, JToken) In patch
                Dim propertyInfo = typeInfo.GetProperty([property].Key)

                If propertyInfo IsNot Nothing Then
                    Dim computedAttribute = propertyInfo.GetCustomAttributes(GetType(DatabaseGeneratedAttribute), False).Cast(Of DatabaseGeneratedAttribute)().FirstOrDefault()
                    If computedAttribute Is Nothing Then
                        Dim value = [property].Value.ToObject(propertyInfo.PropertyType)
                        propertyInfo.SetValue(obj, value)
                    End If
                End If
            Next
        End Sub

    End Class
End Namespace
