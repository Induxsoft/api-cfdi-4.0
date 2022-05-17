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
        comprobante.CuentaTimbradoInduxsoft = "ID DE CUENTA DE TIMBRADO"
        comprobante.ContrasenaCuentaTimbradoInduxsoft = "CONTRASEÑA DE CUENTA DE TIMBRADO"

        'Si el XML NO está sellado, debe indicar también los siguientes:
        'induxsoft.cfdi.v40.Comprobante.NIC = "SU NIC"
        'induxsoft.cfdi.v40.Comprobante.Licencia = "SU LICENCIA"
        'comprobante.UbicacionCertificado = "RUTA DE CERTIFICADO DE SELLO DIGITAL"
        'comprobante.UbicacionClavePrivada = "RUTA DE LLAVE PRIVADA"
        'comprobante.ContrasenaClavePrivada = "CONTRASEÑA DE LLAVE PRIVADA"
        'induxsoft.cfdi.v40.Comprobante.XSLT_CadenaOriginal = "RUTA DE CADENAORIGINAL.xslt"

        Try
            TextBox3.Text = ""

            comprobante.Open(TextBox1.Text)

            Dim resultado = comprobante.Timbrar()
            Dim xmlTimbrado As String = System.Text.UTF8Encoding.UTF8.GetString(Convert.FromBase64String(resultado("xml").ToString()))
            TextBox3.Text = xmlTimbrado

        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try

    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Dim comprobante As induxsoft.cfdi.v40.Comprobante

        Try
            TextBox3.Text = ""

            comprobante = induxsoft.cfdi.v40.Comprobante.FromXml(TextBox2.Text)
            'Si el XML ya está sellado, solo debe indicar Id y pwd de Cuenta de timbrado
            comprobante.CuentaTimbradoInduxsoft = "xipova"
            comprobante.ContrasenaCuentaTimbradoInduxsoft = "123456"
            'Si el XML NO está sellado, debe indicar también los siguientes:
            'induxsoft.cfdi.v40.Comprobante.NIC = "SU NIC"
            'induxsoft.cfdi.v40.Comprobante.Licencia = "SU LICENCIA"
            'comprobante.UbicacionCertificado = "RUTA DEL ARCHIVO CER"
            'comprobante.UbicacionClavePrivada = "RUTA DEL ARCHIVO KEY"
            'comprobante.ContrasenaClavePrivada = "CONTRASEÑA DE LA CLAVE PRIVADA"
            'induxsoft.cfdi.v40.Comprobante.XSLT_CadenaOriginal = "RUTA DEL ARCHIVO DE LA cadenaoriginal.xslt"

            Dim resultado = comprobante.Timbrar()
            Dim xmlTimbrado As String = System.Text.UTF8Encoding.UTF8.GetString(Convert.FromBase64String(resultado("xml").ToString()))
            TextBox3.Text = xmlTimbrado


        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    Private Sub TextBox2_TextChanged(sender As Object, e As EventArgs) Handles TextBox2.TextChanged

    End Sub
End Class
