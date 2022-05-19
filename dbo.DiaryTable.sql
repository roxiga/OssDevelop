CREATE TABLE [dbo].[DiaryTable] (
    [Id]    INT            IDENTITY (1, 1) NOT NULL,
    [date]  INT            NULL,
    [title] NVARCHAR (50)  NULL,
    [text]  NVARCHAR (MAX) NULL,
    [image] IMAGE          NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

