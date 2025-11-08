namespace CookbookDB.Models;

/// <summary>
/// списки рецептов пользователей
/// </summary>
public partial class List
{
    /// <summary>
    /// идентификатор списка
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// название списка
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// идентификатор пользователя
    /// </summary>
    public int? UserId { get; set; }

    /// <summary>
    /// описание
    /// </summary>
    public string? Description { get; set; }

    public virtual User? User { get; set; }
}
