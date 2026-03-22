using System;
using Npgsql;

var adminConnStr = "User ID=postgres;Password=postgres;Database=lab_netmicro_policy;Host=localhost;Port=5432";

try
{
    using var conn = new NpgsqlConnection(adminConnStr);
    conn.Open();
    Console.WriteLine("Connected as postgres.");

    using var cmd = new NpgsqlCommand(@"
        GRANT USAGE ON SCHEMA public TO lab_user;
        GRANT CREATE ON SCHEMA public TO lab_user;
        CREATE TABLE IF NOT EXISTS outbox_messages (
            id SERIAL PRIMARY KEY,
            type VARCHAR(500),
            json_payload TEXT
        );
        GRANT ALL PRIVILEGES ON TABLE outbox_messages TO lab_user;
        GRANT USAGE, SELECT ON ALL SEQUENCES IN SCHEMA public TO lab_user;
    ", conn);
    cmd.ExecuteNonQuery();
    Console.WriteLine("Done! Table created and permissions granted.");
}
catch (Exception ex)
{
    Console.WriteLine($"Error: {ex.Message}");
}
