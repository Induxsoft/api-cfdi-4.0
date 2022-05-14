Public Class Form1
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim opf As New OpenFileDialog()
        opf.Filter = "Archivos XML|*.xml|Todos los archivos|*.*"

        If opf.ShowDialog() = DialogResult.OK Then
            TextBox1.Text = opf.FileName
        End If

    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Dim comprobante As New induxsoft.cfdi.v40.Comprobante()

        'Si el XML ya está sellado, solo debe indicar Id y pwd de Cuenta de timbrado
        comprobante.CuentaTimbradoInduxsoft = "SU ID DE CUENTA DE TIMBRADO INDUXSOFT"
        comprobante.ContrasenaCuentaTimbradoInduxsoft = "CONTRASEÑA DE LA CUENTA DE TIMBRADO INDUXSOFT"

        'Si el XML NO está sellado, debe indicar también los siguientes:
        'induxsoft.cfdi.v40.Comprobante.NIC = "SU NIC"
        'induxsoft.cfdi.v40.Comprobante.Licencia = "SU LICENCIA"
        'comprobante.UbicacionCertificado = "RUTA DEL ARCHIVO CER"
        'comprobante.UbicacionClavePrivada = "RUTA DEL ARCHIVO KEY"
        'comprobante.ContrasenaClavePrivada = "CONTRASEÑA DE LA CLAVE PRIVADA"
        'induxsoft.cfdi.v40.Comprobante.XSLT_CadenaOriginal = "RUTA DEL ARCHIVO DE LA cadenaoriginal.xslt"

        Try
            comprobante.Open(TextBox1.Text)

            Dim resultado = comprobante.Timbrar()
            If Convert.ToBoolean(resultado("success")) Then
                'Timbrado correctamente
                Dim xmlTimbrado As String

                xmlTimbrado = resultado("xml").ToString()
            Else
                MessageBox.Show(resultado("message").ToString())
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try

    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Dim comprobante As induxsoft.cfdi.v40.Comprobante

        Try
            comprobante = induxsoft.cfdi.v40.Comprobante.FromXml(TextBox2.Text)
            'Si el XML ya está sellado, solo debe indicar Id y pwd de Cuenta de timbrado
            comprobante.CuentaTimbradoInduxsoft = "SU ID DE CUENTA DE TIMBRADO INDUXSOFT"
            comprobante.ContrasenaCuentaTimbradoInduxsoft = "CONTRASEÑA DE LA CUENTA DE TIMBRADO INDUXSOFT"

            'Si el XML NO está sellado, debe indicar también los siguientes:
            'induxsoft.cfdi.v40.Comprobante.NIC = "SU NIC"
            'induxsoft.cfdi.v40.Comprobante.Licencia = "SU LICENCIA"
            'comprobante.UbicacionCertificado = "RUTA DEL ARCHIVO CER"
            'comprobante.UbicacionClavePrivada = "RUTA DEL ARCHIVO KEY"
            'comprobante.ContrasenaClavePrivada = "CONTRASEÑA DE LA CLAVE PRIVADA"
            'induxsoft.cfdi.v40.Comprobante.XSLT_CadenaOriginal = "RUTA DEL ARCHIVO DE LA cadenaoriginal.xslt"

            Dim resultado = comprobante.Timbrar()
            If Convert.ToBoolean(resultado("success")) Then
                'Timbrado correctamente
                Dim xmlTimbrado As String

                xmlTimbrado = resultado("xml").ToString()
            Else
                MessageBox.Show(resultado("message").ToString())
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub
End Class
