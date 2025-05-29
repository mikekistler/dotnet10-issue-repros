using System.Text.Json.Serialization;

[JsonSerializable(typeof(Todo))]
[JsonSerializable(typeof(List<Todo>))]
[JsonSerializable(typeof(int))]
[JsonSerializable(typeof(string))]
internal partial class TodoJsonContext : JsonSerializerContext
{
}
