CREATE TABLE [dbo].[Users](
	[Id] BIGINT PRIMARY KEY CLUSTERED IDENTITY(1,1),
	[UserName] [nvarchar](256) UNIQUE NOT NULL,
	[Email] [nvarchar](256) UNIQUE NOT NULL,
	[PasswordHash] [nvarchar](max) NOT NULL
)

CREATE TABLE [dbo].[Posts](
	[Id] BIGINT PRIMARY KEY CLUSTERED IDENTITY(1,1),
	[Title] [nvarchar](55) NOT NULL,
	[Description] [nvarchar](250) NULL,
	[CreatedDate] DATETIME NOT NULL,
	[UserId] BIGINT NOT NULL REFERENCES [Users](Id),
	[PostVisibility] INT NOT NULL, 
	-- ^^^ cum facem pt grupuri (exista vizibilitatea grupuri, dar in cazul ala ar mai trebui ceva)
	[PostTag] INT NULL
)

CREATE TABLE [dbo].[Reactions](
	[UserId] BIGINT NOT NULL REFERENCES [Users]([Id]),
	[PostId] BIGINT NOT NULL REFERENCES[Posts]([Id]),
	[ReactionType] INT NOT NULL

	PRIMARY KEY CLUSTERED ([UserId],[PostId]) 
)

CREATE TABLE [dbo].[Comments](
	[Id] BIGINT PRIMARY KEY CLUSTERED IDENTITY(1,1),
	[UserId] BIGINT NOT NULL REFERENCES[Users]([Id]),
	[PostId] BIGINT NOT NULL REFERENCES[Posts]([Id]),
	[Content] [nvarchar](250) NOT NULL,
	[CreatedDate] DATETIME NOT NULL
)

CREATE TABLE [dbo].[Groups](
	[Id] BIGINT PRIMARY KEY CLUSTERED IDENTITY(1,1),
	[Name] [nvarchar](55) UNIQUE NOT NULL,
	[AdminId] BIGINT NOT NULL REFERENCES [Users]([Id])
)

CREATE TABLE [dbo].[GroupUsers](
	[UserId] BIGINT NOT NULL REFERENCES[Users]([Id]),
	[GroupId] BIGINT NOT NULL REFERENCES[Groups]([Id]),
	PRIMARY KEY CLUSTERED ([UserId],[GroupId]) 
)

CREATE TABLE [dbo].[UserFollowers](
	[UserId] BIGINT NOT NULL REFERENCES[Users]([Id]),
	[FollowerId] BIGINT NOT NULL REFERENCES[Users]([Id]),
	PRIMARY KEY CLUSTERED ([UserId],[FollowerId])
)