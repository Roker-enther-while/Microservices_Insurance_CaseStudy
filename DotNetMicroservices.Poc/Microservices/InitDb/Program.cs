using System;
using Npgsql;

var adminConnStr = "User ID=lab_user;Password=lab_pass;Database=lab_netmicro_policy;Host=localhost;Port=5432";

try
{
    using var conn = new NpgsqlConnection(adminConnStr);
    conn.Open();
    Console.WriteLine("Connected as lab_user.");

    using var cmd = new NpgsqlCommand(@"
        CREATE SCHEMA IF NOT EXISTS lab_user AUTHORIZATION lab_user;
        GRANT ALL ON SCHEMA lab_user TO lab_user;
    ", conn);
    cmd.ExecuteNonQuery();
    Console.WriteLine("Done! lab_user schema created.");
}
catch (Exception ex)
{
    Console.WriteLine($"Error: {ex.Message}");
}
