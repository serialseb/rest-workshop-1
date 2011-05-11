<%@ Page Language="C#" Inherits="OpenRasta.Codecs.WebForms.ResourceView<Open.Documents.DocumentLibrary>" 
%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">


<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Document library</title>
</head>
<body>
  <% using(scope(Xhtml
  .Form(Resource)
  .EncType(MediaType.MultipartFormData)
  .Method("POST"))) { %>
    <fieldset>
      <input type="email" name="author" />
      <input type="file" name="sentFile" />
      <input type="submit" />
    </fieldset>
  <% } %>
</body>
</html>
