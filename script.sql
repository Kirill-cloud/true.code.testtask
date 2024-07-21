START TRANSACTION;

CREATE UNIQUE INDEX "IX_Priorities_Level" ON "Priorities" ("Level");

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20240721112742_UniqueIndex', '8.0.4');

COMMIT;

