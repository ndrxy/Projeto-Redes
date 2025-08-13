namespace myBookSolution.Infrastructure.Migrations;

public abstract class DatabaseVersions
{
    public const int TABLE_USERS = 1;

    public const int TABLE_CURATORS = 2;

    public const int TABLE_AUTHORS_BOOKS = 3;

    public const int TABLE_REFRESH_TOKEN = 4;

    public const int TABLE_BORROWINGS = 5;
}
