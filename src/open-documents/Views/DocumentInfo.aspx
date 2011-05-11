<%@ Page Language="C#" Inherits="OpenRasta.Codecs.WebForms.ResourceView<Open.Documents.DocumentInfo>" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<script runat="server">

</script>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title></title>
</head>
<body>
<h1><%= Resource.FileName %></h1>
<div>
<dl>
  <dt>Last modified time</dt>
  <dd><%= Resource.LastModifiedTimeUTC %></dd>
  <dt>Id</dt>
  <dd><%= Resource.Id %></dd>
  <dt>Data size</dt>
  <dd><%= Resource.Data.Length %></dd>
</dl>
<a href="<%= Resource.DataHref %>">Download</a>
</div>
</body>
</html>
