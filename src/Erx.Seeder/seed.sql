CREATE TABLE Cities (Id INT, [Name] NVARCHAR(50), ShortName NVARCHAR(50));
INSERT INTO Cities VALUES (1, 'Bangkok', 'BKK'); 
INSERT INTO Cities VALUES (2, 'Phuket', 'PHK');
CREATE TABLE [Messages] (Id uniqueidentifier, [Message] NVARCHAR(50), [MappingId] INT null);
